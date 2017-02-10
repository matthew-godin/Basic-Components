using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    public class TexturedCube : AnimatedBasicPrimitive, ICollidable, IDestructible
    {
        const int NUM_VERTICES = 8;
        const int NUM_TRIANGLES = 6;

        protected VertexPositionTexture[] Vertices { get; set; }
        VertexPositionTexture[] Vertices2 { get; set; }
        
        Texture2D Texture { get; set; }
        ResourceManager<Texture2D> TextureMgr { get; set; }
        BlendState AlphaMgr { get; set; }
        BasicEffect BscEffect { get; set; }
        protected Vector3 Origin { get; set; }
        Vector3 Delta { get; set; }
        Vector3 DeltaModified { get; set; }
        float DeltaTexture { get; set; }
        string TextureNameCube { get; set; }
        bool toDestroy;

        public bool ToDestroy
        {
            get
            {
                return toDestroy;
            }
            set
            {
                toDestroy = value;
            }
        }

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

        protected List<Vector3> ListePoints { get; set; }

    public TexturedCube(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, string textureNameCube, 
            Vector3 dimension, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, updateInterval)
        {
            TextureNameCube = textureNameCube;
            Delta = new Vector3(dimension.X, dimension.Y, dimension.Z);
            DeltaModified = new Vector3(Delta.X / 2, Delta.Y / 2, Delta.Z / 2);
            DeltaTexture = 1f / 3;
            Origin = new Vector3(0, 0, 0);
        }

        public override void Initialize()
        {
            Vertices = new VertexPositionTexture[NUM_VERTICES];
            Vertices2 = new VertexPositionTexture[NUM_VERTICES];
            ListePoints = new List<Vector3>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
            Texture = TextureMgr.Find(TextureNameCube);
            BscEffect = new BasicEffect(GraphicsDevice);
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = Texture;
            AlphaMgr = BlendState.AlphaBlend;
            base.LoadContent();
        }

        protected override void InitializeVertices() //vestige pas tres beau du labo sur le cube...
        {
            Vector3 vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z + DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[0] = new VertexPositionTexture(vertexPosition, new Vector2(0,1));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z + DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[1] = new VertexPositionTexture(vertexPosition, new Vector2(0, 0));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z + DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[2] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture, 1));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z + DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[3] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture, 0));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z - DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[4] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture*2, 1));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z - DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[5] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture*2, 0));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z - DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[6] = new VertexPositionTexture(vertexPosition, new Vector2(1, 1));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z - DeltaModified.Z);
            ListePoints.Add(vertexPosition);
            Vertices[7] = new VertexPositionTexture(vertexPosition, new Vector2(1, 0));            

            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z - DeltaModified.Z);
            Vertices2[0] = new VertexPositionTexture(vertexPosition, new Vector2(0, 1));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z + DeltaModified.Z);
            Vertices2[1] = new VertexPositionTexture(vertexPosition, new Vector2(0, 0));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z - DeltaModified.Z);
            Vertices2[2] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture, 1));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y + DeltaModified.Y, Origin.Z + DeltaModified.Z);
            Vertices2[3] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture, 0));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z - DeltaModified.Z);
            Vertices2[4] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture*2, 1));
            vertexPosition = new Vector3(Origin.X - DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z + DeltaModified.Z);
            Vertices2[5] = new VertexPositionTexture(vertexPosition, new Vector2(DeltaTexture*2, 0));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z - DeltaModified.Z);
            Vertices2[6] = new VertexPositionTexture(vertexPosition, new Vector2(1, 1));
            vertexPosition = new Vector3(Origin.X + DeltaModified.X, Origin.Y - DeltaModified.Y, Origin.Z + DeltaModified.Z);
            Vertices2[7] = new VertexPositionTexture(vertexPosition, new Vector2(1, 0));
        }

        public override void Draw(GameTime gameTime)
        {
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, 0, NUM_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices2, 0, NUM_TRIANGLES);
            }
        }
    }
}