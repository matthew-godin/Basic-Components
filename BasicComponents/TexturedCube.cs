using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
    {
        public class TexturedCube : AnimatedBasicPrimitive, ICollidable, IDestructible
        {
            //Constants
            const int NUM_VERTICES = 8;
            const int NUM_TRIANGLES = 6;

            const int NUM_COLUMNS = 3;
            const int NUM_LINES = 1;

            const int NUM_STRIPS_TO_DISPLAY = 2;

            //Initially managed by the constructor
            public string TextureNameCube { get; set; }
            readonly Vector3 Delta;
            readonly Vector3 Origin;

            //Initially managed by Initialize()
            VertexPositionTexture[] Vertices { get; set; }
            protected Vector3[] VertexPts { get; set; }
            Vector2[,] TexturePts { get; set; }

            //Gérée initialement par LoadContent()
            BasicEffect BscEffect { get; set; }
            ResourceManager<Texture2D> TextureMgr { get; set; }
            Texture2D Texture { get; set; }


            public bool ToDestroy { get; set; }

            public bool IsColliding(object otherObject)
            {
                BoundingSphere obj2 = (BoundingSphere)otherObject;
                return obj2.Intersects(CollisionSphere);
            }

            public BoundingSphere CollisionSphere
            {
                get
                {
                    return new BoundingSphere(Position, 2 * Delta.X);
                }
            }


            public TexturedCube(Game game, float initialScale, Vector3 initialRotation,
                            Vector3 initialPosition, string textureNameCube, Vector3 dimension,
                            float updateInterval)
                : base(game, initialScale, initialRotation, initialPosition, updateInterval)
            {
                TextureNameCube = textureNameCube;
                Delta = dimension;
                Origin = new Vector3(-Delta.X / 2, -Delta.Y / 2, Delta.Z / 2);
            }

            public override void Initialize()
            {
                ToDestroy = false;
                AllocateArrays();

                base.Initialize();
                InitializeBscEffectParameters();
            }

            void AllocateArrays()
            {
                VertexPts = new Vector3[NUM_VERTICES];
                TexturePts = new Vector2[NUM_COLUMNS + 1, NUM_LINES + 1];
                Vertices = new VertexPositionTexture[NUM_VERTICES * 2];
            }

            public void InitializeBscEffectParameters()
            {
                Texture = TextureMgr.Find(TextureNameCube);
                BscEffect = new BasicEffect(GraphicsDevice);
                BscEffect.TextureEnabled = true;
                BscEffect.Texture = Texture;
            }

            protected override void InitializeVertices()
            {
                PopulateVertexPts();
                PopulateTexturePts();
                PopulateVertices();
            }

            void PopulateVertexPts()
            {
                //Base vertices from the bottom to the origin (clockwise observing from top)
                VertexPts[0] = new Vector3(-Delta.X / 2, -Delta.Y / 2, Delta.Z / 2);
                VertexPts[1] = new Vector3(VertexPts[0].X, VertexPts[0].Y, VertexPts[0].Z - Delta.Z);
                VertexPts[2] = new Vector3(VertexPts[0].X + Delta.X, VertexPts[0].Y, VertexPts[0].Z - Delta.Z);
                VertexPts[3] = new Vector3(VertexPts[0].X + Delta.X, VertexPts[0].Y, VertexPts[0].Z);

                //Base vertices from the top to the origin (counterclockwise observing from top)
                VertexPts[4] = new Vector3(VertexPts[0].X, VertexPts[0].Y + Delta.Y, VertexPts[0].Z);
                VertexPts[5] = new Vector3(VertexPts[0].X, VertexPts[0].Y + Delta.Y, VertexPts[0].Z - Delta.Z);
                VertexPts[6] = new Vector3(VertexPts[0].X + Delta.X, VertexPts[0].Y + Delta.Y, VertexPts[0].Z - Delta.Z);
                VertexPts[7] = new Vector3(VertexPts[0].X + Delta.X, VertexPts[0].Y + Delta.Y, VertexPts[0].Z);
            }

            void PopulateTexturePts()
            {
                for (int i = 0; i < TexturePts.GetLength(0); ++i)
                {
                    for (int j = 0; j < TexturePts.GetLength(1); ++j)
                    {
                        TexturePts[i, j] = new Vector2(i / (float)NUM_COLUMNS, j / (float)NUM_LINES);
                    }
                }
            }

            protected void PopulateVertices()
            {
                Vertices[0] = new VertexPositionTexture(VertexPts[0], TexturePts[0, 1]);
                Vertices[1] = new VertexPositionTexture(VertexPts[4], TexturePts[0, 0]);
                Vertices[2] = new VertexPositionTexture(VertexPts[3], TexturePts[1, 1]);
                Vertices[3] = new VertexPositionTexture(VertexPts[7], TexturePts[1, 0]);
                Vertices[4] = new VertexPositionTexture(VertexPts[2], TexturePts[2, 1]);
                Vertices[5] = new VertexPositionTexture(VertexPts[6], TexturePts[2, 0]);
                Vertices[6] = new VertexPositionTexture(VertexPts[1], TexturePts[3, 1]);
                Vertices[7] = new VertexPositionTexture(VertexPts[5], TexturePts[3, 0]);


                Vertices[8] = new VertexPositionTexture(VertexPts[3], TexturePts[0, 1]);
                Vertices[9] = new VertexPositionTexture(VertexPts[2], TexturePts[0, 0]);
                Vertices[10] = new VertexPositionTexture(VertexPts[0], TexturePts[1, 1]);
                Vertices[11] = new VertexPositionTexture(VertexPts[1], TexturePts[1, 0]);
                Vertices[12] = new VertexPositionTexture(VertexPts[4], TexturePts[2, 1]);
                Vertices[13] = new VertexPositionTexture(VertexPts[5], TexturePts[2, 0]);
                Vertices[14] = new VertexPositionTexture(VertexPts[7], TexturePts[3, 1]);
                Vertices[15] = new VertexPositionTexture(VertexPts[6], TexturePts[3, 0]);
            }

            protected override void LoadContent()
            {
                TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
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
                    for (int i = 0; i < NUM_STRIPS_TO_DISPLAY; ++i)
                    {
                        DrawTriangleStrip(i);
                    }
                }
            }

            void DrawTriangleStrip(int stripIndex)
            {
                int vertexOffset = stripIndex * NUM_VERTICES;
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, vertexOffset, NUM_TRIANGLES);
            }
        }
    }