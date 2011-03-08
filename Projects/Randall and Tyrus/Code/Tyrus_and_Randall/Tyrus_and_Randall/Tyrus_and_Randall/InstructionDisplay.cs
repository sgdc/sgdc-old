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
    class InstructionDisplay : Entity
    {
        public InstructionDisplay()
            : this(0.0f, 0.0f)
        { }

        public InstructionDisplay(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public InstructionDisplay(float x, float y)
        {
			id = 7;
        }

        public override void Update(GameTime gameTime)
        {
            if (Game1.levelonecomplete)
                SpriteImage.SetAnimation("WIN");
            base.Update(gameTime);
        }
    }
}
