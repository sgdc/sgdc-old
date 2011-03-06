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
    class HutTop : Entity
    {
        private static bool allvisible = true;

        public HutTop()
            : this(0.0f, 0.0f)
        { }

        public HutTop(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public HutTop(float x, float y)
        {
            id = 6;
        }

        public override void Update(GameTime gameTime)
        {
            if (allvisible != SpriteImage.Visible)
                SpriteImage.Visible = allvisible;
            base.Update(gameTime);
        }

        public static void SetAllVisible(bool flag)
        {
            if(flag != allvisible)
                allvisible = flag;
        }
    }
}
