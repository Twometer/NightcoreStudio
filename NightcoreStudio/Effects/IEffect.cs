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
    public interface IEffect
    {
        EffectStage Stage { get; }

        void Initialize(ISampleSource soundSource, GeneratorOptions options);

        void Apply(IRenderer renderer, Frame frame);
    }
}
 