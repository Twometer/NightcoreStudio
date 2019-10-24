using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace AutoNightcore.Renderer
{
    public class Texture
    {
        private int id;

        public Texture(int id)
        {
            this.id = id;
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

    }
}
