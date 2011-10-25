using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.Audio;

namespace MyPolarBear.AI
{
    class AfraidAI : AIComponent
    {
        private Entity mEnt;
        private int fearTimer;
        private Random random = new Random();

        public AfraidAI(Entity ent)
            :base()
        {
            mEnt = ent;
        }

        public override void DoAI(GameTime gameTime)
        {
            CurrentState = State.Good;

            if (fearTimer <= 0)
            {
                mEnt.Velocity.Normalize();
                mEnt.Velocity = new Vector2(random.Next(-4, 4), random.Next(-4, 4));
            }

            fearTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (fearTimer > 2000)
            {
                fearTimer = 0;
                //return false;
                CurrentState = State.Done;
                return;
            }

            if (fearTimer % 10 == 0)
            {
                mEnt.Velocity = new Vector2(random.Next(-4, 4), random.Next(-4, 4));
            }

            //return true;
            //CurrentState = State.Good;
        }

    }
}
