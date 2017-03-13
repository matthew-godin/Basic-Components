using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace XNAProject
{
   public class InputManager : Microsoft.Xna.Framework.GameComponent
   {
      Keys[] OldKeys { get; set; }
      Keys[] NewKeys { get; set; }
      KeyboardState KeyboardState { get; set; }
      MouseState OldMouseState { get; set; }
      MouseState NewMouseState { get; set; }

      public InputManager(Game game)
         : base(game)
      { }

      public override void Initialize()
      {
         NewKeys = new Keys[0];
         OldKeys = NewKeys;
         NewMouseState = Mouse.GetState();
         OldMouseState = NewMouseState;
         base.Initialize();
      }

      public override void Update(GameTime gameTime)
      {
         OldKeys = NewKeys;
         KeyboardState = Keyboard.GetState();
         NewKeys = KeyboardState.GetPressedKeys();
         UpdateMouseState();
      }

      public bool IsKeyboardActivated
      {
         get { return NewKeys.Length > 0; }
      }

      public bool IsPressed(Keys key)
      {
         return KeyboardState.IsKeyDown(key);
      }

      public bool IsNewKey(Keys key)
      {
         int numKeys = OldKeys.Length;
         bool isNewKey = KeyboardState.IsKeyDown(key);
         int i = 0;
         while (i < numKeys && isNewKey)
         {
            isNewKey = OldKeys[i] != key;
            ++i;
         }
         return isNewKey;
      }

      void UpdateMouseState()
      {
         OldMouseState = NewMouseState;
         NewMouseState = Mouse.GetState();
      }

      public bool IsOldRightClick()
      {
         return NewMouseState.RightButton == ButtonState.Pressed && 
                OldMouseState.RightButton == ButtonState.Pressed;
      }

      public bool IsOldLeftClick()
      {
         return NewMouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Pressed;
      }

      public bool IsNewRightClick()
      {
         return NewMouseState.RightButton == ButtonState.Pressed && OldMouseState.RightButton == ButtonState.Released;
      }

      public bool IsNewLeftClick()
      {
         return NewMouseState.LeftButton == ButtonState.Pressed && 
                OldMouseState.LeftButton == ButtonState.Released;
      }

      public Point GetMousePosition()
      {
         return new Point(NewMouseState.X, NewMouseState.Y);
      }
   }
}