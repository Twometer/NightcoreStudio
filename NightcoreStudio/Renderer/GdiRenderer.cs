using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightcoreStudio.Renderer
{
    public class GdiRenderer : IRenderer, IDisposable
    {
        private Dictionary<int, Bitmap> textures = new Dictionary<int, Bitmap>();

        private int textureCounter = 0;

        private Bitmap bitmap;

        private Graphics graphics;

        public GdiRenderer()
        {
            bitmap = new Bitmap(1920, 1080);
            graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }

        public void Clear()
        {
            graphics.Clear(Color.Transparent);
        }

        public void DrawCircle(int x, int y, int size, Color color)
        {
            graphics.FillEllipse(new SolidBrush(color), new Rectangle(x, y, size, size));
        }

        public void DrawImage(Texture texture, int x, int y, int width, int height)
        {
            graphics.DrawImage(textures[texture.Id], new Rectangle(x, y, width, height));
        }

        public void DrawRect(Rectangle rectangle, Color color)
        {
            graphics.FillRectangle(new SolidBrush(color), rectangle);
        }

        public void DrawString(string text, int x, int y, int size)
        {
            var font = new Font(new FontFamily("Segoe UI"), size);
            graphics.DrawString(text, font, Brushes.White, x, y);
        }

        public Texture LoadTexture(Bitmap bitmap)
        {
            var id = textureCounter++;
            textures[id] = bitmap;
            return new Texture(id);
        }

        public Bitmap Snapshot()
        {
            return bitmap;
        }

        public void Dispose()
        {
            bitmap.Dispose();
            graphics.Dispose();
        }
    }
}
