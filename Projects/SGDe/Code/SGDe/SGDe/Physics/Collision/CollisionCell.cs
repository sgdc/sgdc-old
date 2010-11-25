using System;
using System.Collections.Generic;

namespace SGDE.Physics.Collision
{
   public partial class CollisionChief
   {
      protected partial class CollisionCell
      {
         public List<CollisionUnit> collisionMembers;

         public CollisionCell()
         {
            collisionMembers = new List<CollisionUnit>();
         }

         public void AddCollisionUnit(CollisionUnit unit)
         {
            collisionMembers.Add(unit);
         }

         public void RemoveCollisionUnit(CollisionUnit unit)
         {
            collisionMembers.Remove(unit);
         }
      }
   }
}
