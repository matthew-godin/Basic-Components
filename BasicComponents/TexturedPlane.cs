using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class TexturedPlane : Plane
   {
        //Initially managed by the constructor
        readonly string TextureName;

        //Initially managed by LoadContent()
        ResourceManager<Texture2D> TextureMgr { get; set; }
        Texture2D Texture { get; set; }

        //Initially managed by functions called by base.Initialize()
        Vector2[,] TexturePts { get; set; }
        protected VertexPositionTexture[] Vertices { get; set; }
        BlendState AlphaMgr { get; set; }

        public TexturedPlane(Game game, float initialScale, Vector3 initialRotation,
                          Vector3 initialPosition, Vector2 span, Vector2 dimensions,
                          string textureName, float updateInterval)
            :base(game, initialScale, initialRotation, initialPosition, span, dimensions, updateInterval)
        {
            TextureName = textureName;
        }

        protected override void CreateVertexArray()
        {
            TexturePts = new Vector2[NumColumns + 1, NumRows + 1];
            Vertices = new VertexPositionTexture[NumVertices];
            CreateTexturePointArray();
        }

        private void CreateTexturePointArray()
        {
            for (int i = 0; i < TexturePts.GetLength(0); ++i)
            {
                for(int j = 0; j < TexturePts.GetLength(1); ++j)
                {
                    TexturePts[i, j] = new Vector2(i / (float)NumColumns, -j / (float)NumRows);
                }
            }
        }

        protected override void InitializeBscEffectParameters()
        {
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = Texture;
            AlphaMgr = BlendState.AlphaBlend;
        }

        protected override void LoadContent()
        {
            TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
            Texture = TextureMgr.Find(TextureName);
            base.LoadContent();
        }

        protected override void InitializeVertices()
        {
            int vertexIndex = -1;
            for (int j = 0; j < NumRows; ++j)
            {
                for (int i = 0; i < NumColumns + 1; ++i)
                {
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[i, j], TexturePts[i, j]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);
                }
            }
        }

        protected override void DrawTriangleStrip(int stripIndex)
        {
            int vertexOffset = (stripIndex * NumVertices) / NumRows;
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, vertexOffset, NumTrianglesPerStrip);
        }

    }
}
