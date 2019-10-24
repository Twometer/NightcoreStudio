using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Effects
{
    public class EffectChain
    {
        private List<IEffect> effects;


        public void Add(IEffect effect)
        {
            effects.Add(effect);
        }

        public void Apply(EffectStage stage)
        {

        }

    }
}
