using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using CSCore;
using System.Drawing;

namespace NightcoreStudio.Effects
{
    public class FadeEffect : IEffect
    {
        public EffectStage Stage => EffectStage.PostProcess;

        private float duration;
        private float totalLength;

        public FadeEffect(float durationSecs)
        {
            duration = durationSecs;
        }

        public void Apply(IRenderer renderer, Frame frame)
        {
            float brightness = 1.0f;
            if (frame.Number < duration)
            {
                brightness = (frame.Number / duration);
            }
            else if (frame.Number > totalLength - duration)
            {
                brightness = (totalLength - frame.Number) / duration;
            }
            renderer.DrawRect(new Rectangle(0, 0, 1920, 1080), Color.FromArgb((int)((1 - brightness) * 255), 0, 0, 0));
        }

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            totalLength = FrameMath.CalculateTotalFrames(soundSource, options);
            duration *= options.Fps;
        }
    }
}
