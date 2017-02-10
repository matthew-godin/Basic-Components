using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
   public class FPSDisplay : Microsoft.Xna.Framework.DrawableGameComponent
   {
      const int BOTTOM_MARGIN = 10;
      const int RIGHT_MARGIN = 15;
      const float NO_ROTATION = 0f;
      const float NO_SCALE = 1f;
      const float FOREGROUND = 0f;

      float UpdateInterval { get; set; }
      float TimeElapsedSinceUpdate { get; set; }
      int CptFrames { get; set; }
      float ValFPS { get; set; }

      string FPSString { get; set; }
      Vector2 RightBottomPosition { get; set; }
      Vector2 StringPosition { get; set; }
      Vector2 Dimension { get; set; }

      SpriteBatch SpriteMgr { get; set; }
      ResourcesManager<SpriteFont> FontsMgr { get; set; }
      string FontName { get; set; }
      SpriteFont characterFont { get; set; }
      Color FPSColor { get; set; }

      public FPSDisplay(Game game, string fontName, Color colorFPS, float updateInterval)
         : base(game)
      {
         FontName = fontName;
         FPSColor = colorFPS;
         UpdateInterval = updateInterval;
      }

      public override void Initialize()
      {
         TimeElapsedSinceUpdate = 0;
         ValFPS = 0;
         CptFrames = 0;
         FPSString = "";
         RightBottomPosition = new Vector2(Game.Window.ClientBounds.Width - RIGHT_MARGIN,
                                         Game.Window.ClientBounds.Height - BOTTOM_MARGIN);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         //SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         SpriteMgr = new SpriteBatch(Game.GraphicsDevice);
         FontsMgr = Game.Services.GetService(typeof(ResourcesManager<SpriteFont>)) as ResourcesManager<SpriteFont>;
         characterFont = FontsMgr.Find(FontName); 
      }

      public override void Update(GameTime gameTime)
      {
         float TimeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
         ++CptFrames;
         TimeElapsedSinceUpdate += TimeElapsed;
         if (TimeElapsedSinceUpdate >= UpdateInterval)
         {
            ComputeFPS();
            TimeElapsedSinceUpdate = 0;
         }
      }

      void ComputeFPS()
      {
         float oldValFPS = ValFPS;
         ValFPS = CptFrames / TimeElapsedSinceUpdate;
         if (oldValFPS != ValFPS)
         {
            FPSString = ValFPS.ToString("0");
            Dimension = characterFont.MeasureString(FPSString);
            StringPosition = RightBottomPosition - Dimension;
         }
         CptFrames = 0;
      }

      public override void Draw(GameTime gameTime)
      {
         SpriteMgr.Begin();
         SpriteMgr.DrawString(characterFont, FPSString, StringPosition, FPSColor, NO_ROTATION,
                                   Vector2.Zero, NO_SCALE, SpriteEffects.None, FOREGROUND);
         SpriteMgr.End();
      }
   }
}