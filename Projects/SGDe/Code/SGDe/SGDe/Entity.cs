using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using SGDE.Graphics;
using SGDE.Physics.Collision;
using SGDE.Physics;

namespace SGDE
{
   /// <summary>
   /// Base Class representative of any onscreen drawable that appears in a scene
   /// </summary>
   public abstract class Entity : SceneNode
   {
      /// <summary>Sprite which is drawn to represent the entity</summary>
      protected Sprite image;

      /// <summary>Contains the collision logic for the entity</summary>
      protected CollisionUnit mCollisionUnit;

      protected PhysicsBaby mPhysBaby;
      private PhysicsPharaoh mPhysicsPharaoh;

      /// <summary>
      /// Constructs a new entity in the scene
      /// </summary>
      /// <param name="x">X coordinate</param>
      /// <param name="y">Y coordinate</param>
      public Entity( float x = 0, float y = 0 )
      {
         SetRotation( 0 );
         mPhysBaby = new PhysicsBaby( this );
         image = new Sprite( );
         AddChild( image );
         SetTranslation( new Vector2( x, y ) );
      }

      /// <summary>
      /// Constructs a new entity in the scene
      /// </summary>
      /// <param name="position">Coordinates</param>
      public Entity(Vector2 position)
         : this( position.X, position.Y )
      {
      }

      public void EnablePhysics(PhysicsPharaoh pharaoh, bool bPhysics, bool bCollision)
      {
         mPhysicsPharaoh = pharaoh;

         if (bPhysics)
         {
            pharaoh.AddPhysicsBaby(mPhysBaby);
         }
         else
         {
            pharaoh.RemovePhysicsBaby(mPhysBaby);
         }

         if (bCollision && mCollisionUnit != null)
         {
            pharaoh.AddCollisionUnit(mCollisionUnit);
         }
         else
         {
            // TODO: Remove collision unit
         }
      }

      /// <summary>
      /// Called once during each step taken by the engine
      /// </summary>
      /// <param name="gameTime">The current game time</param>
      public virtual void Update(GameTime gameTime)
      {
      }

      public void SetVelocity(float x, float y)
      {
         mPhysBaby.SetVelocity(new Vector2(x, y)); // inherent direction?
      }

      public void SetVelocity(Vector2 velocity)
      {
         mPhysBaby.SetVelocity(velocity);
      }

      public Vector2 GetVelocity()
      {
         return mPhysBaby.GetVelocity();
      }

      public PhysicsBaby GetPhysicsBaby()
      {
         return mPhysBaby;
      }

      public CollisionUnit GetCollisionUnit()
      {
         return mCollisionUnit;
      }

      public void SetCollisionUnit(CollisionUnit unit)
      {
         mCollisionUnit = unit;
         mPhysBaby.AddCollisionUnit(unit);
         AddChild(unit);

         if (mPhysicsPharaoh != null)
         {
            mPhysicsPharaoh.AddCollisionUnit(unit);
         }
      }

      public virtual void LoadContent(ContentManager theContentManager, String theAssetName)
      {
         //int radius;

         image.LoadContent(theContentManager, theAssetName);

         //radius = Math.Max(image.GetWidth(), image.GetHeight()) / 2;
         //mCollisionUnit = new CollisionUnit(this, image.GetCenter(), radius, null, false);
         //AddChild(mCollisionUnit);

         SetUpCollision();
      }

      // call after image.LoadContent()
      protected virtual void SetUpCollision()
      {
         int radius;

         radius = Math.Max(image.GetWidth(), image.GetHeight()) / 2;
         mCollisionUnit = new CollisionUnit(this, image.GetCenter(), radius, null, false);
         mPhysBaby.AddCollisionUnit(mCollisionUnit);
         AddChild(mCollisionUnit);

         //SetCollisionUnit(new CollisionUnit(this, image.GetCenter(), radius, null, false));
         //pharaoh.AddCollisionUnit(mCollisionUnit);
      }

      public virtual void Draw()
      {
         image.Draw();
      }

      public void SetColor(Color backColor)
      {
         image.SetBackgroundColor(backColor);
      }

      public virtual void CollisionChange()
      {
         if (mCollisionUnit.HasCollisions())
         {
            SetVelocity( GetVelocity() * -1 );
         }
      }
   }
}
