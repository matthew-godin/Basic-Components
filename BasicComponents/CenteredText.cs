//
// Author : Vincent Echelard
// Date : Creation - September 2014
//        Modification - November 2016
//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
   public class CenteredText : Microsoft.Xna.Framework.DrawableGameComponent
   {
      float DisplayableZonePercentage;
      string TextToDisplay { get; set; }
      string FontName { get; set; }
      Rectangle DisplayZone { get; set; }
      Vector2 DisplayPosition { get; set; }
      Color TextColor { get; set; }
      Vector2 Origin { get; set; }
      float Scale { get; set; }
      SpriteFont CharacterFont { get; set; }
      SpriteBatch SpriteMgr { get; set; }
      ResourcesManager<SpriteFont> FontsMgr { get; set; }

      public CenteredText(Game game, string textToDisplay, string fontName, Rectangle displayZone, 
                         Color textColor, float margin)
         : base(game)
      {
         TextToDisplay = textToDisplay;
         FontName = fontName;
         TextColor = textColor;
         DisplayZone = displayZone;
         DisplayableZonePercentage = 1f - margin;
         DisplayPosition = new Vector2(displayZone.X + displayZone.Width / 2,
                                         displayZone.Y + displayZone.Height / 2);
      }

      protected override void LoadContent()
      {
         SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         FontsMgr = Game.Services.GetService(typeof(ResourcesManager<SpriteFont>)) as ResourcesManager<SpriteFont>;
         CharacterFont = FontsMgr.Find(FontName);
         ModifyText(TextToDisplay);
      }

      public void ModifyText(string textToDisplay)
      {
         Vector2 dimensionText = CharacterFont.MeasureString(TextToDisplay);
         float horizontalScale = MathHelper.Max(MathHelper.Min(DisplayZone.Width * DisplayableZonePercentage, dimensionText.X),DisplayZone.Width * DisplayableZonePercentage) / dimensionText.X;
         float verticalScale = MathHelper.Max(MathHelper.Min(DisplayZone.Height * DisplayableZonePercentage, dimensionText.Y),DisplayZone.Height * DisplayableZonePercentage) / dimensionText.Y;
         Scale = MathHelper.Min(horizontalScale, verticalScale);
         Origin = dimensionText / 2;
      }

      public override void Draw(GameTime gameTime)
      {
         SpriteMgr.Begin();
         SpriteMgr.DrawString(CharacterFont, TextToDisplay, DisplayPosition, TextColor, 0, Origin, Scale, SpriteEffects.None, 0);
         SpriteMgr.End();
      }
   }
}