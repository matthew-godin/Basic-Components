/*
Ground.cs
----------

By Matthew Godin and Marc-Olivier Fillion

Role : Displays ground based
       on a height map

Created : 7 December 2016
*/
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class Ground : AnimatedBasicPrimitive
   {
      const int NUM_TRIANGLES_PER_TILE = 2;
      const int NUM_VERTICES_PER_TRIANGLE = 3;
      const float MAX_COLOR = 255f;

        const int NO_VERTEX_OFFSET = 0;
        const int BEFORE_FIRST_VERTEX = -1;
        const int NUM_TRIANGLES = 2;
        const int NULL_SIDE = 0;
        const int FIRST_VERTICES_OF_STRIP = 2;

        const int ADDITIONAL_VERTEX_FOR_LINE = 1;
        const int NULL_COMPENSATION = 0;
        const int HALF_DIVISOR = 2;
        const int NUM_TEXTURE_PTS_POSSIBLE = 20;
        const int NUM_VERTICES_PER_TILE = 4;
        const int NULL_Y = 0;
        const int TEXEL_POSITION_REMOVED_RELATIVE_TO_DIMENSION = 1;
        const int J_MAX_VALUE_TOP_TILE = 1;
        const int TOP_TILE_VALUE = 0;
        const int BOTTOM_TILE_VALUE = 1;

        Vector3 Span { get; set; }
      string GroundMapName { get; set; }
      string TextureNameGround { get; set; }
      int NumTextureLevels { get; set; }
 
      BasicEffect BscEffect { get; set; }
      ResourcesManager<Texture2D> TextureMgr { get; set; }
      Texture2D GroundMap { get; set; }
      Texture2D TextureGround { get; set; }
      Vector3 Origin { get; set; }

        // To complete with necessary properties
        int NumRows { get; set; }
        int NumColumns { get; set; }
        Color[] DataTexture { get; set; }
        int TileWidth { get; set; }
        Vector3[,] VertexPts { get; set; }
        Vector2[] TexturePts { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        Vector2 Delta { get; set; }
        int NumTexels { get; set; }

        public Ground(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector3 span, string nameGroundMap, string textureNameGround, int numTextureLevels, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, updateInterval)
      {
         Span = span;
         GroundMapName = nameGroundMap;
         TextureNameGround = textureNameGround;
         NumTextureLevels = numTextureLevels;
      }

      public override void Initialize()
      {
         TextureMgr = Game.Services.GetService(typeof(ResourcesManager<Texture2D>)) as ResourcesManager<Texture2D>;
         InitializeMapData();
         InitializeTextureData();
         Origin = new Vector3(-Span.X / HALF_DIVISOR, 0, -Span.Z / HALF_DIVISOR); //to center the primitive to point (0,0,0)##########################Minus Z removed
         CreatePointArray(); // ############### INVERTED
         AllocateArrays(); // ################## INVERTED
         base.Initialize();
      }

      //
      // We initialize the texture by reading the height map
      // representing the ground
      //
      void InitializeMapData()
      {
            // TODO
            GroundMap = TextureMgr.Find(GroundMapName);
            NumRows = GroundMap.Width - ADDITIONAL_VERTEX_FOR_LINE;
            NumColumns = GroundMap.Height - ADDITIONAL_VERTEX_FOR_LINE;
            NumTriangles = NumRows * NumColumns * NUM_TRIANGLES_PER_TILE;
            NumVertices = NumTriangles * NUM_VERTICES_PER_TRIANGLE;
            NumTexels = GroundMap.Width * GroundMap.Height;
            DataTexture = new Color[NumTexels];
            GroundMap.GetData<Color>(DataTexture);
        }

      //
      // from the texture containing the heightmap textures (HeightMap), we initialize the data
      // representing the ground texture
      //
      void InitializeTextureData()
      {
            // TODO
            TextureGround = TextureMgr.Find(TextureNameGround);
            TileWidth = (int)(TextureGround.Height / (float)NumTextureLevels);
        }

      //
      // Alocation of the two arrays
      //    1) the one containing the vertices (unique points), 
      //    2) the one containing the vertices to draw the triangles
      void AllocateArrays()
      {
            // TODO
            Vertices = new VertexPositionTexture[NumVertices];
            //TexturePts = new Vector2[GroundMap.Width, GroundMap.Height];
            TexturePts = new Vector2[NUM_TEXTURE_PTS_POSSIBLE];
            //VertexPts = new Vector3[GroundMap.Width, GroundMap.Height];
            //Delta = new Vector2(Span.X / NumRows, Span.Z / NumColumns);
            PopulateTexturePoints();
            InitializeVertices();
        }

        void PopulateTexturePoints()
        {
            //for (int i = 0; i < TexturePts.GetLength(0); ++i)
            //{
            //    for (int j = 0; j < TexturePts.GetLength(1); ++j)
            //    {
            //        TexturePts[i, j] = new Vector2(0, VertexPts[i, j].Y / Span.Y);
            //    }
            //}
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    TexturePts[NUM_VERTICES_PER_TILE * i + j] = new Vector2(j % NUM_TRIANGLES_PER_TILE, (i + (j > J_MAX_VALUE_TOP_TILE ? BOTTOM_TILE_VALUE : TOP_TILE_VALUE) * (1 - 1 / (float)TextureGround.Height)) / NumTextureLevels);
                    //TexturePts[NUM_VERTICES_PER_TILE * i + j] = new Vector2(0.5f, 0.9f);
                }
            }
            Game.Window.Title = TexturePts[11].ToString();
        }

        protected override void LoadContent()
      {
         base.LoadContent();
         BscEffect = new BasicEffect(GraphicsDevice);
         InitializeBscEffectParameters();
      }

      void InitializeBscEffectParameters()
      {
         BscEffect.TextureEnabled = true;
         BscEffect.Texture = TextureGround;
      }

      //
      // Creation of the vertex array
      // This process creates 3D coordinates from 2D texture pixels
      //
      private void CreatePointArray()
      {
            // TODO
            VertexPts = new Vector3[GroundMap.Width, GroundMap.Height];
            Delta = new Vector2(Span.X / NumRows, Span.Z / NumColumns);
            for (int i = 0; i < VertexPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VertexPts.GetLength(1); ++j)
                {
                    VertexPts[i, j] = Origin + new Vector3(Delta.X * i, DataTexture[i * VertexPts.GetLength(1) + j].B / MAX_COLOR * Span.Y, Delta.Y * j);
                }
            }
        }

      //
      // Vertices creation.
      // Don't forget this is a TriangleList...
      //
      protected override void InitializeVertices()
      {
            // TODO
            int cpt = -1;
            for (int j = 0; j < NumRows; ++j)
            {
                for (int i = 0; i < NumColumns; ++i)
                {
                    //int val1 = (int)(VertexPts[i, j].Y + VertexPts[i + 1, j].Y + VertexPts[i, j + 1].Y) / 3;
                    //int woho = (int)(val1 / MAX_COLOR);

                    //Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j], TexturePts[i, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j], TexturePts[i + 1, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);

                    ////AssociéTruc(ref Vertices[cpt - 2], ref Vertices[cpt - 1], ref Vertices[cpt]);

                    //int val2 = (int)(((VertexPts[i + 1, j].Y + VertexPts[i + 1, j + 1].Y + VertexPts[i, j + 1].Y) / 3 - Origin.Y) / Delta.Y);
                    //woho = (int)(val2 / MAX_COLOR);

                    //Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j], TexturePts[i + 1, j]);
                    //Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j + 1], TexturePts[i + 1, j + 1]);
                    //Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);
                    PopulateTile(ref cpt, i, j);
                }
            }
        }

        void PopulateTile(ref int cpt, int i, int j)
        {
            int noCase = (int)((VertexPts[i, j].Y + VertexPts[i + 1, j].Y + VertexPts[i, j + 1].Y + VertexPts[i + 1, j + 1].Y) / 4.0f / Span.Y * 19) / 4 * 4;

            //for (int k = 0; k < 4; ++k)
            //{
            //    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j], TexturePts[k + noCase * ]);
            //} 
            Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j], TexturePts[noCase]);
            Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j], TexturePts[noCase + 1]);
            Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[noCase + 2]);
            Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j], TexturePts[noCase + 1]);
            Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j + 1], TexturePts[noCase + 3]);
            Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[noCase + 2]);
        }

      //
      // This method draws the ground...
      //
      public override void Draw(GameTime gameTime)
      {
            // TODO
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, NULL_COMPENSATION, NumTriangles);
            }
        }
   }
}
