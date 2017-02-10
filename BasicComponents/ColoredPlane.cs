/*
ColoredPlane.cs
-------------

By Matthew Godin

Role : Component displaying
       a plane made of colored triangle
       stripes to the screen

Created : 21 November 2016
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class ColoredPlane : Plane
   {
        const int NO_VERTEX_OFFSET = 0, BEFORE_FIRST_VERTEX = -1;

        VertexPositionColors[] Vertices { get; set; }
        Color Color { get; set; }

        /// <summary>
        /// Constructor of the class ColoredPlane
        /// </summary>
        /// <param name="game">Contains the class GameProject</param>
        /// <param name="initialScale">Initial scale</param>
        /// <param name="initialRotation">Initial yaw, pitch, roll</param>
        /// <param name="initialPosition">Initial position</param>
        /// <param name="span">Width and height of the plane</param>
        /// <param name="dimensions">Number of rectangles in x and y</param>
        /// <param name="updateInterval">Update interval</param>
        public ColoredPlane(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, Vector2 dimensions, Color color, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, span, dimensions, updateInterval)
        {
            Color = color;
        }

        /// <summary>
        /// Creates Vertices array
        /// </summary>
        protected override void CreateVerticesArray()
        {
            Vertices = new VertexPositionColors[NumVertices];
        }

        /// <summary>
        /// Initializes the parameters for the shader
        /// </summary>
        protected override void InitializeBscEffectParameters()
        {
            BscEffect.VertexColorEnabled = true;
        }

        /// <summary>
        /// Populates Vertices
        /// </summary>
        protected override void InitializeVertices()
        {
            int vertexIndex = -1;
            for (int i = 0 ; i < NumRows ; ++i)
            {
                for (int j = 0 ; j < NumColumns + ADDITIONAL_VERTEX_FOR_LINE ; ++j)
                {
                    Vertices[++vertexIndex] = new VertexPositionColors(VertexPts[j, i], Color);
                    Vertices[++vertexIndex] = new VertexPositionColors(VertexPts[j, i + ADDITIONAL_VERTEX_FOR_LINE], Color);
                }
            }
        }

        /// <summary>
        /// Draws a triangle stripe to the screen
        /// </summary>
        /// <param name="stripIndex">Triangle stripe index to display</param>
        protected override void DrawTriangleStrip(int stripIndex)
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColors>(PrimitiveType.TriangleStrip, Vertices, stripIndex * NumVertices / NumRows, NumTrianglesPerStrip);
        }
    }
}
