﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Renderer
{
    public interface IRenderer
    {
        void Clear();

        Texture LoadTexture(Bitmap bitmap);

        void DrawImage(Texture texture, int x, int y, int width, int height);

        void DrawString(string text, int x, int y, int size);

        void DrawCircle(int x, int y, int size, Color color);

        void DrawRect(Rectangle rectangle, Color color);

        Bitmap Snapshot();
    }
}
