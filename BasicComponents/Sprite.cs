using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
   public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
   {
      string ImageName { get; set; }
      protected Vector2 Position { get; set; }        // Needed for dynamic sprite specialization
      protected Texture2D Image { get; private set; } // Needed for animated sprite specialization
      protected SpriteBatch SpriteMgr { get; private set; } // Needed for animated sprite specialization
      RessourcesManager<Texture2D> TextureMgr { get; set; }

      public Sprite(Game game, string imageName, Vector2 position)
         : base(game)
      {
         ImageName = imageName;
         Position = position;
      }

      protected override void LoadContent()
      {
         SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
         TextureMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         Image = TextureMgr.Find(ImageName);
      }

      public override void Draw(GameTime gameTime)
      {
         SpriteMgr.Draw(Image, Position, Color.White);
      }
   }
}