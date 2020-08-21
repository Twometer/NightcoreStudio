using System;
using System.Drawing;
using CSCore;
using CSCore.Codecs;
using CSVideo.Writer;
using SoundTouchPitchAndTempo;
using NightcoreStudio.Renderer;
using NightcoreStudio.Effects.Base;

namespace NightcoreStudio.Generator
{
    public class NightcoreGenerator
    {
        public Action<double> ProgressHandler { get; set; }

        private GeneratorOptions options;

        private EffectChain effectChain;

        private Texture wallpaperTexture;

        public NightcoreGenerator(GeneratorOptions options)
        {
            this.options = options;
            this.effectChain = new EffectChain();
        }

        public void AddEffect(IEffect effect)
        {
            effectChain.Add(effect);
        }

        public bool Generate()
        {
            var audioSource = CodecFactory.Instance.GetCodec(options.AudioFile.FullName)
                .ToSampleSource()
                .AppendSource(s => new SoundTouchSource(s, 50), out var touchSource);
            touchSource.SetRate(options.Factor);

            GenerateVideo(audioSource);

            return true;
        }

        private void GenerateVideo(ISampleSource source)
        {
            Console.WriteLine("Initializing video writer...");
            using (var writer = new VideoWriter(options.OutputFile.FullName))
            {
                writer.Width = 1920;
                writer.Height = 1080;
                writer.Fps = options.Fps;
                writer.AudioSampleRate = source.WaveFormat.SampleRate;
                writer.Open();

                double totalFrames = FrameMath.CalculateTotalFrames(source, options);

                Console.WriteLine("Initializing renderer...");
                using (var renderer = new GLRenderer(false))
                {
                    var wallpaper = Image.FromFile(options.WallpaperFile.FullName) as Bitmap;
                    wallpaperTexture = renderer.LoadTexture(wallpaper);
                    effectChain.Initialize(source.ToMono(), options);

                    Console.WriteLine("Generating video...");
                    float[] sampleBuffer = new float[writer.AudioSamplesPerFrame];

                    var frameNumber = 0;
                    while (true)
                    {
                        if (writer.WriteVideo)
                        {
                            renderer.Clear();
                            RenderFrame(renderer, new Frame(frameNumber, TimeSpan.FromSeconds(frameNumber / (double)options.Fps)));

                            var frame = renderer.Snapshot();
                            writer.WriteVideoFrame(frame);

                            if (frameNumber % 60 == 0)
                            {
                                ProgressHandler?.Invoke(Math.Round(frameNumber / totalFrames * 100.0, 2));
                            }

                            frameNumber++;
                        }
                        else
                        {
                            var read = source.Read(sampleBuffer, 0, sampleBuffer.Length);
                            if (read > 0)
                                writer.WriteAudioFrame(sampleBuffer);
                            else break;
                        }
                    }
                    ProgressHandler?.Invoke(Math.Round(frameNumber / totalFrames * 100.0, 2));
                }
            }
        }

        private void RenderFrame(IRenderer renderer, Frame frame)
        {
            renderer.DrawImage(wallpaperTexture, 0, 0, 1920, 1080);
            effectChain.Apply(renderer, frame, EffectStage.PreProcess);
            effectChain.Apply(renderer, frame, EffectStage.Main);
            effectChain.Apply(renderer, frame, EffectStage.PostProcess);
        }

    }
}
