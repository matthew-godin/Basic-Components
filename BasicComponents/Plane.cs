/*
Plane.cs
-------

By Matthew Godin

Role : Component displaying
       a plane made of
       triangles to the screen

Created : 21 November 2016
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
    public abstract class Plane : AnimatedBasicPrimitive
   {
      protected const int NUM_TRIANGLES = 2, HALF_DIVISOR = 2, NULLL_SIDE = 0, NUM_TRIANGLES_PER_SQUARE = 2, FIRST_VERTICES_OF_STRIP = 2, ADDITIONAL_VERTEX_FOR_LINE = 1;
      protected Vector3 Origin { get; private set; }  //The bottom left corner of the plane keeping in mind that the primitive is centered relative to the point  (0,0,0)
      Vector2 Delta { get; set; } // a vector containing the space between two columns (in X) and two rows (in Y)
      protected Vector3[,] VertexPts { get; private set; } //an array containing the different vertices of the plane
      protected int NumColumns { get; private set; } // Guess what it is...
      protected int NumRows { get; private set; } // idem 
      protected int NumTrianglesPerStrip { get; private set; } //...
      protected BasicEffect BscEffect { get; private set; } // 

        /// <summary>
        /// Constructor of the class Plane
        /// </summary>
        /// <param name="game">Contains the class GameProject</param>
        /// <param name="initialScale">Initial scale</param>
        /// <param name="initialRotation">Initial yaw, pitch, roll</param>
        /// <param name="initialPosition">Initial position</param>
        /// <param name="span">Width and height of the plane</param>
        /// <param name="dimensions">Number of rectangles in x and y</param>
        /// <param name="updateInterval">Update interval</param>
        public Plane(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, Vector2 dimensions, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, updateInterval)
      {
         NumColumns = (int)dimensions.X;
         NumRows = (int)dimensions.Y;
         Delta = span / dimensions;
         Origin = new Vector3(-span.X / HALF_DIVISOR, -span.Y / HALF_DIVISOR, NULLL_SIDE);
      }

        /// <summary>
        /// Initializes the plane
        /// </summary>
      public override void Initialize()
      {
         NumTrianglesPerStrip = NumColumns * NUM_TRIANGLES_PER_SQUARE;
         NumVertices = (NumTrianglesPerStrip + FIRST_VERTICES_OF_STRIP) * NumRows;
         VertexPts = new Vector3[NumColumns + ADDITIONAL_VERTEX_FOR_LINE, NumRows + ADDITIONAL_VERTEX_FOR_LINE];
         CreateVerticesArray();
         CreatePointArray();
         base.Initialize();
      }

      protected abstract void CreateVerticesArray();

        /// <summary>
        /// Loads the content
        /// </summary>
      protected override void LoadContent()
      {
         BscEffect = new BasicEffect(GraphicsDevice);
         InitializeBscEffectParameters();
         base.LoadContent();
      }

      protected abstract void InitializeBscEffectParameters();

        /// <summary>
        /// Puts the values in the array VertexPts
        /// </summary>
      private void CreatePointArray()
      {
            for (int i = 0; i < VertexPts.GetLength(0); ++i)
            {
                for (int j = 0; j < VertexPts.GetLength(1); ++j)
                {
                    VertexPts[i, j] = Origin + new Vector3(Delta, NULLL_SIDE) * new Vector3(i, j, NULLL_SIDE);
                }
            }
      }

        /// <summary>
        /// Draws the plane on the screen
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
      public override void Draw(GameTime gameTime)
      {
         BscEffect.World = GetWorld();
         BscEffect.View = GameCamera.View;
         BscEffect.Projection = GameCamera.Projection;
         foreach (EffectPass passEffect in BscEffect.CurrentTechnique.Passes)
         {
            passEffect.Apply();
            for (int i = 0 ; i < NumRows ; ++i)
            {
                    DrawTriangleStrip(i);
            }
            // Here, there should be a loop drawing each TriangleStrip of the plane
            // The drawing of a TriangleStrip in particular should be done by calling the method DrawTriangleStrip()
         }
         //base.Draw(gameTime);
      }

      protected abstract void DrawTriangleStrip(int stripIndex);
   }
}
