using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public abstract class Tile : BasicPrimitive//AnimatedBasicPrimitive
   {
      const int NUM_TRIANGLES = 2;
      protected Vector3[,] VertexPts { get; private set; }
      Vector3 Origin { get; set; }
      Vector2 Delta { get; set; }
      protected BasicEffect BscEffect { get; private set; }


      public Tile(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, 
                   Vector2 span/*, float updateInterval*/)
         : base(game, initialScale, initialRotation, initialPosition/*, updateInterval*/)
      {
         Delta = new Vector2(span.X, span.Y);
         Origin = new Vector3(-Delta.X / 2, -Delta.Y / 2, 0); //to center the primitive to point (0,0,0)
      }

      public override void Initialize()
      {
         NumVertices = NUM_TRIANGLES + 2;
         VertexPts = new Vector3[2, 2];
         CreatePointArray();
         CreateVertexArray();
         base.Initialize();
      }

      private void CreatePointArray()
      {
         VertexPts[0, 0] = new Vector3(Origin.X, Origin.Y, Origin.Z);
         VertexPts[1, 0] = new Vector3(Origin.X + Delta.X, Origin.Y, Origin.Z);
         VertexPts[0, 1] = new Vector3(Origin.X, Origin.Y + Delta.Y, Origin.Z);
         VertexPts[1, 1] = new Vector3(Origin.X + Delta.X, Origin.Y + Delta.Y, Origin.Z);
      }

      protected abstract void CreateVertexArray();

      protected override void LoadContent()
      {
         BscEffect = new BasicEffect(GraphicsDevice);
         InitializeBscEffectParameters();
         base.LoadContent();
      }

      protected abstract void InitializeBscEffectParameters();

      public override void Draw(GameTime gameTime)
      {
         BscEffect.World = GetWorld();
         BscEffect.View = GameCamera.View;
         BscEffect.Projection = GameCamera.Projection;
         foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
         {
            passEffect.Apply();
            DrawTriangleStrip();
         }
      }

      protected abstract void DrawTriangleStrip();
   }
}

