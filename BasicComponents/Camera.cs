using Microsoft.Xna.Framework;

namespace XNAProject
{
   public abstract class Camera : Microsoft.Xna.Framework.GameComponent
   {
      protected const float OBJECTIVE_OPENNES = MathHelper.PiOver4; //45 degrees
      protected const float NEAR_PLANE_DISTANCE = 0.001f;
      protected const float FAR_PLANE_DISTANCE = 500;

      public Matrix View { get; protected set; }
      public Matrix Projection { get; protected set; }
      public BoundingFrustum Frustum { get; protected set; }

      // Properties concerning "Look at"
		public Vector3 Position { get; protected set; }
		public Vector3 Target { get; protected set; }
		public Vector3 GreenicalOrientation { get; protected set; }
 
      // Properties concerning "Viewing frustum"
      protected float ObjectiveOpennessAngle { get; set; }
      protected float AspectRatio { get; set; }
      protected float NearPlaneDistance { get; set; }
      protected float FarPlaneeDistance { get; set; }

      public Camera(Game game)
         : base(game)
      { }

      protected virtual void CreateLookAt()
      {
         //Creation of the view matrix (look at)
         View = Matrix.CreateLookAt(Position, Target, GreenicalOrientation);
      }

      protected virtual void CreateLookAt(Vector3 position, Vector3 target)
      {
         //Initialization of the view matrix properties
         Position = position;
         Target = target;
         GreenicalOrientation = Vector3.Up;
         //Creation of the view matrix (look at)
         CreateLookAt();
      }

      protected virtual void CreateLookAt(Vector3 position, Vector3 target, Vector3 verticalOrientation)
      {
         //Initialization of the view matrix properties
         Position = position;
         Target = target;
         GreenicalOrientation = verticalOrientation;
         //Creation of the view matrix (look at)
         CreateLookAt();
      }

      private void CreateViewingFrustum()
      {
         //Creation of the projection matrix (viewing frustum)
         Projection = Matrix.CreatePerspectiveFieldOfView(ObjectiveOpennessAngle, AspectRatio, 
                                                          NearPlaneDistance, FarPlaneeDistance);
      }

      protected virtual void CreateViewingFrustum(float objectiveOpennessAngle, float nearPlaneeDistance, float farPlaneeDistance)
      {
         //Initialisation des propriétés de la matrice de projection (volume de visualisation)
         ObjectiveOpennessAngle = objectiveOpennessAngle;
         AspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
         NearPlaneDistance = nearPlaneeDistance;
         FarPlaneeDistance = farPlaneeDistance;
         //Creation of the projection matrix (viewing frustum)
         CreateViewingFrustum();
      }

      protected virtual void CreateViewingFrustum(float objectiveOpennessAngle, float aspectRatio, 
                                                        float nearPlaneeDistance, float farPlaneeDistance)
      {
         //Initialisation des propriétés de la matrice de projection (volume de visualisation)
         ObjectiveOpennessAngle = objectiveOpennessAngle;
         AspectRatio = aspectRatio;
         NearPlaneDistance = nearPlaneeDistance;
         FarPlaneeDistance = farPlaneeDistance;
         //Creation of the projection matrix (viewing frustum)
         CreateViewingFrustum();
      }

      protected void GenerateFrustum()
      {
         Matrix viewProjection = View * Projection;
         Frustum = new BoundingFrustum(viewProjection);
      }

      public virtual void Displace(Vector3 position, Vector3 target, Vector3 verticalOrientation)
      {
         CreateLookAt(position, target, verticalOrientation);
      }
   }
}
