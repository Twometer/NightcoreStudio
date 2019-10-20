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

        public NightcoreGenerator(GeneratorOptions options)
        {
            this.options = options;
        }

        public bool Generate()
        {
            var audioSource = CodecFactory.Instance.GetCodec(options.AudioFile.FullName)
                .ToSampleSource()
                .AppendSource(x => new SoundTouchSource(x, 50), out SoundTouchSource pitchSource)
                .AppendSource(x => new SoundTouchSource(x, 50), out SoundTouchSource tempoSource);

            var waveSource =                 audioSource.ToWaveSource();

            pitchSource.SetPitch(1.0f + options.Factor);
            tempoSource.SetTempo(1.0f + options.Factor);

            Console.WriteLine("Analyzing...");
            var detector = new OnsetDetector(DetectorOptions.Default, this);
            var onsets = detector.Detect(audioSource);
            Console.WriteLine("Detected " + onsets.Count + " onsets");
            foreach (var os in onsets)
            {
                Console.WriteLine(os.OnsetTime);
            }

            waveSource.Position = 0;

            var so = new CSCore.SoundOut.WasapiOut();
            so.Initialize(waveSource);
            
            so.Play();

            so.Stopped += So_Stopped;

           

            while (true)
            {
                var pos = waveSource.GetPosition().TotalSeconds;
                foreach (var detect in onsets)
                {
                    if(Math.Abs(Math.Round(detect.OnsetTime,1) - Math.Round(pos, 1)) < 0.5f)
                    {
                        Console.WriteLine(detect.OnsetAmplitude);
                    }
                }
                System.Threading.Thread.Sleep(500);
            }



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
