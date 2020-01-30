using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Renderer
{
    public class Model
    {
        private int id;
        private int count;

        public Model(float[] coordinates, float[] texCoords)
        {
            id = GL.GenVertexArray();
            GL.BindVertexArray(id);

            var vbo_vert = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_vert);
            GL.BufferData(BufferTarget.ArrayBuffer, coordinates.Length * sizeof(float), coordinates, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            var vbo_tex = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_tex);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);

            this.count = (int)(coordinates.Length / 2.0f);
        }

        public void Draw()
        {
            GL.BindVertexArray(id);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, count);

            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

    }
}
