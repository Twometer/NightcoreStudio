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

        private TimeSpan totalTime;

        public void Apply(IRenderer renderer, Frame frame)
        {
            renderer.DrawString("Hello!", 15, 15, 1);

            var progress = frame.Time.TotalSeconds / totalTime.TotalSeconds;
            renderer.DrawRect(new Rectangle(0, 1080 - 10, (int)(1920 * progress), 10), Color.White);
        }

        public void Initialize(ISampleSource soundSource)
        {
            // TODO this results in the non-sped up time
            totalTime = soundSource.GetLength();
        }
    }
}
