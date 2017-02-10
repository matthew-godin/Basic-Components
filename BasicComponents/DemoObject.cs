/*
DemoObject.cs
--------------

By Matthew Godin

Role : Component testing a 3D model
       by changing its scale
       and rotating it along different axes

Created : 2 November 2016
*/
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
    /// <summary>
    /// Component testing a 3D model by changing its scale and rotating it along different axes
    /// </summary>
    public class DemoObject : BaseObject
    {
        const float NO_TIME_ELAPSED = 0.0F, SCALE_CHANGE = 0.001F, MAXIMAL_SCALE = 1.0F, MINIMAL_SCALE = 0.005F, ANGLE_INCREMENT = (float)Math.PI / 120, NO_ROTATION = 0.0F;

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        InputManager InputMgr { get; set; }
        bool RotationYActivated { get; set; }
        bool RotationXActivated { get; set; }
        bool RotationZActivated { get; set; }
        Vector3 RotationIncrementY { get; set; }
        Vector3 RotationIncrementX { get; set; }
        Vector3 RotationIncrementZ { get; set; }

        /// <summary>
        /// Returns initial angles vector
        /// </summary>
        Vector3 NormalRotationAngles
        {
            get
            {
                return new Vector3(NO_ROTATION, MathHelper.PiOver2, NO_ROTATION);
            }
        }

        /// <summary>
        /// Constructor of the class DemoObject
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="modelName">String representing the model file location</param>
        /// <param name="initialScale">Initial scale</param>
        /// <param name="initialRotation">Initial rotation</param>
        /// <param name="initialPosition">Initial position</param>
        /// <param name="updateInterval">Update interval</param>
        public DemoObject(Game game, String modelName, float initialScale, Vector3 initialRotation, Vector3 initialPosition, float updateInterval) : base(game, modelName, initialScale, initialRotation, initialPosition)
        {
            UpdateInterval = updateInterval;
        }

        /// <summary>
        /// Initializes DemoObject
        /// </summary>
        public override void Initialize()
        {
            TimeElapsedSinceUpdate = NO_TIME_ELAPSED;
            RotationYActivated = false;
            RotationXActivated = false;
            RotationZActivated = false;
            RotationIncrementY = new Vector3(NO_ROTATION, ANGLE_INCREMENT, NO_ROTATION);
            RotationIncrementX = new Vector3(ANGLE_INCREMENT, NO_ROTATION, NO_ROTATION);
            RotationIncrementZ = new Vector3(NO_ROTATION, NO_ROTATION, ANGLE_INCREMENT);
            base.Initialize();
        }

        /// <summary>
        /// Loads the content
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        /// <summary>
        /// Updates the component
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            TimeElapsedSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                TimeElapsedSinceUpdate = NO_TIME_ELAPSED;
                CheckIfKeyboardIsUsed();
                UpdateAngles();
                UpdateWorldMatrix();
            }
            CheckCurrentKeys();
        }

        /// <summary>
        /// Updates the world matrix
        /// </summary>
        void UpdateWorldMatrix()
        {
            World = Matrix.Identity;
            World *= Matrix.CreateScale(Scale);
            World *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            World *= Matrix.CreateTranslation(Position);
        }

        /// <summary>
        /// Calls the different methods for keys being pressed
        /// </summary>
        void CheckCurrentKeys()
        {
            CheckRotations();
            CheckReestablishAngles();
        }

        /// <summary>
        /// Calls the different methods for activated rotations
        /// </summary>
        void UpdateAngles()
        {
            CheckYRotation();
            CheckXRotation();
            CheckZRotation();
        }

        /// <summary>
        /// Verify if the Y rotation is activated and increments by Pi over 120 if it is the case
        /// </summary>
        void CheckYRotation()
        {
            if (RotationYActivated)
            {
                Rotation += RotationIncrementY;
            }
        }

        /// <summary>
        /// Verify if the X rotation is activated and increments by Pi over 120 if it is the case
        /// </summary>
        void CheckXRotation()
        {
            if (RotationXActivated)
            {
                Rotation += RotationIncrementX;
            }
        }

        /// <summary>
        /// Verify if the Z rotation is activated and increments by Pi over 120 if it is the case
        /// </summary>
        void CheckZRotation()
        {
            if (RotationZActivated)
            {
                Rotation += RotationIncrementZ;
            }
        }

        /// <summary>
        /// Checks if at least one key is pressed
        /// </summary>
        void CheckIfKeyboardIsUsed()
        {
            if (InputMgr.IsKeyboardActivated)
            {
                CheckScaleUp();
                CheckScaleDown();
            }
        }

        /// <summary>
        /// Calls the rotation verification methods
        /// </summary>
        void CheckRotations()
        {
            VerifyRotationY();
            VerifyRotationX();
            VerifyRotationZ();
        }

        /// <summary>
        /// Checks if key 1 is a new key and activates the Y rotation if that is the case
        /// </summary>
        void VerifyRotationY()
        {
            if (InputMgr.IsNewKey(Keys.NumPad1) || InputMgr.IsNewKey(Keys.D1))
            {
                RotationYActivated = !RotationYActivated;
            }
        }

        /// <summary>
        /// Checks if key 2 is a new key and activates the X rotation if that is the case
        /// </summary>
        void VerifyRotationX()
        {
            if (InputMgr.IsNewKey(Keys.NumPad2) || InputMgr.IsNewKey(Keys.D2))
            {
                RotationXActivated = !RotationXActivated;
            }
        }

        /// <summary>
        /// Checks if key 3 is a new key and activates the Z rotation if that is the case
        /// </summary>
        void VerifyRotationZ()
        {
            if (InputMgr.IsNewKey(Keys.NumPad3) || InputMgr.IsNewKey(Keys.D3))
            {
                RotationZActivated = !RotationZActivated;
            }
        }

        /// <summary>
        /// Checks if key Space du clavier numérique ou alphabétique est une nouvelle key and remet les angles de rotations initiaux rotation if that is the case
        /// </summary>
        void CheckReestablishAngles()
        {
            if (InputMgr.IsNewKey(Keys.Space))
            {
                Rotation = NormalRotationAngles;
            }
        }

        /// <summary>
        /// Checks if key Plus est enfoncée and l'échelle n'est pas encore maximale and incrémente l'échelle rotation if that is the case
        /// </summary>
        void CheckScaleUp()
        {
            if (InputMgr.IsPressed(Keys.OemPlus) && Scale < MAXIMAL_SCALE)
            {
                Scale += SCALE_CHANGE;
            }
        }

        /// <summary>
        /// Checks if key Dash est enfoncée and l'échelle n'est pas encore minimale and décrémente l'échelle rotation if that is the case
        /// </summary>
        void CheckScaleDown()
        {
            if (InputMgr.IsPressed(Keys.OemMinus) && Scale > MINIMAL_SCALE)
            {
                Scale -= SCALE_CHANGE;
            }
        }
    }
}
