/*
SubjectiveCamera.cs
-------------------

By Marc-Olivier Fillion and Matthew Godin

Role : Camera capable of moving and turning according to 
       all degrees of freedom

Created : 7 December 2016
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
   public class SubjectiveCamera : Camera
   {
      const float STANDARD_UPDATE_INTERVAL = 1f / 60f;
      const float ACCELERATION = 0.001f;
      const float INITIAL_ROTATION_SPEED = 5f;
        const float INITIAL_TRANSLATION_SPEED = 0.5f;
      const float DELTA_YAW = MathHelper.Pi / 180; // 1 degree at a time
      const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degree at a time
      const float DELTA_ROLL = MathHelper.Pi / 180; // 1 degree at a time
      const float COLLISION_RADIUS = 1f;

      Vector3 Direction { get; set; }
      Vector3 Lateral { get; set; }
      float TranslationSpeed { get; set; }
      float RotationSpeed { get; set; }

      float UpdateInterval { get; set; }
      float TimeElapsedSinceUpdate { get; set; }
      InputManager InputMgr { get; set; }

      bool inZoom;
      bool InZoom
      {
         get { return inZoom; }
         set
         {
            float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            inZoom = value;
            if (inZoom)
            {
               CreateViewingFrustum(OBJECTIVE_OPENNES / 2, aspectRatio, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
            }
            else
            {
               CreateViewingFrustum(OBJECTIVE_OPENNES, aspectRatio, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
            }
         }
      }

      public SubjectiveCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation, float updateInterval) : base(game)
      {
         UpdateInterval = updateInterval;
         CreateViewingFrustum(OBJECTIVE_OPENNES, NEAR_PLANE_DISTANCE, FAR_PLANE_DISTANCE);
         CreateLookAt(cameraPosition, target, orientation);
         InZoom = false;
      }

      public override void Initialize()
      {
         RotationSpeed = INITIAL_ROTATION_SPEED;
         TranslationSpeed = INITIAL_TRANSLATION_SPEED;
         TimeElapsedSinceUpdate = 0;
         base.Initialize();
         InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
      }

      protected override void CreateLookAt()
      {
            // Method called if it's necessary to recompute the view matrix.
            // Computation and normalization of certain vectors
            // TODO
            Lateral = Vector3.Cross(Direction, GreenicalOrientation);
            Lateral = Vector3.Normalize(Lateral);
            GreenicalOrientation = Vector3.Cross(Lateral, Direction);
            GreenicalOrientation = Vector3.Normalize(GreenicalOrientation);
            View = Matrix.CreateLookAt(Position, Position + Direction, GreenicalOrientation);
         GenerateFrustum();
      }

      protected override void CreateLookAt(Vector3 position, Vector3 target, Vector3 orientation)
      {
            // On construction, initialization of Position, Target, and GreenicalOrientation properties
            // as well as the computation of the Direction, Lateral and GreenicalOrientation vectors
            // allowing the compute the view matrix of the subjective camera
            // TODO
            Position = position;
            GreenicalOrientation = orientation;
            Direction = target - position;
            Direction = Vector3.Normalize(Direction);
         //Creation of the view matrix (look at)
         CreateLookAt();
      }

      public override void Update(GameTime gameTime)
      {
         float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TimeElapsedSinceUpdate += timeElapsed;
         ManageKeyboard();
         if (TimeElapsedSinceUpdate >= UpdateInterval)
         {
            if (InputMgr.IsPressed(Keys.LeftShift) || InputMgr.IsPressed(Keys.RightShift))
            {
               ManageAcceleration();
               ManageDisplacement();
               ManageRotation();
               CreateLookAt();
            }
            TimeElapsedSinceUpdate = 0;
         }
         base.Update(gameTime);
      }

      private int ManageKey(Keys key)
      {
         return InputMgr.IsPressed(key) ? 1 : 0;
      }

      private void ManageAcceleration()
      {
         int accelerationValue = (ManageKey(Keys.Subtract) + ManageKey(Keys.OemMinus)) - (ManageKey(Keys.Add)+ManageKey(Keys.OemPlus));
         if (accelerationValue != 0)
         {
            UpdateInterval += ACCELERATION * accelerationValue;
            UpdateInterval = MathHelper.Max(STANDARD_UPDATE_INTERVAL, UpdateInterval);
         }
      }

      private void ManageDisplacement()
      {
         Vector3 newPosition = Position;
         float displacementDirection = (ManageKey(Keys.W) - ManageKey(Keys.S)) * TranslationSpeed;
         float lateralDisplacement = (ManageKey(Keys.A) - ManageKey(Keys.D)) * TranslationSpeed;

            // Computation of the back forth displacement
            // Computation of the lateral displacement
            // TODO
            Position += Direction * displacementDirection;
            Position -= Lateral * lateralDisplacement;
      }

      private void ManageRotation()
      {
         ManageYaw();
         ManagePitch();
         ManageRoll();
      }

      private void ManageYaw()
      {
            // Yaw management
            // TODO
            if (InputMgr.IsPressed(Keys.Right))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(GreenicalOrientation, -DELTA_YAW * RotationSpeed));
                Direction = Vector3.Normalize(Direction);
            }
            else if (InputMgr.IsPressed(Keys.Left))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(GreenicalOrientation, DELTA_YAW * RotationSpeed));
                Direction = Vector3.Normalize(Direction);
            }
        }

      private void ManagePitch()
      {
            // Pitch management
            // TODO
            if (InputMgr.IsPressed(Keys.Up))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * RotationSpeed));
                GreenicalOrientation = Vector3.Transform(GreenicalOrientation, Matrix.CreateFromAxisAngle(Lateral, -DELTA_ROLL * RotationSpeed));
                Direction = Vector3.Normalize(Direction);
                GreenicalOrientation = Vector3.Normalize(GreenicalOrientation);
            }
            else if (InputMgr.IsPressed(Keys.Down))
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * RotationSpeed));
                GreenicalOrientation = Vector3.Transform(GreenicalOrientation, Matrix.CreateFromAxisAngle(Lateral, DELTA_ROLL * RotationSpeed));
                Direction = Vector3.Normalize(Direction);
                GreenicalOrientation = Vector3.Normalize(GreenicalOrientation);
            }
        }

      private void ManageRoll()
      {
            // Roll management
            // TODO
            if (InputMgr.IsPressed(Keys.PageUp))
            {
                GreenicalOrientation = Vector3.Transform(GreenicalOrientation, Matrix.CreateFromAxisAngle(Direction, -DELTA_ROLL * RotationSpeed));
                GreenicalOrientation = Vector3.Normalize(GreenicalOrientation);
            }
            else if (InputMgr.IsPressed(Keys.PageDown))
            {
                GreenicalOrientation = Vector3.Transform(GreenicalOrientation, Matrix.CreateFromAxisAngle(Direction, DELTA_ROLL * RotationSpeed));
                GreenicalOrientation = Vector3.Normalize(GreenicalOrientation);
            }
        }

      private void ManageKeyboard()
      {
         if (InputMgr.IsNewKey(Keys.Z))
         {
            InZoom = !InZoom;
         }
      }
   }
}
