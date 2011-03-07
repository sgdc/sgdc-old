using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SGDE.Physics.Collision;
using SGDE.Content;

namespace SGDE.Physics
{
    public class PhysicsPharaoh
    {
        private static PhysicsPharaoh mInstance = new PhysicsPharaoh();

        private List<PhysicsBaby> mStaticBabies;
        private List<PhysicsBaby> mDynamicBabies;
        private Vector2 mGravity;

        private PhysicsPharaoh() { }

        public void AddPhysicsBaby(PhysicsBaby physBaby)
        {
            if (ContentUtil.LoadingBuilders)
            {
                return;
            }
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
            if (ContentUtil.LoadingBuilders)
            {
                return;
            }
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
            if (ContentUtil.LoadingBuilders)
            {
                return;
            }
            CollisionChief chief = CollisionChief.GetInstance();
            chief.AddCollisionUnit(unit);
            chief.UpdateUnit(unit);
        }

        public void RemoveCollisionUnit(CollisionUnit unit)
        {
            if (ContentUtil.LoadingBuilders || unit == null)
            {
                return;
            }
            CollisionChief chief = CollisionChief.GetInstance();
            chief.RemoveCollisionUnit(unit);
        }

        public void DrawCollisionGrid(Texture2D gridTexture)
        {
            CollisionChief.GetInstance().DrawCollisionGrid(SGDE.Graphics.SpriteManager.gfx, gridTexture);
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
            if (mDynamicBabies != null)
            {
                foreach (PhysicsBaby physBaby in mDynamicBabies)
                {
                    physBaby.Update(gameTime);
                }
            }

            CollisionChief.GetInstance().Update();
        }

        public void Initialize(Vector2 worldSize, Vector2 collisionCellSize)
        {
            SGDE.Physics.Collision.CollisionChief.GetInstance().Initialize(worldSize, collisionCellSize);
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
