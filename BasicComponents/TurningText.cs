/*
TurningText.cs
------------------

By Matthew Godin

Role : Component showing
       a turning message
       made of a string of characters
       representing this message, its position,
       its color and an update interval
       representing also the speed
       at which the message turns 

Created : 29 August 2016
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace XNAProject
{
    /// <summary>
    /// Component displaying un texte tournoyant personnalise
    /// </summary>
    public class TurningText : Microsoft.Xna.Framework.DrawableGameComponent
    {
        /// <summary>
        /// Constructor saving the parameters passed
        /// </summary>
        /// <param name="game">Game that called this component</param>
        /// <param name="message">Message to display</param>
        /// <param name="position">Position of the message relative to its center</param>
        /// <param name="color">Color wanted for revolving message</param>
        /// <param name="updateInterval">Update interval, a large value will make the message grow faster</param>
        public TurningText(Game game, string message, Vector2 position, Color color, float updateInterval) : base(game)
        {
            Message = message;
            Position = position;
            Color = color;
            UpdateInterval = updateInterval;
        }

        /// <summary>
        /// Initializes the time, angle and scale mechanics of the turning text
        /// </summary>
        public override void Initialize()
        {
            TimeElpasedSinceUpdate = NO_TIME_ELAPSED;
            Scale = STARTING_SCALE;
            AngleRotation = STARTING_ANGLE;
            base.Initialize();
        }

        /// <summary>
        /// Loads the font and the sprite manager and makes the computation of the turning message origin
        /// </summary>
        protected override void LoadContent()
        {
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
            InitializeOrigin();
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Initializes the origin
        /// </summary>
        void InitializeOrigin()
        {
            Vector2 dimension = ArialFont.MeasureString(Message);
            Origin = new Vector2(dimension.X / 2, dimension.Y / 2);
        }

        /// <summary>
        /// Updates the angle and the scale of the turning message, and deactivates after two turns
        /// </summary>
        /// <param name="gameTime">Gives time information</param>
        public override void Update(GameTime gameTime)
        {
            TimeElpasedSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CheckIfIncrementationNecessary();
        }

        /// <summary>
        ///Verifies if the update interval has elapsed and to increment the angle and scale of the turning text if that is the case
        /// </summary>
        void CheckIfIncrementationNecessary()
        {
            if (TimeElpasedSinceUpdate >= UpdateInterval)
            {
                TimeElpasedSinceUpdate = NO_TIME_ELAPSED;
                AngleRotation += ANGLE_INCREMENT;
                Scale += ZOOM_INCREMENT;
                CheckIfDeactivationNecessary();
            }
        }

        /// <summary>
        /// Verifies if the turning text has made two turns and deactivates its turning, its gorwing and rotation if that is the case
        /// </summary>
        void CheckIfDeactivationNecessary()
        {
            if (AngleRotation > TWO_TURNS)
            {
                AngleRotation = TWO_TURNS;
                this.Enabled = false;
            }
        }

        /// <summary>
        /// Draws the turning message on the screen
        /// </summary>
        /// <param name="gameTime">Gives time information</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.DrawString(ArialFont, Message, Position, Color, MathHelper.ToRadians(AngleRotation), Origin, Scale, SpriteEffects.None, NO_DEPTH_LAYER);
        }

        string Message { get; set; }
        Vector2 Position { get; set; }
        Color Color { get; set; }
        float UpdateInterval { get; set; }
        SpriteFont ArialFont { get; set; }
        float Scale { get; set; }
        float AngleRotation { get; set; }
        Vector2 Origin { get; set; }
        SpriteBatch SpriteMgr { get; set; }
        float TimeElpasedSinceUpdate { get; set; }

        const float ANGLE_INCREMENT = 3.6F;
        const float ZOOM_INCREMENT = 0.01F;
        const float TWO_TURNS = 720.0F;
        const float STARTING_ANGLE = 0.0F;
        const float STARTING_SCALE = 0.01F;
        const float NO_TIME_ELAPSED = 0.0F;
        const float NO_DEPTH_LAYER = 0.0F;
    }
}
