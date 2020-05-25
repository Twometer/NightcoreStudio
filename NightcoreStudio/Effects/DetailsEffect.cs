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
    public class DetailsEffect : IEffect
    {
        public EffectStage Stage => EffectStage.Main;

        private int totalFrames;

        public void Apply(IRenderer renderer, Frame frame)
        {
            renderer.DrawString("Hello!", 15, 15, 1);

            var progress = frame.Number / (double)totalFrames;
            renderer.DrawRect(new Rectangle(0, 1080 - 10, (int)(1920 * progress), 10), Color.White);
        }

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            totalFrames = FrameMath.CalculateTotalFrames(soundSource, options);
        }
    }
}
