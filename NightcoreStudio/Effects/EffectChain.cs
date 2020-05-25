using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Effects
{
    public class EffectChain
    {
        private List<IEffect> effects = new List<IEffect>();

        public void Add(IEffect effect)
        {
            effects.Add(effect);
        }

        public void Initialize(ISampleSource source, GeneratorOptions options)
        {
            foreach (var eff in effects)
                eff.Initialize(source, options);
        }

        public void Apply(IRenderer renderer, Frame frame, EffectStage stage)
        {
            foreach (var eff in effects)
                if (eff.Stage == stage)
                    eff.Apply(renderer, frame);
        }

    }
}
