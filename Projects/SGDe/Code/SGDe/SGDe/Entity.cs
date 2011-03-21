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
    /// <summary>
    /// Base Class representative of any onscreen drawable that appears in a scene
    /// </summary>
    public abstract class Entity : SceneNode, IUpdateable
    {
        #region Fields

        /// <summary>Sprite which is drawn to represent the entity</summary>
        protected Sprite image;

        /// <summary>Contains the collision logic for the entity</summary>
        protected CollisionUnit mCollisionUnit;

        /// <summary>Contains the general collision data for the entity.</summary>
        protected PhysicsBaby mPhysBaby;

        /// <summary> ID for Checking the type of an Entity </summary>
        protected uint id;

        //Used for EntityBuilder
        internal bool penabled, pcollision;
        internal object[] args;

        internal int order;

        private bool enabled;

        #endregion

        /// <summary>
        /// Constructs a new entity in the scene
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
#if WINDOWS
        public Entity(float x = 0, float y = 0)
#else
        public Entity()
            : this(0, 0)
        {
        }

        public Entity(float x, float y)
#endif
        {
            SetRotation(0);
            mPhysBaby = new PhysicsBaby(this);
            SetTranslation(new Vector2(x, y));
            id = 0; //Zero is a generic id
        }

        /// <summary>
        /// Constructs a new entity in the scene
        /// </summary>
        /// <param name="position">Coordinates</param>
        public Entity(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        #region Properties and Events

        /// <summary>
        /// Get or set if the Entity component is enabled or not. If the entity is enabled then the following functions are called: Update, HandleInput.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    if (this.EnabledChanged != null)
                    {
                        this.EnabledChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Get or set the update order. Indicates when the game component should be updated relative to other game components. Lower values are updated first.
        /// </summary>
        public int UpdateOrder
        {
            get
            {
                return this.order;
            }
            set
            {
                if (this.order != value)
                {
                    this.order = value;
                    if (this.UpdateOrderChanged != null)
                    {
                        this.UpdateOrderChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Raised when the UpdateOrder property changes.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Raised when the Enabled property changes.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Set the entity's tint.
        /// </summary>
        /// <param name="backColor">The tint to use.</param>
        [Obsolete("Use {this}.SpriteImage.Tint instead")]
        public void SetColor(Color backColor)
        {
            image.Tint = backColor;
        }

        /// <summary>
        /// Get the Entity's Sprite component.
        /// </summary>
        public Sprite SpriteImage
        {
            get
            {
                return this.image;
            }
            internal set
            {
                if (this.image != value)
                {
                    if (this.image != null)
                    {
                        base.RemoveChild(this.image);
                    }
                    this.image = value;
                    base.AddChild(this.image);
                }
            }
        }

        #endregion

        #region Initialization

        internal void InSetUpCollision()
        {
            this.SetUpCollision();
        }

        /// <summary>
        /// Enable physics on this Entity.
        /// </summary>
        /// <param name="bPhysics"><code>true</code> if physics should be enabled for this Entity, <code>false</code> if it should be disabled.</param>
        /// <param name="bCollision"><code>true</code> collisions should be enabled for this Entity, <code>false</code> if it should be disabled.</param>
        public void EnablePhysics(bool bPhysics, bool bCollision)
        {
            PhysicsPharaoh pharaoh = PhysicsPharaoh.GetInstance();
            penabled = bPhysics;
            pcollision = bCollision;

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
                //pharaoh.AddCollisionUnit(mCollisionUnit);
            }
            else
            {
                // TODO: Remove collision unit
            }
        }

        #endregion

        #region Update/Draw

        /// <summary>
        /// Called once during each step taken by the engine
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public virtual void Update(GameTime gameTime)
        {
            image.Update(gameTime);
        }

        /// <summary>Draws the entity to the screen</summary>
        public virtual void Draw(GameTime gameTime)
        {
            if (image.Visible && image.IsVisible)
            {
                //Only draw when the developer desires it to be visible and if it would be visible if drawn
                image.Draw(gameTime);
            }
        }

        #endregion

        #region Physics

        /// <summary>
        /// Set this Entity's velocity. This is a helper function that is equivilant to <code>SetVelocity(new Vector2(x, y));</code>.
        /// </summary>
        /// <param name="x">The horizontal velocity of for this Entity.</param>
        /// <param name="y">The veritcal velocity of for this Entity.</param>
        public void SetVelocity(float x, float y)
        {
            mPhysBaby.SetVelocity(new Vector2(x, y)); // inherent direction?
        }

        /// <summary>
        /// Set this Entity's velocity. This is a helper function that is equivilant to <code>GetPhysicsBaby().SetVelocity(velocity);</code>.
        /// </summary>
        /// <param name="velocity">The velocity to set this Entity's velocity to.</param>
        public void SetVelocity(Vector2 velocity)
        {
            mPhysBaby.SetVelocity(velocity);
        }

        /// <summary>
        /// Get this Entity's velocity. This is a helper function that is equivilant to <code>GetPhysicsBaby().GetVelocity();</code>.
        /// </summary>
        /// <returns>This Entity's velocity.</returns>
        public Vector2 GetVelocity()
        {
            return mPhysBaby.GetVelocity();
        }

        /// <summary>
        /// Get the PhysicsBaby for this Entity.
        /// </summary>
        /// <returns>This Entity's PhysicsBaby.</returns>
        public PhysicsBaby GetPhysicsBaby()
        {
            return mPhysBaby;
        }

        /// <summary>
        /// Get the collision unit for this Entity
        /// </summary>
        /// <returns>The collision unit for this Entity.</returns>
        public CollisionUnit GetCollisionUnit()
        {
            return mCollisionUnit;
        }

        /// <summary>
        /// Set the collision unit for this Entity.
        /// </summary>
        /// <param name="unit">The collision unit to set for this Entity.</param>
        public void SetCollisionUnit(CollisionUnit unit)
        {
            if (unit != null && unit != mCollisionUnit)
            {
                if (mCollisionUnit != null)
                {
                    //Cleanup collision units
                    //TODO
                }
                mCollisionUnit = unit;
                mPhysBaby.AddCollisionUnit(unit);
                AddChild(unit);

                PhysicsPharaoh.GetInstance().AddCollisionUnit(unit);
            }
        }

        /// <summary>
        /// Set up the collision unit for this Entity.
        /// </summary>
        protected virtual void SetUpCollision()
        {
            int radius = Math.Max(image.Width, image.Height) / 2;
            SetCollisionUnit(new SGDE.Physics.Collision.CollisionUnit(this, image.Center, radius, null, false));
        }

        /// <summary>
        /// Collision change event. This is usually called when an Entitiy collides with this Entity.
        /// </summary>
        public virtual void CollisionChange()
        {
            if (mCollisionUnit.HasCollisions())
            {
                SetVelocity(GetVelocity() * -1);
            }
            //foreach (CollisionUnit other in mCollisionUnit.GetCollisions())
            //{
            //    mPhysBaby.AddBounce2(mCollisionUnit, other);
            //}
        }

        #endregion

        #region CopyTo

        /// <summary>
        /// Copy this Entity to another entity. If overriden then the base.CopyTo call must be the first line of code.
        /// </summary>
        /// <param name="ent">The entity to copy to.</param>
        public virtual void CopyTo(ref Entity ent)
        {
            //SceneNode
            SceneNode node = ent;
            base.CopyTo(ref node);
            //Entity
            ent.SpriteImage = (Sprite)Activator.CreateInstance(this.image.GetType());
            this.image.CopySpriteToIn(ref ent.image, true);
            SceneNode nNode = ent.SpriteImage;
            node.CopyTo(ref nNode);
            ent.id = this.id;
            /*
            if (this.mCollisionUnit != null && ent.mCollisionUnit == null)
            {
                ent.SetUpCollision();
            }
            ent.keyboardListener = this.keyboardListener;
            //Need to copy over physics attributes manually
            SGDE.Physics.PhysicsBaby nbaby = this.GetPhysicsBaby();
            SGDE.Physics.PhysicsBaby obaby = ent.GetPhysicsBaby();
            if (obaby.IsStatic() != nbaby.IsStatic())
            {
                obaby.SetStatic(nbaby.IsStatic());
            }
            if (obaby.GetForces() != nbaby.GetForces())
            {
                obaby.SetForces(nbaby.GetForces());
            }
            if (obaby.GetVelocity() != nbaby.GetVelocity())
            {
                obaby.SetVelocity(nbaby.GetVelocity());
            }
            ent.EnablePhysics(this.penabled, this.pcollision);
             */
        }

        #endregion

        #region ID

        /// <summary>
        /// Get the ID of this Entity.
        /// </summary>
        /// <returns>This Entity's ID.</returns>
        public uint GetID()
        {
            return id;
        }

        /*
        /// <summary>
        /// Set the ID for this Entity.
        /// </summary>
        /// <param name="id">The ID of this Entity.</param>
        internal void SetID(uint id)
        {
            this.id = id;
        }
         */

        /// <summary>
        /// Compare this ID's Entity with another Entity to determine if they are the same.
        /// </summary>
        /// <param name="e">The other Entity to compare IDs with.</param>
        /// <returns><code>true</code> if the Entities' IDs are the same.</returns>
        public bool CompareID(Entity e)
        {
            return (e.id == id);
        }

        #endregion
    }
}
