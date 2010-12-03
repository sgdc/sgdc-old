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
    public abstract partial class Entity : SceneNode
    {
        /// <summary>Sprite which is drawn to represent the entity</summary>
        protected Sprite image;

        /// <summary>Contains the collision logic for the entity</summary>
        protected CollisionUnit mCollisionUnit;

        protected PhysicsBaby mPhysBaby;

        /// <summary>Object which allows for keyboard events. Only instantiate if object responds to keyboard input.</summary>
        protected KeyboardComponent keyboardListener;

        internal bool penabled, pcollision;
        internal object[] args;

        /// <summary>
        /// Constructs a new entity in the scene
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Entity(float x = 0, float y = 0)
        {
            SetRotation(0);
            mPhysBaby = new PhysicsBaby(this);
            SetTranslation(new Vector2(x, y));
        }

        /// <summary>
        /// Constructs a new entity in the scene
        /// </summary>
        /// <param name="position">Coordinates</param>
        public Entity(Vector2 position)
            : this(position.X, position.Y)
        {
        }

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

        /*
        public virtual void LoadContent(ContentManager theContentManager, String theAssetName)
        {
            //int radius;

            image = theContentManager.Load<Sprite>(theAssetName);
            AddChild(image);

            //radius = Math.Max(image.GetWidth(), image.GetHeight()) / 2;
            //mCollisionUnit = new CollisionUnit(this, image.GetCenter(), radius, null, false);
            //AddChild(mCollisionUnit);

            SetUpCollision();
        }
         */

        // call after image.LoadContent()
        protected virtual void SetUpCollision()
        {
            int radius = Math.Max(image.Width, image.Height) / 2;
            SetCollisionUnit(new SGDE.Physics.Collision.CollisionUnit(this, image.Center, radius, null, false));
        }

        /// <summary>Draws the entity to the screen</summary>
        public virtual void Draw(GameTime gameTime)
        {
            image.Draw(gameTime);
        }

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

        public void HandleInput(KeyboardState keyboardState, ContentManager content)
        {
            keyboardListener.HandleEvents(keyboardState, content);
        }

        /// <summary>
        /// Used internally
        /// </summary>
        internal Sprite SpriteImage
        {
            get
            {
                return this.image;
            }
            set
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
            ent.SpriteImage = new Sprite();
            ent.image.baseTexture = this.image.baseTexture;
            ent.image.animation = this.image.animation;
            ent.image.overrideAtt = this.image.overrideAtt;
            ent.image.FPS = this.image.FPS;
            ent.image.Tint = this.image.Tint;
            SceneNode nNode = ent.SpriteImage;
            node.CopyTo(ref nNode);
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
    }
}
