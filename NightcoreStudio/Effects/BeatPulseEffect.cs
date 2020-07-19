using System;
using System.Linq;
using System.Drawing;
using CSCore;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using NightcoreStudio.Audio.OnsetDetection;
using NightcoreStudio.Effects.Base;

namespace NightcoreStudio.Effects
{
    public class BeatPulseEffect : IEffect
    {
        private AudioAnalysis analysis;

        public EffectStage Stage => EffectStage.PostProcess;

        private double intensity;
        private int maxFrames;

        private int fps;

        public BeatPulseEffect(double intensity, int maxFrames = 3)
        {
            this.intensity = intensity;
            this.maxFrames = maxFrames;
        }

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
            var maxTimespan = (1.0d / fps) * maxFrames;
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
                        renderer.DrawRect(new Rectangle(0, 0, 1920, 1080), Color.FromArgb((int)(255.0f * intensity * timeProgress * onsetMultiplier), 255, 255, 255));
                        return;
                    }
                }
                num++;
            }
        }
    }
}
