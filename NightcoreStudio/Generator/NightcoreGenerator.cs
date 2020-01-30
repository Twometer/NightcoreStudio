﻿using AutoNightcore.Effects;
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
            touchSource.SetRate(1.0f + options.Factor);

            GenerateVideo(touchSource);

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

                var length = source.GetLength().TotalSeconds;

                var totalFrames = length * options.Fps * (1.0f - options.Factor);

                Console.WriteLine($"Stats:\n Total Seconds: {length}\n Fps: {options.Fps}");

                using (var renderer = new GLRenderer())
                {
                    Console.WriteLine("Starting renderer...");
                    renderer.Create();
                    var image = Image.FromFile(options.WallpaperFile.FullName) as Bitmap;
                    wallpaperTexture = renderer.LoadTexture(image);

                    Console.WriteLine("Generating video...");

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

                            if (f % 60 == 0)
                            {
                                Console.WriteLine($"Progress: {f / (double)totalFrames * 100} %");
                            }

                            f++;
                        }
                        else
                        {
                            var read = source.Read(audioData, 0, audioData.Length);
                            if (read > 0)
                                writer.WriteAudioFrame(audioData);
                            else break;
                        }
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
