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

            return true;
        }

        private void GenerateVideo(IWaveSource wave)
        {
            var vfw = new VideoFileWriter
            {
                FrameRate = options.Fps,
                BitRate = 8 * 1024 * 1024,
                VideoCodec = VideoCodec.Mpeg4,
                Width = 1920,
                Height = 1080
            };
            vfw.Open(options.OutputFile.FullName);

            var glRenderer = new GLRenderer();
            glRenderer.Create();
            var image = Image.FromFile(options.WallpaperFile.FullName);
            var texture = glRenderer.LoadTexture(image);

            var totalFrames = wave.GetLength().TotalSeconds * options.Fps;

            for (var i = 0; i < totalFrames; i++)
            {
                RenderFrame(i, glRenderer, wave);

                var frame = glRenderer.Snapshot();
                vfw.WriteVideoFrame(frame);
            }

            glRenderer.Dispose();
            vfw.Close();
        }

        private void RenderFrame(int frame, GLRenderer renderer, IWaveSource wave)
        {
            renderer.DrawImage(wallpaperTexture, 0, 0, 1920, 1080);
        }


        /*     pitchSource.SetPitch(1.0f + this.options.Factor);
             tempoSource.SetTempo(1.0f + this.options.Factor * 5);

             Console.WriteLine("Analyzing...");
             var doptions = DetectorOptions.Default;
             var detector = new OnsetDetector(doptions, this);
             var onsets = detector.Detect(audioSource);
             Console.WriteLine("Detected " + onsets.Count + " onsets");
             foreach (var os in onsets)
             {
                 if (os.OnsetAmplitude > 15)
                     SendWithDelay(os);

             }

                     var so = new CSCore.SoundOut.WasapiOut();
        so.Initialize(waveSource);
        so.Play();
        so.Stopped += So_Stopped

        var lastpos = 0f;
        while (true)
        {
            var pos = waveSource.GetPosition().TotalSeconds;
            foreach (var detect in onsets)
            {
                if (Math.Abs(Math.Round(detect.OnsetTime, 1) - Math.Round(pos, 1)) < 0.1f)
                {
                    if (detect.OnsetTime == lastpos)
                    {
                        continue;
                    }
                    Console.WriteLine(detect.OnsetAmplitude);
                    lastpos = detect.OnsetTime;
                }
            }
            System.Threading.Thread.Sleep(1);
        }*/



        /*using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(audioSource.WaveFormat, "output.mp3"))
        {
            byte[] buffer = new byte[audioSource.WaveFormat.BytesPerSecond];
            int read;
            while ((read = waveSource.Read(buffer, 0, buffer.Length)) > 0)
            {
                encoder.Write(buffer, 0, read);
            }
        }*/

    }
}
