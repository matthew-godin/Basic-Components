using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class TexturedSphere : AnimatedBasicPrimitive //Plane
   {
      //Initially managed by the constructor
      readonly float Radius;
      readonly int NumColumns;
      readonly int NumLines;
      readonly string TextureName;

      readonly Vector3 Origin;

      //Initially managed by functions called by base.Initialize()
      Vector3[,] VertexPts { get; set; }
      Vector2[,] TexturePts { get; set; }
      VertexPositionTexture[] Vertices { get; set; }
      BasicEffect BscEffect { get; set; }
      //BlendState AlphaMgr { get; set; }

      int NumTrianglesPerStrip { get; set; }

      //Initially managed by LoadContent()
      ResourceManager<Texture2D> TextureMgr { get; set; }
      Texture2D SphereTexture { get; set; }

      public TexturedSphere(Game game, float initialScale, Vector3 initialRotation,
                            Vector3 initialPosition, float radius, Vector2 dimensions,
                            string textureName, float updateInterval)
          : base(game, initialScale, initialRotation, initialPosition, updateInterval)
      {
         Radius = radius;
         NumColumns = (int)dimensions.X;
         NumLines = (int)dimensions.Y;
         TextureName = textureName;

         Origin = new Vector3(0, 0, 0);
      }

      public override void Initialize()
      {
         NumTrianglesPerStrip = NumColumns * 2;
         NumVertices = (NumTrianglesPerStrip + 2) * NumLines;

         AllocateArrays();
         base.Initialize();
         InitializeBscEffectParameters();
      }

      void AllocateArrays()
      {
         VertexPts = new Vector3[NumColumns + 1, NumLines + 1];
         TexturePts = new Vector2[NumColumns + 1, NumLines + 1];
         Vertices = new VertexPositionTexture[NumVertices];
      }

      void InitializeBscEffectParameters()
      {
         BscEffect = new BasicEffect(GraphicsDevice);
         BscEffect.TextureEnabled = true;
         BscEffect.Texture = SphereTexture;
      }

      protected override void InitializeVertices()
      {
         PopulateVertexPts();
         PopulateTexturePts();
         PopulateVertices();
      }

      void PopulateVertexPts()
      {
         float angle = (float)(2 * Math.PI) / NumColumns;
         float phi = 0;
         float theta = 0;

         for (int j = 0; j < VertexPts.GetLength(0); ++j)
         {
            for (int i = 0; i < VertexPts.GetLength(1); ++i)
            {
               VertexPts[i, j] = new Vector3(Origin.X + Radius * (float)(Math.Sin(phi) * Math.Cos(theta)),
                                              Origin.Z + Radius * (float)(Math.Cos(phi)),
                                              Origin.Y + Radius * (float)(Math.Sin(phi) * Math.Sin(theta)));
               theta += angle;
            }
            phi += (float)Math.PI / NumLines;
         }
      }

      void PopulateTexturePts()
      {
         for (int i = 0; i < TexturePts.GetLength(0); ++i)
         {
            for (int j = 0; j < TexturePts.GetLength(1); ++j)
            {
               TexturePts[i, j] = new Vector2(i / (float)NumColumns, -j / (float)NumLines);
            }
         }
      }

      void PopulateVertices()
      {
         int vertexIndex = -1;
         for (int j = 0; j < NumLines; ++j)
         {
            for (int i = 0; i < NumColumns + 1; ++i)
            {
               Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[i, j], TexturePts[i, j]);
               Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);
            }
         }
      }

      protected override void LoadContent()
      {
         TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
         SphereTexture = TextureMgr.Find(TextureName);
         base.LoadContent();
      }

      public override void Draw(GameTime gameTime)
      {
         BscEffect.World = GetWorld();
         BscEffect.View = GameCamera.View;
         BscEffect.Projection = GameCamera.Projection;
         foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
         {
            passEffect.Apply();
            for (int i = 0; i < NumLines; ++i)
            {
               DrawTriangleStrip(i);
            }
         }
      }

      void DrawTriangleStrip(int stripIndex)
      {
         int vertexOffset = (stripIndex * NumVertices) / NumLines;
         GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, vertexOffset, NumTrianglesPerStrip);
      }

   }
}
