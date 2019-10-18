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

        [Argument(1, Description = "Wallpaper file or URL")]
        public string WallpaperFile { get; }

        [Argument(2, Description = "MP4 output file")]
        public string OutputFile { get; }

        [Option(Description = "Lyrics file or URL", ShortName = "l")]
        public string LyricsFile { get; }

        [Option(Description = "Lyrics font family", ShortName = "f")]
        public string FontFamily { get; }

        [Option(Description = "Generate intro", ShortName = "i")]
        public bool GenerateIntro { get; }

        [Option(Description = "Frames per second", ShortName ="r")]
        public int FPS { get; }

        private void OnExecute()
        {

        }
    }
}
