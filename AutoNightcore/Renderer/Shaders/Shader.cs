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
            var vertPath = $"Resources/Shaders/{name}.v.glsl";
            var fragPath = $"Resources/Shaders/{name}.f.glsl";

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

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        protected abstract void BindUniforms(int program);

    }
}
