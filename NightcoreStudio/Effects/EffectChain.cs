using AutoNightcore.Generator;
using AutoNightcore.Renderer;
using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Effects
{
    public class EffectChain
    {
        private List<IEffect> effects = new List<IEffect>();

        public void Add(IEffect effect)
        {
            effects.Add(effect);
        }

        public void Initialize(ISampleSource source)
        {
            foreach (var eff in effects)
                eff.Initialize(source);
        }

        public void Apply(IRenderer renderer, Frame frame, EffectStage stage)
        {
            foreach (var eff in effects)
                if (eff.Stage == stage)
                    eff.Apply(renderer, frame);
        }

    }
}
