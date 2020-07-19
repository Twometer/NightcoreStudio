using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Generator
{
    public class GeneratorOptions
    {
        public FileInfo AudioFile { get; }

        public FileInfo WallpaperFile { get; }

        public FileInfo OutputFile { get; }

        public FileInfo LyricsFile { get; }

        public string FontFamily { get; }

        public bool GenerateIntro { get; }

        public bool GenerateLyrics => LyricsFile != null;

        public int Fps { get; }

        public float Factor { get; }

        public bool RendererVisible { get; }

        public GeneratorOptions(FileInfo audioFile, FileInfo wallpaperFile, FileInfo outputFile, FileInfo lyricsFile, string fontFamily, bool generateIntro, int fps, float factor, bool rendererVisible)
        {
            AudioFile = audioFile;
            WallpaperFile = wallpaperFile;
            OutputFile = outputFile;
            LyricsFile = lyricsFile;
            FontFamily = fontFamily;
            GenerateIntro = generateIntro;
            Fps = fps;
            Factor = factor;
            RendererVisible = rendererVisible;
        }
    }
}
