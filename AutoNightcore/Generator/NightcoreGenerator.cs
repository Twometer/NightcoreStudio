using AutoNightcore.Effects;
using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.WAV;
using CSCore.MediaFoundation;
using OnsetDetection;
using SoundTouchPitchAndTempo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Generator
{
    public class NightcoreGenerator : IProgress<string>
    {
        private GeneratorOptions options;

        private List<IEffect> effects;

        public NightcoreGenerator(GeneratorOptions options)
        {
            this.options = options;
        }

        public void AddEffect(IEffect effect)
        {
            effects.Add(effect);
        }

        public bool Generate()
        {
            var audioSource = CodecFactory.Instance.GetCodec(options.AudioFile.FullName)
                .ToSampleSource()
                .AppendSource(s => new SoundTouchSource(s, 50), out var touchSource);
            touchSource.SetRate(1.20f);


            var waveSource = audioSource.ToWaveSource();


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
                 }*/


            var so = new CSCore.SoundOut.WasapiOut();
            so.Initialize(waveSource);
            so.Play();
            so.Stopped += So_Stopped;


            /*
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

            return true;
        }

        private async void SendWithDelay(Onset onset)
        {
            await Task.Delay((int)(onset.OnsetTime * 1000f));
            Console.WriteLine("-> " + onset.OnsetAmplitude);
        }

        private void So_Stopped(object sender, CSCore.SoundOut.PlaybackStoppedEventArgs e)
        {

            Console.WriteLine("playback stopped ");

        }

        public void Report(string value)
        {
            Console.WriteLine(value);
        }
    }
}
