using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
   public class NightSkyBackground : MovingBackground
   {
      const float VERTICAL_DISPLACEMENT = 0.2f;

      public NightSkyBackground(Game game, string imageName, float intervalMAJ)
         : base(game, imageName, intervalMAJ)
      { }

      protected override void LoadContent()
      {
         base.LoadContent();
         ImageSize = new Vector2(0, BackgroundImage.Height * Scale);
      }

      protected override void PerformUpdate()
      {
         ScreenPosition = new Vector2(0, (ScreenPosition.Y + VERTICAL_DISPLACEMENT) % ImageSize.Y);
      }

      protected override void Display(GameTime gameTime)
      {
         SpriteMgr.Draw(BackgroundImage, ScreenPosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, ARRIÈRE_PLAN);
         SpriteMgr.Draw(BackgroundImage, ScreenPosition - ImageSize, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, ARRIÈRE_PLAN);
      }
   }
}