using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SGDE.Physics.Collision;

namespace SGDE.Physics
{
    public class PhysicsPharaoh
    {
        private CollisionChief mCollisionChief;
        private List<PhysicsBaby> mStaticBabies;
        private List<PhysicsBaby> mDynamicBabies;

        public PhysicsPharaoh(Vector2 worldSize, Vector2 collisionCellSize)
        {
            mCollisionChief = new CollisionChief(worldSize, collisionCellSize);
            mStaticBabies = new List<PhysicsBaby>();
            mDynamicBabies = new List<PhysicsBaby>();
        }

        public void AddPhysicsBaby(PhysicsBaby physBaby)
        {
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

        public void AddCollisionUnit(CollisionUnit unit)
        {
            mCollisionChief.AddCollisionUnit(unit);
            unit.SetCollisionChief(mCollisionChief);
        }

        public void Update(GameTime gameTime)
        {
            foreach (PhysicsBaby physBaby in mDynamicBabies)
            {
                physBaby.Update(gameTime);
            }

            mCollisionChief.Update();
        }
    }
}
