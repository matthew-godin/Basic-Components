using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAProject
{
   public interface ICollidable
   {
      BoundingSphere CollisionSphere { get; }
      bool IsColliding(object otherObject);
   }
}
