using System;
using Microsoft.Xna.Framework;


namespace XNAProject
{
    public class TexturedCubeFlottant : TexturedCube
    {
        float TimeElpasedSinceUpdate { get; set; }
        float IntervalleVariation { get; set; }
        float Variation { get; set; }

        public TexturedCubeFlottant(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, string textureNameCube,
            Vector3 dimension, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, textureNameCube, dimension, updateInterval)
        {
            IntervalleVariation = updateInterval;
        }
        public override void Initialize()
        {
            Yaw = true;
            Variation = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElpasedSinceUpdate += timeElapsed;
            if (TimeElpasedSinceUpdate >= IntervalleVariation)
            {
                VariationCube();
                TimeElpasedSinceUpdate = 0;
            }
        }
        private void VariationCube()
        {
            Position = new Vector3(Position.X, InitialPosition.Y + (float)Math.Sin(Variation), Position.Z);
            Variation += 0.02f;
        }
    }
}
