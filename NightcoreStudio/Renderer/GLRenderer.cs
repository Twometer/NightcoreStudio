﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

using SDPixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using System.Drawing.Imaging;
using NightcoreStudio.Renderer.Shaders;

namespace NightcoreStudio.Renderer
{
    public class GLRenderer : IDisposable, IRenderer
    {
        private GameWindow window;

        public ImageShader ImageShader { get; private set; }
        public FlatShader FlatShader { get; private set; }

        public Matrix4 ProjectionMatrix = Matrix4.Identity;

        private Bitmap frame = new Bitmap(1920, 1080, SDPixelFormat.Format32bppRgb);

        public GLRenderer(bool rendererVisible)
        {
            var mode = new GraphicsMode(new ColorFormat(32), 24, 0, 0, ColorFormat.Empty, 1);
            var win = new GameWindow(1920, 1080, mode, "NightcoreStudio Renderer", GameWindowFlags.FixedWindow, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Default);
            win.Visible = rendererVisible;
            if (!rendererVisible)
                win.WindowBorder = WindowBorder.Hidden;
            win.MakeCurrent();
            window = win;

            ImageShader = new ImageShader();
            FlatShader = new FlatShader();

            GL.Enable(EnableCap.Texture2D);
            GL.ClearColor(Color.Purple);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0f, 1920f, 1080f, 0f, 0.0f, 1.0f);

            Models.BuildModels();
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
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
            ImageShader.Bind();
            image.Bind();

            ImageShader.ProjectionMatrix = ProjectionMatrix;
            ImageShader.ModelMatrix = Matrix4.CreateScale(width, height, 1.0f) * Matrix4.CreateTranslation(x, y, 0);

            Models.Rectangle.Draw();

            image.Unbind();
            ImageShader.Unbind();
        }


        public void DrawString(string text, int x, int y, int size)
        {

        }

        public void DrawCircle(int x, int y, int size, Color color)
        {
            FlatShader.Bind();
            FlatShader.ProjectionMatrix = ProjectionMatrix;
            FlatShader.ModelMatrix = Matrix4.CreateScale(size, size, 1.0f) * Matrix4.CreateTranslation(x, y, 0);
            FlatShader.Color = new Color4(color.R, color.G, color.B, color.A);

            Models.Circle.Draw();
            FlatShader.Unbind();
        }

        public void DrawRect(Rectangle rectangle, Color color)
        {
            FlatShader.Bind();
            FlatShader.ProjectionMatrix = ProjectionMatrix;
            FlatShader.ModelMatrix = Matrix4.CreateScale(rectangle.Width, rectangle.Height, 1.0f) * Matrix4.CreateTranslation(rectangle.X, rectangle.Y, 0);
            FlatShader.Color = new Color4(color.R, color.G, color.B, color.A);

            Models.Rectangle.Draw();
            FlatShader.Unbind();
        }

        public Bitmap Snapshot()
        {
            GL.Flush();

            var mem = frame.LockBits(new Rectangle(0, 0, 1920, 1080), ImageLockMode.WriteOnly, SDPixelFormat.Format32bppRgb);
            GL.PixelStore(PixelStoreParameter.PackRowLength, mem.Stride / 4);
            GL.ReadPixels(0, 0, 1920, 1080, PixelFormat.Bgra, PixelType.UnsignedByte, mem.Scan0);
            frame.UnlockBits(mem);
            return frame;
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
