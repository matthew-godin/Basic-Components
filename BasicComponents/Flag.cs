/*
Flag.cs
----------

By Matthew Godin

Role : Component displaying
       a waving flag made of 
       triangle stripes

Created : 21 November 2016
*/
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    class Flag : TexturedPlane
    {
        const int NO_TIME_ELAPSED = 0;

        float MaxVariation { get; set; }
        float VariationInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        float Temps { get; set; }

        /// <summary>
        /// Constructor of the class Flag
        /// </summary>
        /// <param name="game">Contains the class GameProject</param>
        /// <param name="initialScale">Initial scale</param>
        /// <param name="initialRotation">Initial yaw, pitch, roll</param>
        /// <param name="initialPosition">Initial position</param>
        /// <param name="span">Width and height of the plane</param>
        /// <param name="dimensions">Number of rectangles in x and y</param>
        /// <param name="updateInterval">Update interval</param>
        public Flag(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, Vector2 dimensions, string textureName, float maxVariation, float intervalleVariation, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, span, dimensions, textureName, updateInterval)
        {
            MaxVariation = maxVariation;
            VariationInterval = intervalleVariation;
        }

        /// <summary>
        /// Initializes the plane
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Temps = NO_TIME_ELAPSED;
        }

        /// <summary>
        /// Method updating the content and takes care of time management
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Temps += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeElapsedSinceUpdate >= VariationInterval)
            {
                TimeElapsedSinceUpdate = NO_TIME_ELAPSED;
                MettreÀJourVertices(gameTime);
                InitializeVertices();
            }
        }

        /// <summary>
        /// Updates the vertices to wave the flag
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        void MettreÀJourVertices(GameTime gameTime)
        {
            for (int i = 0 ; i < VertexPts.GetLength(0) ; ++i)
            {
                for (int j = 0 ; j < VertexPts.GetLength(1) ; ++j)
                {
                    VertexPts[i, j].Z = (float)(MaxVariation * Math.Sin(VertexPts[i, j].X - Temps));
                }
            }
        }

        /// <summary>
        /// Draws a triangle stripe to the screen
        /// </summary>
        /// <param name="stripIndex">Triangle stripe index to display</param>
        protected override void DrawTriangleStrip(int stripIndex)
        {
            RasterizerState gameRasterizerStateNormal = new RasterizerState();
            gameRasterizerStateNormal.CullMode = GraphicsDevice.RasterizerState.CullMode;
            RasterizerState gameRasterizerStateRien = new RasterizerState();
            gameRasterizerStateRien.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = gameRasterizerStateRien;
            base.DrawTriangleStrip(stripIndex);
            GraphicsDevice.RasterizerState = gameRasterizerStateNormal;
        }
    }
}
