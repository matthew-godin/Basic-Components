using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace XNAProject
{
   public class Displayer3D : Microsoft.Xna.Framework.DrawableGameComponent
   {
      InputManager InputMgr { get; set; }
      public DepthStencilState GameBufferState { get; private set; }
      public RasterizerState GameRasterizerState { get; private set; }
      public BlendState GameBlendState { get; private set; }


      bool isDisplayedInWireframes;
      bool IsDisplayedInWireframes
      {
         get { return isDisplayedInWireframes; }
         set
         {
            GameRasterizerState = new RasterizerState();
            isDisplayedInWireframes = value;
            GameRasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GameRasterizerState.FillMode = isDisplayedInWireframes ? FillMode.WireFrame : FillMode.Solid;
            Game.GraphicsDevice.RasterizerState = GameRasterizerState;
         }
      }

      public Displayer3D(Game game)
         : base(game)
      { }

      public override void Initialize()
      {
         GameBufferState = new DepthStencilState();
         GameBufferState.DepthBufferEnable = true;
         GameRasterizerState = new RasterizerState();
         GameRasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
         GameBlendState = BlendState.NonPremultiplied;
         base.Initialize();
      }

      protected override void LoadContent()
      {
         InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
         base.LoadContent();
      }

      //public override void Update(GameTime gameTime)
      //{
      //   ManageKeyboard();
      //   base.Update(gameTime);
      //}

      public override void Draw(GameTime gameTime)
      {
         GraphicsDevice.DepthStencilState = GameBufferState;
         GraphicsDevice.RasterizerState = GameRasterizerState;
         GraphicsDevice.BlendState = GameBlendState;
         GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
         base.Draw(gameTime);
      }

      //void ManageKeyboard()
      //{
      //   if (InputMgr.IsNewKey(Keys.F))
      //   {
      //      IsDisplayedInWireframes = !IsDisplayedInWireframes;
      //   }
      //}
   }
}