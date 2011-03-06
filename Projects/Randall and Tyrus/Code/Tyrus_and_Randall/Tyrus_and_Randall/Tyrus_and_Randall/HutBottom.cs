using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE;
using SGDE.Physics.Collision;

namespace Tyrus_and_Randall
{
    class HutBottom : Entity
    {
        public HutBottom()
            : this(0.0f, 0.0f)
        { }

        public HutBottom(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public HutBottom(float x, float y)
        {
			id = 5;
        }

        public override void CollisionChange()
        {
            this.SpriteImage.Visible = true;
            HutTop.SetAllVisible(true);
            if(this.GetCollisionUnit().IsSolid())
                this.GetCollisionUnit().SetSolid(false);
            foreach (CollisionUnit other in GetCollisionUnit().GetCollisions())
            {
                if (((Entity)other.GetParent()).GetID() == 1)
                {
                    if (other.CollidesWith(this.GetCollisionUnit()))
                    {
                        this.SpriteImage.Visible = false;
                        HutTop.SetAllVisible(false);
                    }
                }
            }
            base.CollisionChange();
        }
    }
}
