using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
    class Flag : TexturedPlane
    {
        //Initially managed by the constructor
        readonly float MaxVariation;
        readonly float VariationInterval;

        //Initially managed by Initialize()
        float TimeElapsedSinceUpdate { get; set; }
        float TimeElapsed { get; set; }
        float TotalTime { get; set; }
        RasterizerState GameRasterizerState { get; set; }

        public Flag(Game game, float initialScale, Vector3 initialRotation,
                       Vector3 initialPosition, Vector2 span, Vector2 dimensions,
                       string textureName, float maxVariation, float intervalleVariation,
                       float updateInterval)
            :base(game, initialScale, initialRotation, initialPosition, span, 
                  dimensions, textureName, updateInterval)
        {
            MaxVariation = maxVariation;
            VariationInterval = intervalleVariation;
        }

        public override void Initialize()
        {
            TimeElapsedSinceUpdate = 0;
            TimeElapsed = 0;
            TotalTime = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            TimeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += TimeElapsed;
            if (TimeElapsedSinceUpdate >= VariationInterval)
            {
                CreatePointArray();
                InitializeVertices();
                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        private void CreatePointArray()
        {
            for (int i = 0; i < VertexPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VertexPts.GetLength(1); ++j)
                {
                    VertexPts[i, j] = new Vector3(VertexPts[i, j].X, VertexPts[i, j].Y, GetVariation(i,j));
                }
            }
        }

        float GetVariation(int i, int j)
        {
            TotalTime += TimeElapsed;
            TotalTime = TotalTime > 10*Math.PI ? 0 : TotalTime;
            return (MaxVariation *(float)Math.Sin(VertexPts[i, j].X + 2*TotalTime));
        }

        public override void Draw(GameTime gameTime)
        {
            RasterizerState oldRasterizerState = GraphicsDevice.RasterizerState;
            RasterizerState gameRasterizerState = new RasterizerState();
            gameRasterizerState.CullMode = CullMode.None;

            gameRasterizerState.FillMode = oldRasterizerState.FillMode;
            GraphicsDevice.RasterizerState = gameRasterizerState;
            base.Draw(gameTime);
            GraphicsDevice.RasterizerState = oldRasterizerState;
        }
    }
}
