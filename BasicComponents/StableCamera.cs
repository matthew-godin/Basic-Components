using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class StableCamera : Camera
   {
      public StableCamera(Game game, Vector3 cameraPosition, Vector3 target, Vector3 orientation)
         : base(game)
      {
         CreateLookAt(cameraPosition, target, orientation); // Creation of the view matrix
         CreateViewingFrustum(Camera.OBJECTIVE_OPENNES, Camera.NEAR_PLANE_DISTANCE, Camera.FAR_PLANE_DISTANCE); // Creation of the projection matrix (viewing frustum)
      }
   }
}
