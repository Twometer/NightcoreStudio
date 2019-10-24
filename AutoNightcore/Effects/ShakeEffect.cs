using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoNightcore.Generator;
using AutoNightcore.Renderer;
using CSCore;
using OnsetDetection;

namespace AutoNightcore.Effects
{
    public class ShakeEffect : IEffect, IProgress<string>
    {
        private List<Onset> onsets;

        public EffectStage Stage => EffectStage.PostProcess;

        public void Initialize(ISampleSource soundSource)
        {
            var detector = new OnsetDetector(DetectorOptions.Default, this);
            onsets = detector.Detect(soundSource);
        }

        public void Apply(IRenderer renderer, Frame frame)
        {
            throw new NotImplementedException();
        }

        public void Report(string value)
        {
            
        }
    }
}
