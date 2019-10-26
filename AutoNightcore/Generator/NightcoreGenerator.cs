using Accord.Video.FFMPEG;
using AutoNightcore.Effects;
using AutoNightcore.Renderer;
using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.WAV;
using CSCore.MediaFoundation;
using OnsetDetection;
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

            var waveSource = audioSource.ToWaveSource();

            GenerateVideo(waveSource);
            GenerateAudio(waveSource);

            return true;
        }

        private void GenerateAudio(IWaveSource wave)
        {
            using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(wave.WaveFormat, "__tmp.mp3"))
            {
                byte[] buffer = new byte[wave.WaveFormat.BytesPerSecond];
                int read;
                while ((read = wave.Read(buffer, 0, buffer.Length)) > 0)
                {
                    encoder.Write(buffer, 0, read);
                }
            }
        }

        private void GenerateVideo(IWaveSource wave)
        {
            var vfw = new VideoFileWriter
            {
                FrameRate = options.Fps,
                BitRate = 8 * 1024 * 1024,
                VideoCodec = VideoCodec.Mpeg4,
                Width = 1920,
                Height = 1080,
            };
            vfw.FrameRate = new Accord.Math.Rational(options.Fps);
            vfw.Open(options.OutputFile.FullName);

            var totalFrames = wave.GetLength().TotalSeconds * options.Fps;
            Console.WriteLine($"Stats:\n Total Seconds: {wave.GetLength().TotalSeconds}\n Fps: {options.Fps}\n Frames: {totalFrames}");

            Console.WriteLine("Starting renderer...");
            var glRenderer = new GLRenderer();
            glRenderer.Create();

            Console.WriteLine("Loading image...");
            var image = Image.FromFile(options.WallpaperFile.FullName) as Bitmap;
            wallpaperTexture = glRenderer.LoadTexture(image);

            Console.WriteLine("Init complete");

            Thread.Sleep(1000);

            for (var i = 0; i < totalFrames; i++)
            {
                glRenderer.Clear();
                RenderFrame(glRenderer, new Frame(i, TimeSpan.FromSeconds(i / (double)options.Fps)));

                var frame = glRenderer.Snapshot();
                vfw.WriteVideoFrame(frame);
                frame.Dispose();
            }

            glRenderer.Dispose();
            vfw.Close();
        }

        private void RenderFrame(IRenderer renderer, Frame frame)
        {
            renderer.DrawImage(wallpaperTexture, 0, 0, 1920, 1080);
        }

    }
}
