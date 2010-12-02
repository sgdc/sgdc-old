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
   public class BounceBall : Entity
   {
      private List<CollisionUnit> mPrevCheckedUnits;
      private Vector2 mHitLocation;
      private Sprite mHitSprite;

      public BounceBall(float x = 0, float y = 0)
         : base(x, y)
      {
         mPrevCheckedUnits = new List<CollisionUnit>();
         mHitLocation = new Vector2(0, 0);
      }

      public override void Update(GameTime gameTime)
      {
         CollisionUnit unit = GetCollisionUnit();

         foreach (CollisionUnit other in mPrevCheckedUnits)
         {
            other.GetOwner().SetColor(Color.White);
         }

         mPrevCheckedUnits = new List<CollisionUnit>(GetCollisionUnit().GetCheckedUnits());

         foreach (CollisionUnit other in mPrevCheckedUnits)
         {
            other.GetOwner().SetColor(Color.Green);
         }

         if (GetCollisionUnit().HasCollisions())
         {
            //Velocity(Velocity().X * -1, Velocity().Y * -1);

            foreach (CollisionUnit other in GetCollisionUnit().GetCollisions())
            {
               other.GetOwner().SetColor(Color.Pink);
               mHitLocation = GetCollisionUnit().GetCollisionPoint(other);
            }
         }
      }

      public void DrawHitSpot(Texture2D hitTexture)
      {
         Vector2 drawSpot = new Vector2();
         drawSpot.X = mHitLocation.X - (hitTexture.Width / 2);
         drawSpot.Y = mHitLocation.Y - (hitTexture.Height / 2);

         Sprite.spriteBatch.Draw(hitTexture, drawSpot, Color.White);
      }

      public override void CollisionChange()
      {
         foreach (CollisionUnit other in mCollisionUnit.GetCollisions())
         {
            if (other.IsSolid())
            {
               mPhysBaby.AddBounce(mCollisionUnit, other);
            }
         }
      }
   }
}