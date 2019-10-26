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
            CheckError("Vertex", vertId);

            GL.ShaderSource(fragId, frag);
            GL.CompileShader(fragId);
            CheckError("Fragment",  fragId);

            var program = GL.CreateProgram();
            GL.AttachShader(program, vertId);
            GL.AttachShader(program, fragId);
            GL.LinkProgram(program);
            GL.DetachShader(program, vertId);
            GL.DetachShader(program, fragId);
            GL.DeleteShader(vertId);
            GL.DeleteShader(fragId);

            return program;
        }

        private static void CheckError(string shader, int shaderId)
        {
            var log = GL.GetShaderInfoLog(shaderId);
            if (log.Length > 0)
                Console.WriteLine($"ERROR IN SHADER {shader}:\n{log}");
        }

    }
}
