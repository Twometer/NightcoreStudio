using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore
{
    [HelpOption]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args.Length == 0 ? new string[] { "--help" } : args);

        [Argument(0, Description = "Audio file or URL")]
        public string AudioFile { get; }

        [Argument(1, Description = "Lyrics file or URL")]
        public string LyricsFile { get; }

        [Argument(2, Description = "Wallpaper file or URL")]
        public string WallpaperFile { get; }

        [Option(Description = "Lyrics font family", ShortName = "ff")]
        public string FontFamily { get; }

        [Option(Description = "Generate intro", ShortName = "I")]
        public bool GenerateIntro { get; }

        private void OnExecute()
        {

        }
    }
}
