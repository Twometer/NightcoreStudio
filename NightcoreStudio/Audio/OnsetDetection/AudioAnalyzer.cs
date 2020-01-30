using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Audio.OnsetDetection
{
    public class AudioAnalyzer
    {
        private ISampleSource source;

        private OnsetDetection onsetDetection;

        private const int SampleSize = 1024;

        public float[] Onsets => onsetDetection.Onsets;

        public float TimePerSample => onsetDetection.TimePerSample();

        public AudioAnalyzer(ISampleSource source)
        {
            this.source = source;
            this.onsetDetection = new OnsetDetection(this.source, SampleSize);
        }

        public void DetectOnsets(float sensitivity = 1.5f)
        {
            var data = new float[2048];

            while (true)
            {
                var read = source.Read(data, 0, data.Length);
                if (read <= 0)
                    break;

                onsetDetection.AddFlux(data);
            }
            onsetDetection.FindOnsets(sensitivity);

            source.Position = 0;
        }

        public void Normalize(int type)
        {
            onsetDetection.NormalizeOnsets(type);
        }
    }
}
