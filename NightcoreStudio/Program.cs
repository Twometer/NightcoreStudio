using NightcoreStudio.Effects;
using NightcoreStudio.Generator;
using CSVideo;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio
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

        [Option(Description = "Speed increase (percent)", ShortName = "s")]
        public int SpeedIncrease { get; } = 25;

        [Option(Description = "Show renderer during export", ShortName = "V")]
        public bool RendererVisible { get; } = false;

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

            Console.WriteLine("Loading FFmpeg...");
            var ffmpegOk = FFmpegLoader.Load("Libraries");
            if (!ffmpegOk)
            {
                Console.WriteLine("FFmpeg libraries not found.");
                return;
            }

            Console.WriteLine($"Loaded ffmpeg v{FFmpegLoader.FFmpegVersion}");

            Console.WriteLine("Generating Nightcore video...");
            var options = new GeneratorOptions(new FileInfo(AudioFile), new FileInfo(WallpaperFile), new FileInfo(OutputFile), LyricsFile != null ? new FileInfo(LyricsFile) : null, FontFamily, GenerateIntro, FPS, 1.0f + SpeedIncrease / 100.0f, RendererVisible);
            var generator = new NightcoreGenerator(options);

            generator.AddEffect(new BeatPulseEffect(0.1, 4));
            generator.AddEffect(new DetailsEffect());
            generator.AddEffect(new FadeEffect(3));
            generator.AddEffect(new ParticleEffect(20));

            generator.ProgressHandler = progress =>
            {
                Console.WriteLine($"Progress: {progress}%");
            };
            

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
