using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAProject
{
   public class ResourceManager<T>
   {
      Game AS2D { get; set; }
      string ResourcesLocation { get; set; }
      List<BasicResource<T>> ResourceList { get; set; }

      public ResourceManager(Game game, string resourcesLocation)
      {
         AS2D = game;
         ResourcesLocation = resourcesLocation;
         ResourceList = new List<BasicResource<T>>();
      }

      public void Add(string name, T existingResource)
      {
         BasicResource<T> resourceToAdd = new BasicResource<T>(name, existingResource);
         if (!ResourceList.Contains(resourceToAdd))
         {
            ResourceList.Add(resourceToAdd);
         }
      }

      void Add(BasicResource<T> resourceToAdd)
      {
         resourceToAdd.Load();
         ResourceList.Add(resourceToAdd);
      }

      public T Find(string resourceName)
      {
         const int RESOURCE_NOT_FOUND = -1;
         BasicResource<T> resourceToFind = new BasicResource<T>(AS2D.Content, ResourcesLocation, resourceName);
         int resourceIndex = ResourceList.IndexOf(resourceToFind);
         if (resourceIndex == RESOURCE_NOT_FOUND)
         {
            Add(resourceToFind);
            resourceIndex = ResourceList.Count - 1;
         }
         return ResourceList[resourceIndex].Resource;
      }
   }
}
