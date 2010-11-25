using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE;
using SGDE.Physics.Collision;
using SGDE.Graphics;

namespace TestDemo
{
   public class PortalBox : Entity
   {
      private bool bSender;
      private PortalBox mOther;
      private Vector2 mDumpPoint;

      public PortalBox(int x, int y, PortalBox other, bool isSender)
         : base(x, y)
      {
         bSender = isSender;
         mDumpPoint = new Vector2(x, y);
         mOther = other;
      }

      protected override void SetUpCollision()
      {
         SetCollisionUnit(new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.GetWidth(), image.GetHeight()), CollisionUnit.COLLISION_BOX, null, false));

         if (bSender)
         {
            GetCollisionUnit().BlockOthers(false);
         }

         mDumpPoint += new Vector2(image.GetWidth() / 2, image.GetHeight() + 10);
      }

      public void SetOther(PortalBox other)
      {
         mOther = other;
      }

      public Vector2 GetDumpPoint()
      {
         return mDumpPoint;
      }

      public void SetDumpPoint(Vector2 dumpPoint)
      {
         mDumpPoint = dumpPoint;
      }

      public override void CollisionChange()
      {
         Vector2 otherVelocity;
         Vector2 newVelocity;
         float mag;

         if (GetCollisionUnit().HasCollisions() && bSender)
         {
            foreach (CollisionUnit unit in GetCollisionUnit().GetCollisions())
            {
               otherVelocity = unit.GetOwner().GetVelocity();
               newVelocity = new Vector2(0, 0);
               mag = (float)Math.Sqrt(otherVelocity.X * otherVelocity.X + otherVelocity.Y * otherVelocity.Y);

               //newVelocity.X = otherVelocity.X / mag;
               //newVelocity.Y = otherVelocity.Y / mag;
               newVelocity.X = otherVelocity.X;
               newVelocity.Y = otherVelocity.Y / 10;

               //newVelocity *= 10;

               unit.GetOwner().SetTranslation(mOther.GetDumpPoint());
               unit.GetOwner().SetVelocity(newVelocity);
            }
         }
      }
   }
}
