using AutoNightcore.Generator;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string LyricsFile { get; } = null;

        [Option(Description = "Lyrics font family", ShortName = "f")]
        public string FontFamily { get; } = "Arial Black";

        [Option(Description = "Generate intro", ShortName = "i")]
        public bool GenerateIntro { get; } = false;

        [Option(Description = "Frames per second", ShortName = "r")]
        public int FPS { get; } = 30;

        [Option(Description = "Speed up factor (percent)", ShortName = "s")]
        public int Factor { get; } = 25;

        private void OnExecute()
        {
            if (!File.Exists(AudioFile))
            {
                Console.WriteLine("Audio file does not exist");
                return;
            }

            if (!File.Exists(WallpaperFile))
            {
                Console.WriteLine("Wallpaper file does not exist");
                return;
            }

            if (OutputFile == null)
            {
                Console.WriteLine("Output file is required");
                return;
            }

            bool generateLyrics = LyricsFile != null;
            if (generateLyrics && !File.Exists(LyricsFile))
            {
                Console.WriteLine("Lyrics file does not exist");
                return;
            }

            if (FPS <= 0 || FPS > 120)
            {
                Console.WriteLine("FPS outside of range 1...120 are not supported");
                return;
            }

            Console.WriteLine("Generating Nightcore video...");
            var options = new GeneratorOptions(new FileInfo(AudioFile), new FileInfo(WallpaperFile), new FileInfo(OutputFile), LyricsFile != null ? new FileInfo(LyricsFile) : null, FontFamily, GenerateIntro, FPS, Factor / 100.0f);
            var generator = new NightcoreGenerator(options);

            if (generator.Generate())
            {
                Console.WriteLine("Generated video successfully");
            }
            else
            {
                Console.WriteLine("Failed to generate video. Check error log for more info");
            }
        }
    }
}
