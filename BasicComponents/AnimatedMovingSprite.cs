/*
AnimatedMovingSprite.cs
--------------------

By Matthew Godin

Role : Component inheriting from MovingSprite that 
       that animates its sprite 
       by showing different frames

Created : 1 October 2016
*/
using Microsoft.Xna.Framework;

namespace XNAProject
{
    /// <summary>
    /// Component displaying an animated sprite showing different frames coming from the same image
    /// </summary>
    public class AnimatedMovingSprite : MovingSprite
   {
      /// <summary>
      /// Enum representing possible directions the sprite can take
      /// </summary>
      enum SpriteDirection
      {
           LEFT, DOWN, RIGHT, UP
      }

      const float NO_DISPLACEMENT = 0.0F;
      const int ORIGIN = 0;
      Vector2 NumImages {get; set;}
      Vector2 Delta { get; set; }
      Rectangle SourceRectangle { get; set; }
      Vector2 PreviousPosition { get; set; }
      Vector2 Displacement { get; set; }
      Vector2 DisplacementNull { get; set; }

      /// <summary>
      /// AnimatedMovingSprite's constructor
      /// </summary>
      /// <param name="game">Game object</param>
      /// <param name="nameSprite">Sprite file name</param>
      /// <param name="numImages">Vector2 containing the number of images in height and width</param>
      /// <param name="startingPosition">Sprite starting position</param>
      /// <param name="updateInterval">Sprite update interval</param>
      public AnimatedMovingSprite(Game game, string nameSprite, Vector2 numImages, Vector2 startingPosition, float updateInterval) : base(game, nameSprite, startingPosition, updateInterval)
      {
            NumImages = new Vector2(numImages.X, numImages.Y);
      }

      /// <summary>
      /// LoadContent method for AnimatedMovingSprite
      /// </summary>
      protected override void LoadContent()
      {
         base.LoadContent();
         SourceRectangle = new Rectangle(ORIGIN, (int)SpriteDirection.RIGHT * (int)Delta.Y, (int)Delta.X, (int)Delta.Y);
         DisplacementNull = new Vector2(NO_DISPLACEMENT, NO_DISPLACEMENT);
      }

      protected override void ComputeMargins()
      {
         Delta = new Vector2(Image.Width, Image.Height) / NumImages;
         RightMargin = Game.Window.ClientBounds.Width - (int)Delta.X;
         BottomMargin = Game.Window.ClientBounds.Height - (int)Delta.Y;
      }

      /// <summary>
      /// Method updating the AnimatedMovingSprite accoridng to time elapsed and displacement caused by WASD keys pressed
      /// </summary>
      protected override void PerformUpdate()
      {
         PreviousPosition = new Vector2(Position.X, Position.Y);
         base.PerformUpdate();
         Displacement = Position - PreviousPosition;
         if (Displacement != DisplacementNull)
         {
            SourceRectangle = new Rectangle((SourceRectangle.X + (int)Delta.X) % Image.Width, Displacement.Y > NO_DISPLACEMENT ? (int)SpriteDirection.DOWN * (int)Delta.Y : (Displacement.Y < NO_DISPLACEMENT ? (int)SpriteDirection.UP * (int)Delta.Y : (Displacement.X > NO_DISPLACEMENT ? (int)SpriteDirection.RIGHT * (int)Delta.Y : (int)SpriteDirection.LEFT * (int)Delta.Y)), (int)Delta.X, (int)Delta.Y);
         }
      }

      /// <summary>
      /// Method drawing the AnimatedMovingSprite to the screen
      /// </summary>
      /// <param name="gameTime">Contains time information</param>
      public override void Draw(GameTime gameTime)
      {
         SpriteMgr.Draw(Image, Position, SourceRectangle, Color.White);
      }
   }
}
