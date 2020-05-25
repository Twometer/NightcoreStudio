using CSCore;

namespace NightcoreStudio.Generator
{
    public class FrameMath
    {
        public static int CalculateTotalFrames(ISampleSource audio, GeneratorOptions options)
        {
            return (int)(audio.GetLength().TotalSeconds * options.Fps * (1 / options.Factor));
        }

    }
}
