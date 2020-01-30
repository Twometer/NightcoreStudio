using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Effects
{
    public enum EffectStage
    {
        /**
         * For effects that apply to the background, such as particle effects or animations
         * on the background
         */
        PreProcess,

        /**
         * For effects that apply to the video itself, such as overlaying text
         * onto the screen
         */
        Main,

        /**
         *  For effects that apply to all previous effects and the video itself,
         *  such as something that wiggles the entire screen around
         */
        PostProcess

    }
}
