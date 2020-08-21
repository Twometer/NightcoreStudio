using System.Drawing;
using CSCore;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using NightcoreStudio.Effects.Base;

namespace NightcoreStudio.Effects
{
    public class DetailsEffect : IEffect
    {
        public EffectStage Stage => EffectStage.Main;

        private int totalFrames;

        public void Apply(IRenderer renderer, Frame frame)
        {
            renderer.DrawString("Castle", 15, 15, 42);
            renderer.DrawString("Halsey", 15, 70, 32);

            var progress = frame.Number / (double)totalFrames;
            renderer.DrawRect(new Rectangle(0, 1080 - 10, (int)(1920 * progress), 10), Color.White);
        }

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            totalFrames = FrameMath.CalculateTotalFrames(soundSource, options);
        }
    }
}

