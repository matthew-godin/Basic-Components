using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace XNAProject
{
   public abstract class BasicAnimatedPrimitive : BasicPrimitive
   {
      protected float Scale { get; set; }
      protected Vector3 Position { get; set; }
      float UpdateInterval { get; set; }
      float TimeElpasedSinceUpdate { get; set; }
      protected InputManager InputMgr { get; private set; }
      float RotationAngleIncrement { get; set; }
      protected bool Yaw { get; set; }
      protected bool Pitch { get; set; }
      protected bool Roll { get; set; }
      protected bool WorldToRecompute { get; set; }

      float yawAngle;
      protected float YawAngle
      {
         get
         {
            if (Yaw)
            {
               yawAngle += RotationAngleIncrement;
               yawAngle = MathHelper.WrapAngle(yawAngle);
            }
            return yawAngle;
         }
         set { yawAngle = value; }
      }

      float pitchAngle;
      protected float PitchAngle
      {
         get
         {
            if (Pitch)
            {
               pitchAngle += RotationAngleIncrement;
               pitchAngle = MathHelper.WrapAngle(pitchAngle);
            }
            return pitchAngle;
         }
         set { pitchAngle = value; }
      }

      float rollAngle;
      protected float RollAngle
      {
         get
         {
            if (Roll)
            {
               rollAngle += RotationAngleIncrement;
               rollAngle = MathHelper.WrapAngle(rollAngle);
            }
            return rollAngle;
         }
         set { rollAngle = value; }
      }


      protected BasicAnimatedPrimitive(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, float updateInterval)
         : base(game, initialScale, initialRotation, initialPosition)
      {
         UpdateInterval = updateInterval;
      }

      public override void Initialize()
      {
         Scale = InitialScale;
         InitializeRotations();
         Position = InitialPosition;
         InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
         RotationAngleIncrement = MathHelper.Pi * UpdateInterval / 2;
         TimeElpasedSinceUpdate = 0;
         base.Initialize();
      }

      protected override void ComputeWorldMatrix()
      {
         World = Matrix.Identity *
                 Matrix.CreateScale(Scale) *
                 Matrix.CreateFromYawPitchRoll(YawAngle, PitchAngle, RollAngle) *
                 Matrix.CreateTranslation(Position);
      }

      public override void Update(GameTime gameTime)
      {
         ManageKeyboard();
         float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TimeElpasedSinceUpdate += timeElapsed;
         if (TimeElpasedSinceUpdate >= UpdateInterval)
         {
            PerformUpdate();
            TimeElpasedSinceUpdate = 0;
         }
      }

      protected virtual void PerformUpdate()
      {
         if (WorldToRecompute)
         {
            ComputeWorldMatrix();
            WorldToRecompute = false;
         }
      }

      private void InitializeRotations()
      {
         YawAngle = InitialRotation.Y;
         PitchAngle = InitialRotation.X;
         RollAngle = InitialRotation.Z;
      }

      protected virtual void ManageKeyboard()
      {
         //if (InputMgr.IsPressed(Keys.LeftControl) || InputMgr.IsPressed(Keys.RightControl))
         //{
         //   if (InputMgr.IsNewKey(Keys.Space))
         //   {
         //      InitializeRotations();
         //      WorldToRecompute = true;
         //   }
         //   if (InputMgr.IsNewKey(Keys.D1) || InputMgr.IsNewKey(Keys.NumPad1))
         //   {
         //      Yaw = !Yaw;
         //   }
         //   if (InputMgr.IsNewKey(Keys.D2) || InputMgr.IsNewKey(Keys.NumPad2))
         //   {
         //      Pitch = !Pitch;
         //   }
         //   if (InputMgr.IsNewKey(Keys.D3) || InputMgr.IsNewKey(Keys.NumPad3))
         //   {
         //      Roll = !Roll;
         //   }
         //}
         //WorldToRecompute = WorldToRecompute || Yaw || Pitch || Roll;
      }
   }
}