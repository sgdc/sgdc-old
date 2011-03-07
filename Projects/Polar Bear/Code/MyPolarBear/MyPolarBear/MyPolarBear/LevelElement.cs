using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear
{
    public class LevelElement
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

            CollisionRect = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);

            //if (type.Equals("Lake"))
            //{
            //    CollisionRect.Width = 50;
            //    CollisionRect.Height = 50;
            //}
        }
    }
}
