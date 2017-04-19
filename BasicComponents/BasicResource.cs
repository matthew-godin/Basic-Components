using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
   public class BasicResource<T>:IEquatable<BasicResource<T>>
   {
      public string Name { get; private set; }
      public T Resource { get; private set; }
      ContentManager Content { get; set; }
      string Location { get; set; }

      // This constructor is called when we build a BasicTexture object
      // from an image already present in memory.
      public BasicResource(string name, T ressource)
      {
         Name = name;
         Content = null;
         Location = "";
         Resource = ressource;
      }

      // This constructor is called when we build a BasicTexture object
      // from an image that will eventually be loaded in memory.
      public BasicResource(ContentManager content, string répertoire, string name)
      {
         Name = name;
         Content = content;
         Location = répertoire;
         Resource = default(T);
      }

      public void Load()
      {
          if (Resource == null)
         {
            string fullName = Location + "/" + Name;
            Resource = Content.Load<T>(fullName);
         }
      }

      #region IEquatable<BasicTexture> Members

      public bool Equals(BasicResource<T> basicResourceToCompare)
      {
         return Name == basicResourceToCompare.Name;
      }

      #endregion
   }
}
