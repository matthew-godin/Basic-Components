using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    public class TexturedSphere : AnimatedBasicPrimitive, ICollidable
    {
        Texture2D Texture { get; set; }
        ResourceManager<Texture2D> TextureMgr { get; set; }
        BasicEffect BscEffect { get; set; }
        Vector2 Dimensions { get; set; }
        float Radius { get; set; }
        string TextureName { get; set; }
        float Delta { get; set; }
        Vector3 Origin { get; set; }
        bool Colision { get; set; }

        Vector3[,] VertexPts { get; set; }
        Vector2[,] TexturePts { get; set; }
        VertexPositionTexture[] Vertices { get; set; }
        List<Vector3> pointList { get; set; }

        public TexturedSphere(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, float radius, Vector2 dimensions, string textureName, float updateInterval) 
            : base(game, initialScale, initialRotation, initialPosition, updateInterval)
        {
            Dimensions = dimensions;
            Radius = radius;
            TextureName = textureName;
            NumTriangles = (int)(Dimensions.X * Dimensions.Y * 2);
            Origin = new Vector3(-(float)(Math.PI * Radius), -Radius, (float)(Math.PI * Radius));
        }
        public BoundingSphere CollisionSphere
        {
            get
            {
                return new BoundingSphere(Position, Radius);
            }
        }

        public override void Initialize()
        {
            Delta = (Radius * 2) / Dimensions.X;
            VertexPts = new Vector3[(int)(Dimensions.Y + 1),(int)(Dimensions.X + 1)]; //row,column
            TexturePts = new Vector2[(int)(Dimensions.Y + 1), (int)(Dimensions.X + 1)];
            Vertices = new VertexPositionTexture[NumTriangles * 3];
            CreateTexturePointArray();
            CreatePointArray();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
            Texture = TextureMgr.Find(TextureName);
            BscEffect = new BasicEffect(GraphicsDevice);
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = Texture;
            base.LoadContent();
        }

        private void CreateTexturePointArray()
        {
            for (int row = 0; row < Dimensions.Y + 1; ++row)
            {
                for (int column = 0; column < Dimensions.X + 1; ++column)
                {
                    TexturePts[row, column] = new Vector2((1f/Dimensions.X) * column, 1 - (1f/Dimensions.Y) * row);
                }
            }
        }

        private void CreatePointArray()
        {
            List<float> listHeight = new List<float>();
            for(int i = 0; i <= Dimensions.X / 2;++i)
            {
                listHeight.Add((2 * i) / Dimensions.X);
            }
            float DeltaHeight = -10; //for the 2
            for (int row = 0; row < Dimensions.Y + 1; ++row)
            {
                float Phi = 0; //for the 1
                DeltaHeight += 90f / (Dimensions.X / 2); //must change every row change
                for (int column = 0; column < Dimensions.X + 1; ++column)
                {
                    if (row <= 10)
                        VertexPts[row, column] = new Vector3((float)Math.Sin(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHeight)), //1 : turns the plane, 2 : reduces the x from 0 to 90 to 0
                                                               (-Radius + Radius*listHeight[row]*listHeight[row]),                                                                   //1 : reduces the y from 0 to 90 to 0
                                                               (float)Math.Cos(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHeight))); //1 : turns the plane, 2 : reduces the z from 0 to 90 to 0
                    else
                    {
                        VertexPts[row, column] = new Vector3((float)Math.Sin(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHeight)), //1 : turns the plane, 2 : reduces the x from 0 to 90 to 0
                                                              (Radius - Radius * listHeight[20 - row] * listHeight[20 - row]),                                                                   //1 : reduces the y from 0 to 90 to 0
                                                              (float)Math.Cos(MathHelper.ToRadians(Phi)) * (float)Math.Sin(MathHelper.ToRadians(DeltaHeight))); //1 : turns the plane, 2 : reduces the z from 0 to 90 to 0
                    }
                    Phi += 360f / Dimensions.X; //must always change
                }
            }
            pointList = new List<Vector3>();
            foreach (Vector3 v in VertexPts)
            {
                pointList.Add(v);
            }
        }

        protected override void InitializeVertices()
        {
            int vertexIndex = -1;
            for (int ligne = 0; ligne < Dimensions.Y; ++ligne)
            {
                for (int column = 0; column < Dimensions.X; ++column)
                {
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[ligne, column], TexturePts[ligne, column]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[ligne + 1, column], TexturePts[ligne + 1, column]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[ligne + 1, column + 1], TexturePts[ligne + 1, column + 1]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[ligne, column], TexturePts[ligne, column]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[ligne + 1, column + 1], TexturePts[ligne+1, column +1]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[ligne, column + 1], TexturePts[ligne, column + 1]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Vertices, 0, NumTriangles);
            }
        }

        public bool IsColliding(object otherObject)
        {
            return CollisionSphere.Intersects((otherObject as TexturedCube).CollisionSphere);
        }
    }
}