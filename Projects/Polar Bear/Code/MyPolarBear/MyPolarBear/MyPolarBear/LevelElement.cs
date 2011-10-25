using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyPolarBear.Interfaces;
using MyPolarBear.GameScreens;
using MyPolarBear.Content;
using MyPolarBear.Pathfinding;

namespace MyPolarBear
{
    public class LevelElement : ITargetable, IDamageable
    {
        public Vector2 Position;
        public Rectangle CollisionRect;
        public String Type;
        public Texture2D Tex;

        public LevelElement(Vector2 position, String type, Texture2D tex)
        {
            Position = position;
            Type = type;
            Tex = tex;

            CollisionRect = new Rectangle((int)position.X, (int)position.Y, tex.Width, (int)(tex.Height * (3.0 / 4.0)));

            //if (type.Equals("Lake"))
            //{
            //    CollisionRect.Width = 50;
            //    CollisionRect.Height = 50;
            //}
        }

        public virtual String GetTargetType()
        {
            return Type;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public Rectangle GetCollisionRect()
        {
            return CollisionRect;
        }

        public void TakeDamage(int amount, String damageType, Entity source)
        {
            if (damageType.Equals("fire") && (Type.Equals("Stump") || Type.Equals("BabyPlant")))
            {
                if (Type.Equals("BabyPlant"))
                {
                    GameScreen.CurWorldHealth--;
                }

                Type = "SoftGround";
                Tex = ContentManager.GetTexture("SoftGround");
                AGrid.GetInstance().addResource(this);
            }

            if (damageType.Equals("ice") && Type.Equals("Water"))
            {
                Type = "Ice";
                Tex = ContentManager.GetTexture("Ice");
            }
        }
    }
}
