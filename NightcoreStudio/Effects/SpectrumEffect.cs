using CSCore;
using CSCore.DSP;
using NightcoreStudio.Effects.Base;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Effects
{
    public class SpectrumEffect : IEffect
    {
        public EffectStage Stage => EffectStage.Main;

        public void Apply(IRenderer renderer, Frame frame)
        {
            
        }

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            var fftSize = FftSize.Fft4096;
            var fftBuffer = new float[(int)fftSize];

        }
    }
}
