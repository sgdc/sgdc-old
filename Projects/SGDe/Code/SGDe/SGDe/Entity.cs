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
        private CollisionUnit mCollisionUnit;
        private PhysicsBaby mPhysBaby;

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
            if (bPhysics)
            {
                pharaoh.AddPhysicsBaby(mPhysBaby);
            }
            else
            {
                pharaoh.RemovePhysicsBaby(mPhysBaby);
            }

            if (bCollision)
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

        public void LoadContent(ContentManager theContentManager, String theAssetName)
        {
            int radius;

            image.LoadContent(theContentManager, theAssetName);

            radius = Math.Max(image.GetWidth(), image.GetHeight()) / 2;
            mCollisionUnit = new CollisionUnit(this, image.GetCenter(), radius, null, false);
            AddChild(mCollisionUnit);
        }

        public virtual void Draw()
        {
            image.Draw();
        }

        public void SetColor(Color backColor)
        {
            image.SetBackgroundColor(backColor);
        }
    }
}
