using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class ColoredTile : Tile
   {
      const int NUM_TRIANGLES = 2;
      VertexPositionColor[] Vertices { get; set; }
      Color Color { get; set; }

      public ColoredTile(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, 
                          Vector2 span, Color color, float updateInterval)
         : base(game, initialScale, initialRotation, initialPosition, span/*, updateInterval*/)
      {
         Color = color;
      }

      protected override void CreateVertexArray()
      {
         Vertices = new VertexPositionColor[NumVertices];
      }

      protected override void InitializeBscEffectParameters()
      {
         BscEffect.VertexColorEnabled = true;
      }

      protected override void InitializeVertices() // Is called by base.Initialize()
      {
         int vertexIndex = -1;
         for (int j = 0; j < 1; ++j)
         {
            for (int i = 0; i < 2; ++i)
            {
               Vertices[++vertexIndex] = new VertexPositionColor(VertexPts[i, j], Color);
               Vertices[++vertexIndex] = new VertexPositionColor(VertexPts[i, j + 1], Color);
            }
         }
      }

      protected override void DrawTriangleStrip()
      {
         GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
      }
   }
}

