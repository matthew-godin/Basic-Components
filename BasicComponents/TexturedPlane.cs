/*
TexturedPlane.cs
--------------

By Matthew Godin

Role : Component displaying
       un plan texturé composé de bandes 
       stripes to the screen

Created : 21 November 2016
*/
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
    public class TexturedPlane : Plane
    {
        const int NO_VERTEX_OFFSET = 0, BEFORE_FIRST_VERTEX = -1;

        VertexPositionTexture[] Vertices { get; set; }
        string TextureName { get; set; }
        Vector2[,] TexturePts { get; set; }
        ResourceManager<Texture2D> TextureMgr { get; set; }
        BlendState AlphaMgr { get; set; }
        Texture2D texturePlane { get; set; }

        /// <summary>
        /// Constructor of the class TexturedPlane
        /// </summary>
        /// <param name="game">Contains the class GameProject</param>
        /// <param name="initialScale">Initial scale</param>
        /// <param name="initialRotation">Initial yaw, pitch, roll</param>
        /// <param name="initialPosition">Initial position</param>
        /// <param name="span">Width and height of the plane</param>
        /// <param name="dimensions">Number of rectangles in x and y</param>
        /// <param name="updateInterval">Update interval</param>
        public TexturedPlane(Game game, float initialScale, Vector3 initialRotation, Vector3 initialPosition, Vector2 span, Vector2 dimensions, string textureName, float updateInterval) : base(game, initialScale, initialRotation, initialPosition, span, dimensions, updateInterval)
        {
            TextureName = textureName;
        }

        /// <summary>
        /// Creates arrays holding vertices
        /// </summary>
        protected override void CreateVertexArray()
        {
            TexturePts = new Vector2[NumColumns + ADDITIONAL_VERTEX_FOR_LINE, NumRows + ADDITIONAL_VERTEX_FOR_LINE];
            Vertices = new VertexPositionTexture[NumVertices];
            CreateTexturePointArray();
        }

        /// <summary>
        /// Populates TexturePts
        /// </summary>
        private void CreateTexturePointArray()
        {
            for (int i = 0 ; i < TexturePts.GetLength(0) ; ++i)
            {
                for (int j = 0 ; j < TexturePts.GetLength(1) ; ++j)
                {
                    TexturePts[i, j] = new Vector2(i / (float)NumColumns, -j / (float)NumRows);
                }
            }
        }

        /// <summary>
        /// Loads the content
        /// </summary>
        protected override void LoadContent()
        {
            TextureMgr = Game.Services.GetService(typeof(ResourceManager<Texture2D>)) as ResourceManager<Texture2D>;
            texturePlane = TextureMgr.Find(TextureName);
            base.LoadContent();
        }

        /// <summary>
        /// Initializes the shader parameters
        /// </summary>
        protected override void InitializeBscEffectParameters()
        {
            BscEffect.TextureEnabled = true;
            BscEffect.Texture = texturePlane;
            AlphaMgr = BlendState.AlphaBlend;
        }

        /// <summary>
        /// Populates Vertices
        /// </summary>
        protected override void InitializeVertices()
        {
            int vertexIndex = -1;
            for (int i = 0 ; i < NumRows ; ++i)
            {
                for (int j = 0 ; j < NumColumns + ADDITIONAL_VERTEX_FOR_LINE ; ++j)
                {
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[j, i], TexturePts[j, i]);
                    Vertices[++vertexIndex] = new VertexPositionTexture(VertexPts[j, i + ADDITIONAL_VERTEX_FOR_LINE], TexturePts[j, i + ADDITIONAL_VERTEX_FOR_LINE]);
                }
            }
        }

        /// <summary>
        /// Draws a triangle stripe to the screen
        /// </summary>
        /// <param name="stripIndex">Triangle stripe index to display</param>
        protected override void DrawTriangleStrip(int stripIndex)
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, Vertices, stripIndex * NumVertices / NumRows, NumTrianglesPerStrip);
        }
    }
}
