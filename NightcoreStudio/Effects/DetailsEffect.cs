using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoNightcore.Generator;
using AutoNightcore.Renderer;
using CSCore;

namespace AutoNightcore.Effects
{
    public class DetailsEffect : IEffect
    {
        public EffectStage Stage => EffectStage.Main;

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
