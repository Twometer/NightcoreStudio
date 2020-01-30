using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using CSCore;
using NightcoreStudio.Audio.OnsetDetection;

namespace NightcoreStudio.Effects
{
    public class ShakeEffect : IEffect, IProgress<string>
    {
        private AudioAnalyzer audioAnalyzer;

        public EffectStage Stage => EffectStage.PostProcess;

        public void Initialize(ISampleSource soundSource)
        {
            audioAnalyzer = new AudioAnalyzer(soundSource);
            audioAnalyzer.DetectOnsets();
            audioAnalyzer.Normalize(0);
        }

        public void Apply(IRenderer renderer, Frame frame)
        {
            var frameTime = frame.Time.TotalSeconds;
            foreach (var onset in audioAnalyzer.Onsets)
            {
                if (Math.Abs(frameTime - (onset * audioAnalyzer.TimePerSample)) < 0.01)
                {
                    renderer.DrawRect(new System.Drawing.Rectangle(0, 0, 1920, 1080), System.Drawing.Color.White);
                }
            }
        }

        public void Report(string value)
        {

        }
    }
}
