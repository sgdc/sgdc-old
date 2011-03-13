using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SGDE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE.Physics.Collision;

namespace Tyrus_and_Randall
{
    class Food : Entity
    {
        public enum FOOD { STAR }
        private byte food;
        private Boolean enabled;

        public Food()
            : this(0.0f, 0.0f)
        { }

        public Food(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public Food(float x, float y) :base(x,y)
        {
            Random randy = new Random();
            food = (byte)randy.Next(0);
            SetTranslation(new Vector2(x, y));
            enabled = true;

            //this.mCollisionUnit.SetSolid(false);

			id = 3;
        }

        public override void CollisionChange()
        {
            foreach (CollisionUnit other in GetCollisionUnit().GetCollisions())
            {
                if (((Entity)(other.GetParent())).GetID() != 1 && ((Entity)(other.GetParent())).GetID() != 3)
                {
                    SetVelocity(Vector2.Zero);
                    this.EnablePhysics(false, true);
                    break;
                }
            }
        }

        public void Disable()
        {
            this.SpriteImage.Visible = false;
            enabled = false;
        }

        public void Enable()
        {
            this.SpriteImage.Visible = true;
            enabled = true;
        }

        public Boolean IsEnabled()
        {
            return enabled;
        }
    }
}
