using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.Audio;

namespace MyPolarBear.AI
{
    class AfraidAI
    {
        private Entity mEnt;
        private int fearTimer;
        private Random random = new Random();

        public AfraidAI(Entity ent)
        {
            mEnt = ent;
        }

        public bool DoAI(GameTime gameTime)
        {
            if (fearTimer <= 0)
            {
                mEnt.Velocity.Normalize();
                mEnt.Velocity = new Vector2(random.Next(-4, 4), random.Next(-4, 4));
            }

            fearTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (fearTimer > 2000)
            {
                fearTimer = 0;
                return false;
            }

            if (fearTimer % 10 == 0)
            {
                mEnt.Velocity = new Vector2(random.Next(-4, 4), random.Next(-4, 4));
            }

            return true;
        }

    }
}
