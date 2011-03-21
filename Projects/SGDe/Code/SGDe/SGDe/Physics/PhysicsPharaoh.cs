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
    /// <summary>
    /// Central manager for game physics.
    /// </summary>
    public class PhysicsPharaoh
    {
        private static PhysicsPharaoh mInstance;

        private List<PhysicsBaby> mStaticBabies;
        private List<PhysicsBaby> mDynamicBabies;
        private Vector2 mGravity;

        private PhysicsPharaoh() { }

        /// <summary>
        /// Add a PhysicsBaby to be managed by the Pharaoh.
        /// </summary>
        /// <param name="physBaby">The PhysicsBaby to add.</param>
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

        /// <summary>
        /// Remove a PhysicsBaby from the Pharaoh's control.
        /// </summary>
        /// <param name="physBaby">The PhysicsBaby to free.</param>
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

        /// <summary>
        /// Add a CollisionUnit to the Pharaoh's control.
        /// </summary>
        /// <param name="unit">The CollisionUnit to add.</param>
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

        /// <summary>
        /// Remove a CollisionUnit from the Pharaoh's control.
        /// </summary>
        /// <param name="unit">the CollisionUnit to remove.</param>
        public void RemoveCollisionUnit(CollisionUnit unit)
        {
            if (ContentUtil.LoadingBuilders || unit == null)
            {
                return;
            }
            CollisionChief chief = CollisionChief.GetInstance();
            chief.RemoveCollisionUnit(unit);
        }

        /// <summary>
        /// Helper function to draw the currently active physics grid.
        /// </summary>
        /// <param name="gridTexture">The texture that represents a cell within the physics grid.</param>
        public void DrawCollisionGrid(Texture2D gridTexture)
        {
            CollisionChief.GetInstance().DrawCollisionGrid(SGDE.Graphics.SpriteManager.gfx, gridTexture);
        }

        /// <summary>
        /// Set the game gravity for all physics babies.
        /// </summary>
        /// <param name="gravity">Gravity to affect all units.</param>
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

        /// <summary>
        /// Get the game gravity.
        /// </summary>
        /// <returns>Game gravity.</returns>
        public Vector2 GetGravity()
        {
            return mGravity;
        }

        /// <summary>
        /// Update the game's physics system.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public void Update(GameTime gameTime)
        {
            if (mDynamicBabies != null)
            {
                List<Entity> entities = null;
                if (Game.CurrentGame.gameContent != null)
                {
                    entities = Game.CurrentGame.gameContent.UpdateEntities;
                }

                foreach (PhysicsBaby physBaby in mDynamicBabies)
                {
                    if (entities != null && !entities.Contains(physBaby.mOwner))
                    {
                        continue;
                    }
                    physBaby.Update(gameTime);
                }
            }

            CollisionChief.GetInstance().Update();
        }

        /// <summary>
        /// Initialize the PhysicsPharaoh.
        /// </summary>
        /// <param name="worldSize">The size of the physical map.</param>
        /// <param name="collisionCellSize">The size of a grid within the physical map.</param>
        public void Initialize(Vector2 worldSize, Vector2 collisionCellSize)
        {
            SGDE.Physics.Collision.CollisionChief.GetInstance().Initialize(worldSize, collisionCellSize);
            mStaticBabies = new List<PhysicsBaby>();
            mDynamicBabies = new List<PhysicsBaby>();
            mGravity = new Vector2(0, 0);
        }

        /// <summary>
        /// Get the PhysicsPharaoh singleton.
        /// </summary>
        /// <returns>PhysicsPharaoh singleton.</returns>
        public static PhysicsPharaoh GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = new PhysicsPharaoh();
            }
            return mInstance;
        }
    }
}
