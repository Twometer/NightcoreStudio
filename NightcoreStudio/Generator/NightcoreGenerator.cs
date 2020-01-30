using AutoNightcore.Effects;
using AutoNightcore.Renderer;
using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.WAV;
using CSCore.MediaFoundation;
using CSVideo.Writer;
using SoundTouchPitchAndTempo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoNightcore.Generator
{
    public class NightcoreGenerator
    {
        private GeneratorOptions options;

        private EffectChain effectChain;

        private Texture wallpaperTexture;

        public NightcoreGenerator(GeneratorOptions options)
        {
            this.options = options;
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
            touchSource.SetRate(1.0f + (options.Factor / 100.0f));

            var waveSource = audioSource.ToWaveSource()
                .ToSampleSource();

            GenerateVideo(waveSource);

            return true;
        }

        private void GenerateVideo(ISampleSource source)
        {
            using (var writer = new VideoWriter(options.OutputFile.FullName))
            {
                writer.Fps = options.Fps;
                writer.Open();

                var length = source.GetLength().TotalSeconds;

                Console.WriteLine($"Stats:\n Total Seconds: {length}\n Fps: {options.Fps}");

                using (var renderer = new GLRenderer())
                {
                    Console.WriteLine("Starting renderer...");
                    renderer.Create();

                    Console.WriteLine("Loading image...");
                    var image = Image.FromFile(options.WallpaperFile.FullName) as Bitmap;
                    wallpaperTexture = renderer.LoadTexture(image);

                    Console.WriteLine("Init complete");

                    Thread.Sleep(1000);

                    float[] audioData = new float[writer.AudioSamplesPerFrame];

                    var f = 0;
                    while (true)
                    {
                        if (writer.WriteVideo)
                        {
                            renderer.Clear();
                            RenderFrame(renderer, new Frame(f, TimeSpan.FromSeconds(f / (double)options.Fps)));

                            var frame = renderer.Snapshot();
                            writer.WriteVideoFrame(frame);
                            frame.Dispose();
                        }
                        else
                        {
                            var read = source.Read(audioData, 0, audioData.Length);
                            if (read > 0)
                                writer.WriteAudioFrame(audioData);
                            else break;
                        }

                        f++;
                    }
                }
            }
        }

        private void RenderFrame(IRenderer renderer, Frame frame)
        {
            renderer.DrawImage(wallpaperTexture, 0, 0, 1920, 1080);
        }

    }
}
