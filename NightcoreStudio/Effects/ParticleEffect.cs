using System;
using NightcoreStudio.Generator;
using NightcoreStudio.Renderer;
using NightcoreStudio.Effects.Base;
using CSCore;
using OpenTK;
using System.Collections.Generic;

namespace NightcoreStudio.Effects
{
    public class ParticleEffect : IEffect
    {
        public EffectStage Stage => EffectStage.PreProcess;

        private Random random = new Random();
        private List<Particle> particles = new List<Particle>();

        private int particleCount;

        public ParticleEffect(int particleCount)
        {
            this.particleCount = particleCount;
        }

        public void Apply(IRenderer renderer, Frame frame)
        {
            foreach (var particle in particles)
            {
                var halfSize = particle.size / 2;
                renderer.DrawCircle((int)(particle.position.X - halfSize), (int)(particle.position.Y - halfSize), (int)(particle.size), System.Drawing.Color.FromArgb(particle.opacity, 255, 255, 255));
                particle.position.Y += (int)particle.speed;
                if (particle.position.Y > 1080)
                {
                    RandomizeParticle(particle);
                    particle.position.Y = (float)-particle.size * 2f;
                }
            }
        }

        public void Initialize(ISampleSource soundSource, GeneratorOptions options)
        {
            for (int i = 0; i < particleCount; i++)
            {
                var p = new Particle();
                RandomizeParticle(p);
                particles.Add(p);
            }
        }

        private void RandomizeParticle(Particle particle)
        {
            particle.position = new Vector2(random.Next(10, 1910), random.Next(10, 1070));
            particle.opacity = random.Next(15, 100);
            particle.size = random.NextDouble() * 16;
            particle.speed = Math.Max(1, random.NextDouble() * 4);
        }

        private class Particle
        {
            public Vector2 position;
            public int opacity;
            public double size;
            public double speed;
        }
    }
}
