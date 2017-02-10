using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
   public class Background : Microsoft.Xna.Framework.DrawableGameComponent
   {
      protected const float BACKGROUND = 1f;
      protected SpriteBatch SpriteMgr { get; private set; }
      ResourcesManager<Texture2D> TextureMgr { get; set; }
      string ImageName { get; set; }
      Rectangle DisplayZone { get; set; }
      protected Texture2D BackgroundImage { get; private set; }

      public Background(Game game, string imageName)
         : base(game)
      {
         ImageName = imageName;
      }

      public override void Initialize()
      {
         DisplayZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
         base.Initialize();
      }

      protected override void LoadContent()
      {
         SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         TextureMgr = Game.Services.GetService(typeof(ResourcesManager<Texture2D>)) as ResourcesManager<Texture2D>;
         BackgroundImage = TextureMgr.Find(ImageName);
      }

      public override void Draw(GameTime gameTime)
      {
         SpriteMgr.Begin();
         Display(gameTime);
         SpriteMgr.End();
      }

      protected virtual void Display(GameTime gameTime)
      {
         SpriteMgr.Draw(BackgroundImage, DisplayZone, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, BACKGROUND);
      }
   }
}