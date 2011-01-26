using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SGDE;
using SGDE.Physics.Collision;
using SGDE.Graphics;

namespace TestDemo
{
    public class BounceBall : Entity
    {
        private List<CollisionUnit> mPrevCheckedUnits;
        private Vector2 mHitLocation;

        public bool HandleCollisions { get; set; }

#if WINDOWS
        public BounceBall(int x = 0, int y = 0)
#else
        public BounceBall()
            : this(0, 0)
        {
        }

        public BounceBall(int x, int y)
#endif
            : base(x, y)
        {
            mPrevCheckedUnits = new List<CollisionUnit>();
            mHitLocation = new Vector2(0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (mCollisionUnit != null && HandleCollisions)
            {
                //foreach (CollisionUnit other in mPrevCheckedUnits)
                //{
                //    other.GetOwner().SetColor(Color.White);
                //}

                mPrevCheckedUnits = new List<CollisionUnit>(mCollisionUnit.GetCheckedUnits());

                //foreach (CollisionUnit other in mPrevCheckedUnits)
                //{
                //    other.GetOwner().SetColor(Color.Green);
                //}

                if (mCollisionUnit.HasCollisions())
                {
                    //Velocity(Velocity().X * -1, Velocity().Y * -1);

                    foreach (CollisionUnit other in mCollisionUnit.GetCollisions())
                    {
                        //other.GetOwner().SetColor(Color.Pink);
                        mHitLocation = GetCollisionUnit().GetCollisionPoint(other);
                    }
                }
            }

            base.Update(gameTime);
        }

        public void DrawHitSpot(SpriteBatch batch, Texture2D hitTexture)
        {
            Vector2 drawSpot = new Vector2();
            drawSpot.X = mHitLocation.X - (hitTexture.Width / 2);
            drawSpot.Y = mHitLocation.Y - (hitTexture.Height / 2);

            batch.Draw(hitTexture, drawSpot, Color.White);
        }

        public override void CollisionChange()
        {
            if (!mPhysBaby.IsStatic())
            {
                if (HandleCollisions)
                {
                    Game1 game1 = (Game1)SGDE.Game.CurrentGame;
                    game1.mySoundQueue.PlaySoundEffect(game1.ping);
                }
                foreach (CollisionUnit other in mCollisionUnit.GetCollisions())
                {
                    if (other.IsSolid())
                    {
                        mPhysBaby.AddBounce(mCollisionUnit, other);
                    }
                }
            }
        }
    }
}
