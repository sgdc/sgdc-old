using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Physics
{
    public class PhysicsBaby
    {
        private bool bStatic;
        private Vector2 mVelocity;
        private Entity mOwner;

        public PhysicsBaby(Entity owner)
        {
            bStatic = false;
            mVelocity = new Vector2(0.0f, 0.0f);

            mOwner = owner;
        }

        public void SetStatic(bool beStatic)
        {
            bStatic = beStatic;
        }

        public bool IsStatic()
        {
            return bStatic;
        }

        public void SetVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }

        public void AddVelocity(Vector2 velocity)
        {
            mVelocity += velocity;
        }

        public Vector2 GetVelocity()
        {
            return mVelocity;
        }

        public void Update(GameTime gameTime)
        {
            mOwner.Translate((int)mVelocity.X, (int)mVelocity.Y);
        }
    }
}
