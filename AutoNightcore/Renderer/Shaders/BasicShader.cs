using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Renderer.Shaders
{
    public class BasicShader : Shader
    {
        private int modelMatrixLoc;

        private int viewMatrixLoc;
        
        private int projectionMatrixLoc;

        public Matrix4 ModelMatrix { set => GL.UniformMatrix4(modelMatrixLoc, false, ref value);  }

        public Matrix4 ViewMatrix { set => GL.UniformMatrix4(viewMatrixLoc, false, ref value); }

        public Matrix4 ProjectionMatrix { set => GL.UniformMatrix4(projectionMatrixLoc, false, ref value); }

        public BasicShader() : base("basic")
        {
        }

        protected override void BindUniforms(int program)
        {
            modelMatrixLoc = GL.GetUniformLocation(program, "modelMatrix");
            viewMatrixLoc = GL.GetUniformLocation(program, "viewMatrix");
            projectionMatrixLoc = GL.GetUniformLocation(program, "projectionMatrix");
        }

    }
}
