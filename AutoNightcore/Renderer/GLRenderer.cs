using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDPixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using System.Drawing.Imaging;
using AutoNightcore.Renderer.Shaders;

namespace AutoNightcore.Renderer
{
    public class GLRenderer : IDisposable, IRenderer
    {
        private GameWindow window;

        public BasicShader BasicShader { get; private set; }

        public void Create()
        {
            var mode = new GraphicsMode(new ColorFormat(32), 24, 0, 0, ColorFormat.Empty, 1);
            var win = new GameWindow(1920, 1080, mode, "", GameWindowFlags.FixedWindow, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Default);
            win.Visible = false;
            win.WindowBorder = WindowBorder.Hidden;
            win.MakeCurrent();
            window = win;

            BasicShader = new BasicShader();
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public Texture LoadTexture(Bitmap bitmap)
        {
            var tex = new Texture(GL.GenTexture());
            tex.Bind();

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, SDPixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return tex;
        }

        public void DrawImage(Texture image, int x, int y, int width, int height)
        {
            BasicShader.Bind();
            image.Bind();

        }


        public void DrawString(string text, int x, int y, int size)
        {

        }

        public void DrawRect(Rectangle rectangle, Color color)
        {

        }

        public Bitmap Snapshot()
        {
            GL.Flush();
            var bmp = new Bitmap(640, 480, SDPixelFormat.Format32bppArgb);
            var mem = bmp.LockBits(new Rectangle(0, 0, 640, 480), ImageLockMode.WriteOnly, SDPixelFormat.Format32bppArgb);
            GL.PixelStore(PixelStoreParameter.PackRowLength, mem.Stride / 4);
            GL.ReadPixels(0, 0, 640, 480, PixelFormat.Bgra, PixelType.UnsignedByte, mem.Scan0);
            bmp.UnlockBits(mem);
            return bmp;
        }

        public void Dispose()
        {
            if (window != null)
            {
                window.Close();
                window.Dispose();
            }
        }

    }
}
