using AutoNightcore.Generator;
using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Effects
{
    public interface IEffect
    {
        EffectStage Stage { get; }

        void Initialize(ISampleSource soundSource);

        void Apply(NightcoreVideo video, int frame, TimeSpan time);
    }
}
 