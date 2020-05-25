using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using CSCore;
using NightcoreStudio.Audio.OnsetDetection;
using System.Drawing;

namespace NightcoreStudio.Effects
{
    public class BeatPulseEffect : IEffect
    {
        private AudioAnalysis analysis;

        public EffectStage Stage => EffectStage.PostProcess;

        private int fps;

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            analysis = new AudioAnalysis();
            analysis.PCMStream = soundSource;
            analysis.DetectOnsets();
            analysis.NormalizeOnsets(0);
            analysis.PCMStream.Position = 0;

            fps = options.Fps;

            Console.WriteLine("Audio analyzer detected " + analysis.GetOnsets().Count(f => f > 0) + " onsets!");
        }

        public void Apply(IRenderer renderer, Frame frame)
        {
            var frameTime = frame.Time.TotalSeconds;
            var num = 0;
            var maxTimespan = (1.0d / fps) * 3;
            foreach (var onset in analysis.GetOnsets())
            {
                if (onset > 0.04)
                {

                    float timePos = num * analysis.GetTimePerSample();
                    var timeDiff = Math.Abs(frameTime - timePos);
                    if (timeDiff <= maxTimespan)
                    {
                        var onsetMultiplier = Math.Min(1.0, onset + 0.5);
                        var timeProgress = timeDiff / maxTimespan;
                        renderer.DrawRect(new Rectangle(0, 0, 1920, 1080), Color.FromArgb((int)(50 * timeProgress * onsetMultiplier), 255, 255, 255));
                        return;
                    }
                }
                num++;
            }
        }
    }
}
