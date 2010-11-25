using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SGDE
{
   /// <summary>
   /// Base Class representative of any onscreen drawable that appears in a scene
   /// </summary>
   public abstract partial class Entity : SceneNode
   {
      /// <summary>Sprite which is drawn to represent the entity</summary>
      protected SGDE.Graphics.Sprite image;

      protected SGDE.Physics.Collision.CollisionUnit mCollisionUnit;

      protected SGDE.Physics.PhysicsBaby mPhysBaby;

      /// <summary>Object which allows for keyboard events. Only instantiate if object responds to keyboard input.</summary>
      protected KeyboardComponent keyboardListener;

      /// <summary>
      /// Constructs a new entity in the scene
      /// </summary>
      /// <param name="x">X coordinate</param>
      /// <param name="y">Y coordinate</param>
      public Entity( float x = 0, float y = 0 )
      {
         SetRotation( 0 );
         mPhysBaby = new SGDE.Physics.PhysicsBaby(this);
         image = new SGDE.Graphics.Sprite();
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

      public void EnablePhysics(bool bPhysics, bool bCollision)
      {
         if (bPhysics)
         {
            SGDE.Physics.PhysicsPharaoh.GetInstance().AddPhysicsBaby(mPhysBaby);
         }
         else
         {
            SGDE.Physics.PhysicsPharaoh.GetInstance().RemovePhysicsBaby(mPhysBaby);
         }

         if (bCollision && mCollisionUnit != null)
         {
            SGDE.Physics.PhysicsPharaoh.GetInstance().AddCollisionUnit(mCollisionUnit);
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

      public SGDE.Physics.PhysicsBaby GetPhysicsBaby()
      {
         return mPhysBaby;
      }

      public SGDE.Physics.Collision.CollisionUnit GetCollisionUnit()
      {
         return mCollisionUnit;
      }

      public void SetCollisionUnit(SGDE.Physics.Collision.CollisionUnit unit)
      {
         mCollisionUnit = unit;
         mPhysBaby.AddCollisionUnit(unit);
         AddChild(unit);

         SGDE.Physics.PhysicsPharaoh.GetInstance().AddCollisionUnit(unit);
      }

      public virtual void LoadContent(ContentManager theContentManager, String theAssetName)
      {
         image.LoadContent(theContentManager, theAssetName);

         SetUpCollision();
      }

      // call after image.LoadContent()
      protected virtual void SetUpCollision()
      {
         int radius = Math.Max(image.GetWidth(), image.GetHeight()) / 2;

         mCollisionUnit = new SGDE.Physics.Collision.CollisionUnit(this, image.GetCenter(), radius, null, false);
         mPhysBaby.AddCollisionUnit(mCollisionUnit);
         AddChild(mCollisionUnit);
      }

      /// <summary>Draws the entity to the screen</summary>
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
      }

      public void HandleInput(KeyboardState keyboardState, ContentManager content)
      {
         keyboardListener.HandleEvents(keyboardState, content);
      }
   }
}
