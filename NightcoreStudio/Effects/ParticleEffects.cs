using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using CSCore;

namespace NightcoreStudio.Effects
{
    public class ParticleEffects : IEffect
    {
        public EffectStage Stage => EffectStage.PreProcess;

        public void Apply(IRenderer renderer, Frame frame)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ISampleSource soundSource)
        {
            throw new NotImplementedException();
        }
    }
}
