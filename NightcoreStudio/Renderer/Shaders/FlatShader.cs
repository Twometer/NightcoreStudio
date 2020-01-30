using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Renderer.Shaders
{
    public class FlatShader : Shader
    {
        private int modelMatrixLoc;
       
        private int projectionMatrixLoc;

        private int colorLoc;

        public Matrix4 ModelMatrix { set => GL.UniformMatrix4(modelMatrixLoc, false, ref value);  }

        public Matrix4 ProjectionMatrix { set => GL.UniformMatrix4(projectionMatrixLoc, false, ref value); }

        public Color4 Color { set => GL.Uniform4(colorLoc, value); }

        public FlatShader() : base("flat")
        {
        }

        protected override void BindUniforms(int program)
        {
            modelMatrixLoc = GL.GetUniformLocation(program, "modelMatrix");
            projectionMatrixLoc = GL.GetUniformLocation(program, "projectionMatrix");
            colorLoc = GL.GetUniformLocation(program, "color");
        }

    }
}
