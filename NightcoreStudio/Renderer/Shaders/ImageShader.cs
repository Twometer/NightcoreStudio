using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNightcore.Renderer.Shaders
{
    public class ImageShader : Shader
    {
        private int modelMatrixLoc;
       
        private int projectionMatrixLoc;

        public Matrix4 ModelMatrix { set => GL.UniformMatrix4(modelMatrixLoc, false, ref value);  }

        public Matrix4 ProjectionMatrix { set => GL.UniformMatrix4(projectionMatrixLoc, false, ref value); }

        public ImageShader() : base("image")
        {
        }

        protected override void BindUniforms(int program)
        {
            modelMatrixLoc = GL.GetUniformLocation(program, "modelMatrix");
            projectionMatrixLoc = GL.GetUniformLocation(program, "projectionMatrix");
        }

    }
}
