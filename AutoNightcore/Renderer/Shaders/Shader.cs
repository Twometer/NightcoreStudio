using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Renderer.Shaders
{
    public abstract class Shader
    {
        private int programId;

        protected Shader(string name)
        {
            var vertPath = $"shaders/{name}.vs";
            var fragPath = $"shaders/{name}.fs";

            var vert = File.ReadAllText(vertPath);
            var frag = File.ReadAllText(fragPath);
            programId = Loader.LoadShader(vert, frag);
            Initialize();
        }

        private void Initialize()
        {
            Bind();
            BindUniforms(programId);
        }

        public void Bind()
        {
            GL.UseProgram(programId);
        }

        protected abstract void BindUniforms(int program);

    }
}
