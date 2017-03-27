using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class TexturedTile : Tile
   {
      const int NUM_TRIANGLES = 2;
      ResourceManager<Texture2D> TextureMgr;
      Texture2D textureTile;
      GreenxPositionTexture[] Vertices { get; set; }
      protected Vector2[,] TexturePts { get; set; }
      string TextureNameTile { get; set; }
      BlendState AlphaMgr { get; set; }

      public TexturedTile(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, string textureNameTile/*, float updateInterval*/)
         : base(game, initialScale, initialRotation, initialPosition, span/*, updateInterval*/)
      {
         TextureNameTile = textureNameTile;
      }

      protected override void CreateVertexArray()
      {
         TexturePts = new Vector2[2, 2];
         Vertices = new GreenxPositionTexture[NumVertices];
         CreateTexturePointArray();
      }

      private void CreateTexturePointArray()
      {
         TexturePts[0, 0] = new Vector2(0, 1);
         TexturePts[1, 0] = new Vector2(1, 1);
         TexturePts[0, 1] = new Vector2(0, 0);
         TexturePts[1, 1] = new Vector2(1, 0);
      }

      protected override void InitializeVertices() // Is called by base.Initialize()
      {
         int vertexIndex = -1;
         for (int j = 0; j < 1; ++j)
         {
            for (int i = 0; i < 2; ++i)
            {
               Vertices[++vertexIndex] = new GreenxPositionTexture(VertexPts[i, j], TexturePts[i, j]);
               Vertices[++vertexIndex] = new GreenxPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);
            }
         }
      }

      protected override void LoadContent()
      {

         TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
         textureTile = TextureMgr.Find(TextureNameTile);
         base.LoadContent();
      }

      protected override void InitializeBscEffectParameters()
      {
         BscEffect.TextureEnabled = true;
         BscEffect.Texture = textureTile;
         AlphaMgr = BlendState.AlphaBlend; // Attention à ceci...
      }

      public override void Draw(GameTime gameTime)
      {
         BlendState oldBlendState = GraphicsDevice.BlendState; // ... and à cela!
         GraphicsDevice.BlendState = AlphaMgr;
         base.Draw(gameTime);
         GraphicsDevice.BlendState = oldBlendState;
      }

      protected override void DrawTriangleStrip()
      {
         GraphicsDevice.DrawUserPrimitives<GreenxPositionTexture>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
      }
   }
}
