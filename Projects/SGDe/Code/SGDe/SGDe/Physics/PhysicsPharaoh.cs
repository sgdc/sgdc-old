using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SGDE.Physics
{
   public class PhysicsPharaoh
   {
      private static PhysicsPharaoh mInstance = new PhysicsPharaoh( );

      private List<PhysicsBaby> mStaticBabies;
      private List<PhysicsBaby> mDynamicBabies;
      private Vector2 mGravity;

      private PhysicsPharaoh()
      {
      }

      public void AddPhysicsBaby(PhysicsBaby physBaby)
      {
         physBaby.AddForce(mGravity);

         if (physBaby.IsStatic())
         {
            mStaticBabies.Add(physBaby);
         }
         else
         {
            mDynamicBabies.Add(physBaby);
         }
      }

      public void RemovePhysicsBaby(PhysicsBaby physBaby)
      {
         if (physBaby.IsStatic())
         {
            mStaticBabies.Remove(physBaby);
         }
         else
         {
            mDynamicBabies.Remove(physBaby);
         }
      }

      public void AddCollisionUnit(SGDE.Physics.Collision.CollisionUnit unit)
      {
         SGDE.Physics.Collision.CollisionChief.GetInstance().AddCollisionUnit(unit);
         SGDE.Physics.Collision.CollisionChief.GetInstance().UpdateUnit(unit);
      }

      public void DrawCollisionGrid(SpriteBatch spriteBatch, Texture2D gridTexture)
      {
         SGDE.Physics.Collision.CollisionChief.GetInstance().DrawCollisionGrid(spriteBatch, gridTexture);
      }

      public void SetGravity(Vector2 gravity)
      {
         foreach (PhysicsBaby physBaby in mDynamicBabies)
         {
            physBaby.AddForce(mGravity * -1);
            physBaby.AddForce(gravity);
         }

         foreach (PhysicsBaby physBaby in mStaticBabies)
         {
            physBaby.AddForce(mGravity * -1);
            physBaby.AddForce(gravity);
         }

         mGravity = gravity;
      }

      public Vector2 GetGravity()
      {
         return mGravity;
      }

      public void Update(GameTime gameTime)
      {
         foreach (PhysicsBaby physBaby in mDynamicBabies)
         {
            physBaby.Update(gameTime);
         }

         SGDE.Physics.Collision.CollisionChief.GetInstance().Update();
      }

      public void Initialize(Vector2 worldSize, Vector2 collisionCellSize)
      {
         SGDE.Physics.Collision.CollisionChief.GetInstance( ).Initialize(worldSize, collisionCellSize);
         mStaticBabies = new List<PhysicsBaby>();
         mDynamicBabies = new List<PhysicsBaby>();
         mGravity = new Vector2(0, 0);
      }

      public static PhysicsPharaoh GetInstance()
      {
         return mInstance;
      }
   }
}
