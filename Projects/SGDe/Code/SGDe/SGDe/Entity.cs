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
    public abstract class Entity : SceneNode
    {
        /// <summary>Sprite which is drawn to represent the entity</summary>
        protected Sprite image;

        /// <summary>Contains the collision logic for the entity</summary>
        protected CollisionUnit mCollisionUnit;

        protected PhysicsBaby mPhysBaby;

        /// <summary> ID for Checking the type of an Entity </summary>
        protected uint id;

        internal bool penabled, pcollision;
        internal object[] args;

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

        /// <summary>
        /// Get or set if the Entity component is enabled or not. If the entity is enabled then the following functions are called: Update, HandleInput.
        /// </summary>
        public bool Enabled { get; set; }

        public virtual void Initialize()
        {
            this.SetUpCollision();
        }

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

        /// <summary>
        /// Called once during each step taken by the engine
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public virtual void Update(GameTime gameTime)
        {
            image.Update(gameTime);
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

            PhysicsPharaoh.GetInstance().AddCollisionUnit(unit);
        }

        // call after image.LoadContent()
        protected virtual void SetUpCollision()
        {
            int radius = Math.Max(image.Width, image.Height) / 2;
            SetCollisionUnit(new SGDE.Physics.Collision.CollisionUnit(this, image.Center, radius, null, false));
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

        /// <summary>
        /// Set the entity's tint.
        /// </summary>
        /// <param name="backColor">The tint to use.</param>
        [Obsolete("Use {this}.SpriteImage.Tint instead")]
        public void SetColor(Color backColor)
        {
            image.Tint = backColor;
        }

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
            this.image.CopySpriteToIn(ref ent.image);
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

        public uint GetID()
        {
            return id;
        }

        public bool CompareID(Entity e)
        {
            return (e.id == id);
        }
    }
}
