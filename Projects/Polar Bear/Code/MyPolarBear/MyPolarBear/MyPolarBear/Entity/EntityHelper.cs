using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear
{
    static class EntityHelper
    {
        public static Vector2 LevelBounce(Rectangle EntityCollisionBox, Rectangle LevelCollisionBox, float Bounciness)
        {
            if (!LevelCollisionBox.Contains(EntityCollisionBox))
            {
                if (EntityCollisionBox.Right > LevelCollisionBox.Right)
                {
                    return new Vector2(-1, 1) * Bounciness;
                }
                else if (EntityCollisionBox.Left < LevelCollisionBox.Left)
                {
                    return new Vector2(-1, 1) * Bounciness;
                }
                else if (EntityCollisionBox.Top < LevelCollisionBox.Top)
                {
                    return new Vector2(1, -1) * Bounciness;
                }
                else if (EntityCollisionBox.Bottom > LevelCollisionBox.Bottom)
                {
                    return new Vector2(1, -1) * Bounciness;
                }
                else
                {
                    return Vector2.One;
                }
            }
            else
            {
                return Vector2.One;
            }
        }

        public static Vector2 WallBounce(Rectangle EntityCollisionBox, Rectangle WallCollisionBox, float Bounciness)
        {
            if (WallCollisionBox.Intersects(EntityCollisionBox))
            {                
                if (WallCollisionBox.Top < EntityCollisionBox.Bottom)
                {
                    return new Vector2(1, -1) * Bounciness;
                }
                else if (WallCollisionBox.Bottom > EntityCollisionBox.Top)
                {
                    return new Vector2(1, -1) * Bounciness;
                }
                else if (WallCollisionBox.Left < EntityCollisionBox.Right)
                {
                    return new Vector2(-1, 1) * Bounciness;
                }
                else if (WallCollisionBox.Right > EntityCollisionBox.Left)
                {
                    return new Vector2(-1, 1) * Bounciness;
                }
                else
                {
                    return Vector2.One;
                }
            }
            else
            {
                return Vector2.One;
            }           
        }

        public static float AngleBetween(Vector2 firstVector, Vector2 secondVector)
        {
            double y = (double)(firstVector.Y - secondVector.Y);
            double x = (double)(firstVector.X - secondVector.X);

            return (float)Math.Atan2(x, y);
        }

        public static float DistanceBetween(Vector2 firstVector, Vector2 secondVector)
        {
            Vector2 result = firstVector - secondVector;
            return result.Length();
        }

        public static Vector2 MoveBackOnScreen(Rectangle EntityCollisionBox, Rectangle ScreenCollisionBox)
        {
            Vector2 delta = Vector2.Zero;

            if (EntityCollisionBox.Right > ScreenCollisionBox.Right)
            {
                delta.X = ScreenCollisionBox.Right - EntityCollisionBox.Right;
            }
            if (EntityCollisionBox.Left < ScreenCollisionBox.Left)
            {
                delta.X = ScreenCollisionBox.Left - EntityCollisionBox.Left;
            }
            if (EntityCollisionBox.Top < ScreenCollisionBox.Top)
            {
                delta.Y = ScreenCollisionBox.Top - EntityCollisionBox.Top;
            }
            if (EntityCollisionBox.Bottom > ScreenCollisionBox.Bottom)
            {
                delta.Y = ScreenCollisionBox.Bottom - EntityCollisionBox.Bottom;
            }

            return delta;
        }

        /// <summary>
        /// Returns a vector at the center of the given texture.
        /// </summary>
        /// <param name="texture">The texture to find the origin of.</param>
        /// <returns></returns>
        public static Vector2 OriginFromTexture(Texture2D texture)
        {
            return new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
        }

        public static Rectangle CollisionBoxFromTexture(Vector2 position, Texture2D texture, Vector2 origin, float scale)
        {
            return new Rectangle((int)(position.X - (origin.X * scale)), (int)(position.Y - (origin.Y * scale)),
                (int)(texture.Width * scale), (int)(texture.Height * scale));
        }

        public static Rectangle UpdateCollisionBox(Rectangle collisionBox, Vector2 position, Vector2 origin, float scale)
        {
            collisionBox.X = (int)(position.X - (origin.X * scale));
            collisionBox.Y = (int)(position.Y - (origin.Y * scale));

            return collisionBox;
        }
    }
}
