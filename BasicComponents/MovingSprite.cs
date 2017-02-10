using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
   public class MovingSprite : Sprite
   {
      const int NUM_DISPLACEMENT_PIXELS = 2;
      const float ACCELERATION_FACTOR = 1f / 600f;
      const float MIN_INTERVAL = 0.01f;
      float IntervalleMax { get; set; }
      float UpdateInterval { get; set; }
      float TimeElapsedSinceUpdate { get; set; }
      InputManager InputMgr { get; set; }
      protected int LeftMargin { get; set; }
      protected int RightMargin { get; set; }
      protected int TopMargin { get; set; }
      protected int BottomMargin { get; set; }


      public MovingSprite(Game game, string nameSprite, Vector2 startingPosition, float updateInterval)
         : base(game, nameSprite, ValidatePosition(startingPosition, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height))
      {
         UpdateInterval = updateInterval;
      }

      static Vector2 ValidatePosition(Vector2 position, int largeurÉcran, int heightÉcran)
      {
         float posX = MathHelper.Max(MathHelper.Min(position.X, largeurÉcran), 0);
         float posY = MathHelper.Max(MathHelper.Min(position.Y, heightÉcran), 0);
         return new Vector2(posX, posY);
      }

      public override void Initialize()
      {
         TimeElapsedSinceUpdate = 0;
         LeftMargin = 0;
         TopMargin = 0;
         IntervalleMax = UpdateInterval;
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
         ComputeMargins();
         AdjustPosition(0, 0);
      }

      protected virtual void ComputeMargins()
      {
         RightMargin = Game.Window.ClientBounds.Width - Image.Width;
         BottomMargin = Game.Window.ClientBounds.Height - Image.Height;
      }

      public override void Update(GameTime gameTime)
      {
         float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TimeElapsedSinceUpdate += timeElapsed;
         if (TimeElapsedSinceUpdate >= UpdateInterval)
         {
            PerformUpdate();
            TimeElapsedSinceUpdate = 0;
         }
      }

      protected virtual void PerformUpdate()
      {
         ManageKeyboard();
      }

      void ManageKeyboard()
      {
         if (InputMgr.IsKeyboardActivated)
         {

            int horizontalDisplacement = ManageKey(Keys.D) - ManageKey(Keys.A);
            int verticalDisplacement = ManageKey(Keys.S) - ManageKey(Keys.W);
            ManageAcceleration();
            if (horizontalDisplacement != 0 || verticalDisplacement != 0)
            {
               AdjustPosition(horizontalDisplacement, verticalDisplacement);
            }
         }
      }

      int ManageKey(Keys key)
      {
         return InputMgr.IsPressed(key) ? NUM_DISPLACEMENT_PIXELS : 0;
      }

      void ManageAcceleration()
      {
         int accelerationModification = ManageKey(Keys.PageDown) - ManageKey(Keys.PageUp);
         if (accelerationModification != 0)
         {
            UpdateInterval += accelerationModification * ACCELERATION_FACTOR;
            UpdateInterval = MathHelper.Max(MathHelper.Min(UpdateInterval, IntervalleMax), MIN_INTERVAL);

         }
      }

      void AdjustPosition(int horizontalDisplacement, int verticalDisplacement)
      {
         float posX = ComputePosition(horizontalDisplacement, Position.X, LeftMargin, RightMargin);
         float posY = ComputePosition(verticalDisplacement, Position.Y, TopMargin, BottomMargin);
         Position = new Vector2(posX, posY);
      }

      float ComputePosition(int displacement, float currentPosition, int MinThreshold, int MaxThreshold)
      {
         float position = currentPosition + displacement;
         return MathHelper.Min(MathHelper.Max(MinThreshold, position), MaxThreshold);
      }
   }
}