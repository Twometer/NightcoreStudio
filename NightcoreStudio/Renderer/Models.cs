using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Renderer
{
    public static class Models
    {
        public static Model Rectangle { get; private set; }

        public static Model Circle { get; private set; }

        public static void BuildModels()
        {
            BuildRectangle();
            BuildCircle();
        }

        private static void BuildCircle()
        {
            const int halfPrec = 40;

            var vertices = new List<float>();
            var textures = new List<float>();
            

            var angInc = Math.PI / (float)halfPrec;
            var cosInc = Math.Cos(angInc);
            var sinInc = Math.Sin(angInc);

            vertices.Add(1.0f);
            vertices.Add(0.0f);

            double xc = 1.0f;
            double yc = 0.0f;
            for (var iAng = 1; iAng < halfPrec; iAng++)
            {
                var xcNew = cosInc * xc - sinInc * yc;
                yc = sinInc * xc + cosInc * yc;
                xc = xcNew;

                vertices.Add((float)xc);
                vertices.Add((float)yc);

                vertices.Add((float)xc);
                vertices.Add((float)-yc);
            }

            vertices.Add(-1.0f);
            vertices.Add(0.0f);
            Circle = new Model(vertices.ToArray(), textures.ToArray());
        }

        private static void BuildRectangle()
        {
            var vertices = new float[]
            {
                0, 1,
                0, 0,
                1, 1,
                1, 0
            };
            var textures = new float[]
            {
                0, 0,
                0, 1,
                1, 0,
                1, 1
            };
            Rectangle = new Model(vertices, textures);
        }
    }
}
