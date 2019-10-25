using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Renderer
{
    public class Loader
    {
        public static int LoadShader(string vert, string frag)
        {
            var vertId = GL.CreateShader(ShaderType.VertexShader);
            var fragId = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertId, vert);
            GL.CompileShader(vertId);

            GL.ShaderSource(fragId, frag);
            GL.CompileShader(fragId);

            var program = GL.CreateProgram();
            GL.AttachShader(program, vertId);
            GL.AttachShader(program, fragId);
            GL.DeleteShader(vertId);
            GL.DeleteShader(fragId);

            return program;
        }

    }
}
