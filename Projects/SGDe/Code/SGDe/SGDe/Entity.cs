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
using SGDe.Graphics;

namespace SGDE
{
    //class Sprite
    //{
    //}

    class CollisionObject
    {
    }

    public class Entity
    {
        private Point position;
        private Vector2 velocity;
        private ushort direction;
        private Sprite image = new Sprite();
        private CollisionObject colObj = new CollisionObject();

        public Entity()
        {
            position = new Point(0, 0);
            velocity = new Vector2(0.0f, 0.0f);
            direction = 0;
        }

        public Entity(int x, int y)
        {
            position = new Point(x, y);
            velocity = new Vector2(0.0f, 0.0f);
            direction = 0;
        }

        public void Position(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public Point Position()
        {
            return position;
        }

        public void Velocity(float x, float y)
        {
            velocity.X = x;
            velocity.Y = y;
        }

        public Vector2 Velocity()
        {
            return velocity;
        }

        public void move()
        {
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;
        }

        public void Direction(ushort angle)
        {
            direction = angle;
        }

        public ushort Direction()
        {
            return direction;
        }

        public void LoadContent(ContentManager theContentManager, String theAssetName)
        {
            image.LoadContent(theContentManager, theAssetName);
        }

        public void Draw()
        {
            image.DrawOffset(position.X, position.Y);
        }
    }
}
