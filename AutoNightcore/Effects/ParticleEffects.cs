﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoNightcore.Generator;
using CSCore;

namespace AutoNightcore.Effects
{
    public class ParticleEffects : IEffect
    {
        public EffectStage Stage => EffectStage.PreProcess;

        public void Apply(NightcoreVideo video, int frame, TimeSpan time)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ISampleSource soundSource)
        {
            throw new NotImplementedException();
        }
    }
}