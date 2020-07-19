using System;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using NightcoreStudio.Effects.Base;
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

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
