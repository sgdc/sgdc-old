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
   public class BounceBox : Entity
   {
      //CollisionUnit mBoxCollisionUnit;

      public BounceBox(int x, int y)
         : base(x, y)
      {
      }

      //public override void LoadContent(ContentManager theContentManager, string theAssetName)
      //{
      //   image.LoadContent(theContentManager, theAssetName);

      //   mCollisionUnit = new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.GetWidth(), image.GetHeight()), CollisionUnit.COLLISION_BOX, null, false);
      //   mPhysBaby.AddCollisionUnit(mCollisionUnit);
      //   AddChild(mCollisionUnit);
      //}

      protected override void SetUpCollision()
      {
         //mCollisionUnit = new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.GetWidth(), image.GetHeight()), CollisionUnit.COLLISION_BOX, null, false);
         //mPhysBaby.AddCollisionUnit(mCollisionUnit);
         //AddChild(mCollisionUnit);

         //mBoxCollisionUnit = new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.GetWidth(), image.GetHeight()), CollisionUnit.COLLISION_BOX, null, false);
         //mPhysBaby.AddCollisionUnit(mBoxCollisionUnit);
         //AddChild(mBoxCollisionUnit);

         SetCollisionUnit(new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.GetWidth(), image.GetHeight()), CollisionUnit.CollisionType.COLLISION_BOX, null, false));
         //SetCollisionUnit(mBoxCollisionUnit);
      }
   }
}
