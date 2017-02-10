using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace XNAProject
{
   public class BaseObject : Microsoft.Xna.Framework.DrawableGameComponent
   {
      string ModelName { get; set; }
      ResourcesManager<Model> ModelMgr { get; set; }
      Camera GameCamera { get; set; }
      protected float Scale { get; set; }
      protected Vector3 Rotation { get; set; }
      protected Vector3 Position { get; set; }

      protected Model Model { get; private set; }
      protected Matrix[] TransformationsModel { get; private set; }
      protected Matrix World { get; set; }

      public BaseObject(Game game, String modelName, float initialScale, Vector3 initialRotation, Vector3 initialPosition)
         : base(game)
      {
         ModelName = modelName;
         Position = initialPosition;
         Scale = initialScale;
         Rotation = initialRotation;
      }

      public override void Initialize()
      {
         base.Initialize();
         ComputeWorld();
      }

      private void ComputeWorld()
      {
         World = Matrix.Identity;
         World *= Matrix.CreateScale(Scale);
         World *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
         World *= Matrix.CreateTranslation(Position);
      }

      protected override void LoadContent()
      {
         GameCamera = Game.Services.GetService(typeof(Camera)) as Camera;
         ModelMgr = Game.Services.GetService(typeof(ResourcesManager<Model>)) as ResourcesManager<Model>;
         Model = ModelMgr.Find(ModelName);
         TransformationsModel = new Matrix[Model.Bones.Count];
         Model.CopyAbsoluteBoneTransformsTo(TransformationsModel);
      }

      public override void Draw(GameTime gameTime)
      {
         foreach (ModelMesh mesh in Model.Meshes)
         {
            Matrix localWorld = TransformationsModel[mesh.ParentBone.Index] * GetWorld();
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
               BasicEffect effect = (BasicEffect)meshPart.Effect;
               effect.EnableDefaultLighting();
               effect.Projection = GameCamera.Projection;
               effect.View = GameCamera.View;
               effect.World = localWorld;
            }
            mesh.Draw();
         }
      }

      public virtual Matrix GetWorld()
      {
         return World;
      }
   }
}
