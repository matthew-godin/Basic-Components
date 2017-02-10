using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
   public class MovingBackground : Background
   {
      const float HORIZONTAL_DISPLACEMENT = 0.2f;
      float IntervalMAJ { get; set; }
      float TimeElpasedSinceUpdate { get; set; }
      protected float Scale { get; private set; }
      protected Vector2 ScreenPosition { get; set; }
      protected Vector2 ImageSize { get; set; }

      public MovingBackground(Game game, string imageName, float intervalMAJ)
         : base(game, imageName)
      {
         IntervalMAJ = intervalMAJ;
      }

      public override void Initialize()
      {
         TimeElpasedSinceUpdate = 0;
         base.Initialize();
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         ScreenPosition = new Vector2(Game.Window.ClientBounds.Width / 2, 0);
         Scale = MathHelper.Max(Game.Window.ClientBounds.Width / (float)BackgroundImage.Width,
                                  Game.Window.ClientBounds.Height / (float)BackgroundImage.Height);
         ImageSize = new Vector2(BackgroundImage.Width * Scale, 0);
      }
      public override void Update(GameTime gameTime)
      {
         float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TimeElpasedSinceUpdate += timeElapsed;
         if (TimeElpasedSinceUpdate >= IntervalMAJ)
         {
            PerformUpdate();
            TimeElpasedSinceUpdate = 0;
         }
      }

      protected virtual void PerformUpdate()
      {
         ScreenPosition = new Vector2((ScreenPosition.X + HORIZONTAL_DISPLACEMENT) % ImageSize.X, 0);
      }

      protected override void Afficher(GameTime gameTime)
      {
         SpriteMgr.Draw(BackgroundImage, ScreenPosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, BACKGROUND);
         SpriteMgr.Draw(BackgroundImage, ScreenPosition - ImageSize, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, BACKGROUND);
      }
   }
}