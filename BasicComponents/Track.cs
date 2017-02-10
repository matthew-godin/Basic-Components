using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    public class Track : BasicAnimatedPrimitive
   {
      Vector3[] VertexPts { get; set; }
      BasicEffect BscEffect { get; set; }
      TrackData TckData { get; set; }
      GroundWithBase Ground { get; set; }
      List<Vector2> InnerBorder { get; set; }
      List<Vector2> OuterBorder { get; set; }
      VertexPositionColors[] Vertices { get; set; }

      public Track(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, updateInterval) { }

      protected override void InitializeVertices()
      {
         for (int i = 0; i < NumVertices; ++i)
         {
            Vertices[i] = new VertexPositionColors(VertexPts[i], Color.Black);
         }
      }

      public override void Initialize()
      {
         base.Initialize();
         InnerBorder = TckData.GetInnerBorder();
         OuterBorder = TckData.GetOuterBorder();
         NumTriangles = (InnerBorder.Count - 1) * 2;
         NumVertices = InnerBorder.Count + OuterBorder.Count + 2;
         VertexPts = new Vector3[NumVertices];
         Vertices = new VertexPositionColors[NumVertices];
         CreatePointArray();
         InitializeVertices();
      }

        protected override void LoadContent()
        {
            TckData = Game.Services.GetService(typeof(TrackData)) as TrackData;
            Ground = Game.Services.GetService(typeof(GroundWithBase)) as GroundWithBase;
            BscEffect = new BasicEffect(GraphicsDevice);
            InitializeBscEffectParameters();
            base.LoadContent();
        }

        void InitializeBscEffectParameters()
        {
            BscEffect.VertexColoredEnabled = true;
        }


        void CreatePointArray()
        {
            for (int i = 0; i < InnerBorder.Count; ++i)
            {
                VertexPts[2 * i] = Ground.GetPointSpatial((int)InnerBorder[i].X, Ground.NumRows - (int)InnerBorder[i].Y);
                VertexPts[2 * i + 1] = Ground.GetPointSpatial((int)OuterBorder[i].X, Ground.NumRows - (int)OuterBorder[i].Y);

                if (i == InnerBorder.Count - 1)
                {
                    VertexPts[2 * i - 1] = Ground.GetPointSpatial((int)OuterBorder[i].X, Ground.NumRows - (int)OuterBorder[i].Y);
                    VertexPts[2 * i] = Ground.GetPointSpatial((int)InnerBorder[i].X, Ground.NumRows - (int)InnerBorder[i].Y);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            BscEffect.World = GetWorld();
            BscEffect.View = GameCamera.View;
            BscEffect.Projection = GameCamera.Projection;
            ManageTrackVisibility();
            foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
            {
                passEffect.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColors>(PrimitiveType.TriangleStrip, Vertices, 0, NumTriangles);
            }
        }

      void ManageTrackVisibility()
      {
         DepthStencilState old = GraphicsDevice.DepthStencilState;
         DepthStencilState gameDepthBufferState = new DepthStencilState();
         gameDepthBufferState.DepthBufferFunction = old.DepthBufferFunction;
         gameDepthBufferState.DepthBufferEnable = false;
         GraphicsDevice.DepthStencilState = gameDepthBufferState;
      }
   }
}
