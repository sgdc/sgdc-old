using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SGDE.Graphics;
using SGDE.Physics.Collision;
using SGDE.Physics;

namespace SGDE
{
    public class Entity : SceneNode
    {
        protected Sprite image;
        protected CollisionUnit mCollisionUnit;
        protected PhysicsBaby mPhysBaby;
        private PhysicsPharaoh mPhysicsPharaoh;

        public Entity()
            : this(0, 0)
        {
        }

        public Entity(int x, int y)
        {
            SetRotation(0);
            mPhysBaby = new PhysicsBaby(this);
            image = new Sprite();
            AddChild(image);
            SetTranslation(new Vector2(x, y));
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

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public void Velocity(float x, float y)
        {
            mPhysBaby.SetVelocity(new Vector2(x, y)); // inherent direction?
        }

        public void Velocity(Vector2 velocity)
        {
            mPhysBaby.SetVelocity(velocity);
        }

        public Vector2 Velocity()
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
                Velocity(Velocity() * -1);
            }
            //foreach (CollisionUnit other in mCollisionUnit.GetCollisions())
            //{
            //    mPhysBaby.AddBounce2(mCollisionUnit, other);
            //}
        }
    }
}
