using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
    public class GroundWithBase : BasicAnimatedPrimitive
    {
        const int NUM_TRIANGLES_PAR_TUILE = 2;
        const int NUM_VERTICES_PER_TRIANGLE = 3;
        const int NUM_TILES_PER_SIDE = 256;     //for 256x256 dimensions   

        string GroundMapName { get; set; }
        string TextureNameBase { get; set; }
        string[] TextureNameGround { get; set; }
        
        Color[] TextureMapData { get; set; }
        VertexPositionTexture[] VerticesBase { get; set; }

        Texture2D Ground { get; set; }
        Texture2D TextureBase { get; set; }
        Texture2D SandTexture { get; set; }
        Texture2D GrassTexture { get; set; }
        
        public int NumRows { get; set; }
        public int NumColumns { get; set; }

        public Vector3 Origin { get; private set; }
        Vector3 Delta { get; set; }

        Texture2D CombinedTexture { get; set; }
        BasicEffect BscEffect { get; set; }
        ResourcesManager<Texture2D> TextureMgr { get; set; }

        VertexPositionTexture[] Vertices { get; set; }
        Vector3 Span { get; set; }
        Vector3[,] VertexPts { get; set; }
        Vector2[,] TexturePts { get; set; }
        Vector3[,] VertexPtsBase { get; set; }

        Vector3[] VertexPtsBefore { get; set; }
        VertexPositionTexture[] VerticesBefore { get; set; }

        Vector3[] VertexPtsRight { get; set; }
        VertexPositionTexture[] VerticesRight { get; set; }

        Vector3[] VertexPtsLeft { get; set; }
        VertexPositionTexture[] BackVertices { get; set; }

        Vector3[] BackVertexPts { get; set; }
        VertexPositionTexture[] VerticesLeft { get; set; }

        public GroundWithBase(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector3 span, string nameGroundMap, string[] textureNameGround, string textureNameBase, float updateInterval)
           : base(game, initialScale, initialRotation, initialPosition, updateInterval)
        {
            Span = span;
            GroundMapName = nameGroundMap;
            TextureNameGround = textureNameGround;
            TextureNameBase = textureNameBase;
        }

        public Vector3 GetPointSpatial(int x, int y)
        {
            return new Vector3(VertexPts[x, y].X, VertexPts[x, y].Y, VertexPts[x, y].Z);
        }

        public Vector3 GetNormal(int x, int y)
        {
            Vector3 A = GetPointSpatial(x, y);
            Vector3 B = GetPointSpatial(x + 1, y); //the point to the right of A
            Vector3 C = GetPointSpatial(x, y + 1); //the point on top of A
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            return Vector3.Cross(AB, AC);
        }

        public override void Initialize()
        {
            TextureMgr = Game.Services.GetService(typeof(ResourcesManager<Texture2D>)) as ResourcesManager<Texture2D>;

            //for the map
            Ground = TextureMgr.Find(GroundMapName);
            TextureMapData = new Color[Ground.Width * Ground.Height];
            Ground.GetData<Color>(TextureMapData);

            //for the ground
            TextureBase = TextureMgr.Find(TextureNameBase);
            SandTexture = TextureMgr.Find(TextureNameGround[0]);
            GrassTexture = TextureMgr.Find(TextureNameGround[1]);

            NumColumns = Ground.Width - 1;
            NumRows = Ground.Height - 1;
            NumVertices = (NumColumns) * (NumRows) * NUM_VERTICES_PER_TRIANGLE * NUM_TRIANGLES_PAR_TUILE;
            Origin = new Vector3(-Span.X / 2, 0, -Span.Z / 2);
            Delta = new Vector3(Span.X / NumColumns, Span.Y / 255f, Span.Z / NumRows);
            
            CreateArray();
            CreateArrayVertexPts();
            CreateArrayTexturePts();
            CombineTextures();
            CreateArrayBases();
            CreateArrayVertexPtsBase();
            InitializeVerticesBase();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            BscEffect = new BasicEffect(GraphicsDevice);
            BscEffect.TextureEnabled = true;
        }

        protected override void InitializeVertices()
        {
            int cpt = -1;
            for (int j = 0; j < NumRows; j++)
            {
                for (int i = 0; i < NumColumns; i++)
                {
                    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j], TexturePts[i + 1, j]);
                    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j], TexturePts[i, j]);
                    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i, j + 1], TexturePts[i, j + 1]);
                    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j], TexturePts[i + 1, j]);
                    Vertices[++cpt] = new VertexPositionTexture(VertexPts[i + 1, j + 1], TexturePts[i + 1, j + 1]);
                }
            }
        }

        void InitializeVerticesBase()
        {
            Vector2 TextureBottomLeft = new Vector2(0, 1);
            Vector2 TextureTopLeft = Vector2.Zero;
            Vector2 TextureBottomRight = new Vector2(1, 1);
            Vector2 TextureTopRight = new Vector2(1, 0);

            for (int i = 0; i < 2 * NumColumns; i++)
            {
                VerticesBefore[i] = new VertexPositionTexture(VertexPtsBefore[i++], TextureBottomLeft);
                VerticesBefore[i] = new VertexPositionTexture(VertexPtsBefore[i++], TextureTopLeft);
                VerticesBefore[i] = new VertexPositionTexture(VertexPtsBefore[i++], TextureBottomRight);
                VerticesBefore[i] = new VertexPositionTexture(VertexPtsBefore[i], TextureTopLeft);
            }
            VerticesBefore[512] = new VertexPositionTexture(VertexPtsBefore[512], TextureBottomLeft);
            VerticesBefore[513] = new VertexPositionTexture(VertexPtsBefore[513], TextureTopLeft);

            for (int i = 0; i < 2 * NumRows; i++)
            {
                VerticesRight[i] = new VertexPositionTexture(VertexPtsRight[i++], TextureBottomLeft);
                VerticesRight[i] = new VertexPositionTexture(VertexPtsRight[i++], TextureTopLeft);
                VerticesRight[i] = new VertexPositionTexture(VertexPtsRight[i++], TextureBottomRight);
                VerticesRight[i] = new VertexPositionTexture(VertexPtsRight[i], TextureTopRight);
            }
            VerticesRight[512] = new VertexPositionTexture(VertexPtsRight[512], TextureBottomLeft);
            VerticesRight[513] = new VertexPositionTexture(VertexPtsRight[513], TextureTopLeft);

            for (int i = 0; i < 2 * NumRows; i++)
            {
                BackVertices[i] = new VertexPositionTexture(BackVertexPts[i++], TextureBottomLeft);
                BackVertices[i] = new VertexPositionTexture(BackVertexPts[i++], TextureTopLeft);
                BackVertices[i] = new VertexPositionTexture(BackVertexPts[i++], TextureBottomRight);
                BackVertices[i] = new VertexPositionTexture(BackVertexPts[i], TextureTopRight);
            }
            BackVertices[512] = new VertexPositionTexture(BackVertexPts[512], TextureBottomLeft);
            BackVertices[513] = new VertexPositionTexture(BackVertexPts[513], TextureTopLeft);

            for (int i = 0; i < 2 * NumColumns; i++)
            {
                VerticesLeft[i] = new VertexPositionTexture(VertexPtsLeft[i++], TextureBottomLeft);
                VerticesLeft[i] = new VertexPositionTexture(VertexPtsLeft[i++], TextureTopLeft);
                VerticesLeft[i] = new VertexPositionTexture(VertexPtsLeft[i++], TextureBottomRight);
                VerticesLeft[i] = new VertexPositionTexture(VertexPtsLeft[i], TextureTopRight);
            }
            VerticesLeft[512] = new VertexPositionTexture(VertexPtsLeft[512], TextureBottomLeft);
            VerticesLeft[513] = new VertexPositionTexture(VertexPtsLeft[513], TextureTopLeft);

        }

        void CreateArray()
        {
            Vertices = new VertexPositionTexture[NumVertices];
            VertexPts = new Vector3[Ground.Width, Ground.Height];
            TexturePts = new Vector2[Ground.Width, Ground.Height];
        }

        void CreateArrayVertexPts()
        {
            for (int i = 0; i < VertexPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VertexPts.GetLength(1); ++j)
                {
                    VertexPts[i, j] = new Vector3(Origin.X + (i * Delta.X), Origin.Y + (TextureMapData[j * VertexPts.GetLength(0) + i].B * Delta.Y), Origin.Z + (j * Delta.Z));
                }
            }
        }

        void CreateArrayTexturePts()
        {
            for (int i = 0; i < TexturePts.GetLength(0); ++i)
            {
                for (int j = 0; j < TexturePts.GetLength(1); ++j)
                {
                    TexturePts[i, j] = new Vector2(i / (float)NumColumns, -j / (float)NumRows);
                }
            }
        }

        void CreateArrayBases()
        {
            VertexPtsBefore = new Vector3[2 * (NumColumns + 1)];
            VerticesBefore = new VertexPositionTexture[2 * (NumColumns + 1)];

            VertexPtsRight = new Vector3[2 * (NumRows + 1)];
            VerticesRight = new VertexPositionTexture[2 * (NumRows + 1)];

            VertexPtsLeft = new Vector3[2 * (NumRows + 1)];
            VerticesLeft = new VertexPositionTexture[2 * (NumRows + 1)];

            BackVertexPts = new Vector3[2 * (NumColumns + 1)];
            BackVertices = new VertexPositionTexture[2 * (NumColumns + 1)];
        }

        void CreateArrayVertexPtsBase()
        {
            int cpt = 0;
            for (int i = 0; i < 2 * (NumColumns + 1) - 1; i++)
            {
                BackVertexPts[i] = new Vector3(Origin.X + cpt * Delta.X, VertexPts[i / 2, 0].Y, Origin.Z);
                BackVertexPts[++i] = new Vector3(Origin.X + cpt * Delta.X, 0, Origin.Z);
                cpt++;
            }

            cpt = 256;
            for (int i = 0; i < 2 * (NumColumns + 1) - 1; i++)
            {
                VertexPtsLeft[i] = new Vector3(Origin.X, VertexPts[0, ((-1 * i) + (2 * (NumColumns + 1) - 1)) / 2].Y, Origin.Z + cpt * Delta.Z);
                VertexPtsLeft[++i] = new Vector3(Origin.X, 0, Origin.Z + cpt * Delta.Z);
                cpt--;
            }

            cpt = 256;
            for (int i = 0; i < 2 * (NumColumns + 1) - 1; i++)
            {
                VertexPtsBefore[i] = new Vector3(Origin.X + cpt * Delta.X, VertexPts[((-1 * i) + (2 * (NumColumns + 1) - 1)) / 2, VertexPts.GetLength(1) - 1].Y, Origin.Z + Ground.Height - 1);
                VertexPtsBefore[++i] = new Vector3(Origin.X + cpt * Delta.X, 0, Origin.Z + Ground.Height - 1);
                cpt--;
            }

            cpt = 0;
            for (int i = 0; i < 2 * (NumRows + 1) - 1; ++i)
            {
                VertexPtsRight[i] = new Vector3(Origin.X + Ground.Width - 1, VertexPts[VertexPts.GetLength(0) - 1, i / 2].Y, Origin.Z + cpt * Delta.Z);
                VertexPtsRight[++i] = new Vector3(Origin.X + Ground.Width - 1, 0, Origin.Z + cpt * Delta.Z);
                cpt++;
            }
        }
        
        void CombineTextures()
        {
            CombinedTexture = new Texture2D(SandTexture.GraphicsDevice, VertexPts.GetLength(0), VertexPts.GetLength(1));
            int numTexels = VertexPts.GetLength(0) * VertexPts.GetLength(0);
            Color[] texels = new Color[numTexels];
            SandTexture.GetData(texels);

            Color[] grassTexels = new Color[GrassTexture.Width * GrassTexture.Height];
            GrassTexture.GetData(grassTexels);

            Color[] sandTexels = new Color[SandTexture.Width * SandTexture.Height];
            SandTexture.GetData(sandTexels);
            
            for (int texelIndex = 0; texelIndex < numTexels; ++texelIndex)
            {
                float j = TextureMapData[texelIndex].B / 255f;

                texels[texelIndex].R = (byte)((j * (byte)grassTexels[texelIndex].R) + (byte)((1 - j) * (byte)sandTexels[texelIndex].R));
                texels[texelIndex].G = (byte)((j * (byte)grassTexels[texelIndex].G) + (byte)((1 - j) * (byte)sandTexels[texelIndex].G));
                texels[texelIndex].B = (byte)((j * (byte)grassTexels[texelIndex].B) + (byte)((1 - j) * (byte)sandTexels[texelIndex].B));
            }
            CombinedTexture.SetData<Color>(texels);
        }

        public override void Draw(GameTime gameTime)
        {
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;

            BscEffect.Texture = CombinedTexture;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / NUM_VERTICES_PER_TRIANGLE);                
            }

            BscEffect.Texture = TextureBase;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, VerticesBefore, 0, NUM_TILES_PER_SIDE * NUM_TRIANGLES_PAR_TUILE);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, VerticesRight, 0, NUM_TILES_PER_SIDE * NUM_TRIANGLES_PAR_TUILE);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, BackVertices, 0, NUM_TILES_PER_SIDE * NUM_TRIANGLES_PAR_TUILE);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, VerticesLeft, 0, NUM_TILES_PER_SIDE * NUM_TRIANGLES_PAR_TUILE);
            }
        }
    }
}