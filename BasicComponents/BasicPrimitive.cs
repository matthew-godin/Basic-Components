using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace XNAProject
{
   public abstract class BasicPrimitive : Microsoft.Xna.Framework.DrawableGameComponent
   {
      protected float InitialScale { get; private set; }
      protected Vector3 InitialRotation { get; private set; }
      protected Vector3 InitialPosition { get; private set; }
      protected int NumVertices { get; set; }
      protected int NumTriangles { get; set; }
      protected Matrix World { get; set; }
      protected Camera GameCamera { get; private set; }

      protected BasicPrimitive(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition)
         : base(game)
      {
         InitialScale = initialScale;
         InitialRotation = initialRotation;
         InitialPosition = initialPosition;
      }

      public override void Initialize()
      {
         InitializeVertices();
         ComputeWorldMatrix();
         base.Initialize();
      }

      protected override void LoadContent()
      {
         GameCamera = Game.Services.GetService(typeof(Camera)) as Camera;
         base.LoadContent();
      }

      protected virtual void ComputeWorldMatrix()
      {
         World = Matrix.Identity *
                 Matrix.CreateScale(InitialScale) *
                 Matrix.CreateFromYawPitchRoll(InitialRotation.Y, InitialRotation.X, InitialRotation.Z) *
                 Matrix.CreateTranslation(InitialPosition);
      }

      protected abstract void InitializeVertices();

      public virtual Matrix GetWorld()
      {
         return World;
      }
   }
}