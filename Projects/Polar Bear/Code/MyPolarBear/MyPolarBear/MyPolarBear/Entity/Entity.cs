using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear
{
    public class Entity
    {
        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private Rectangle _collisionBox;
        public Rectangle CollisionBox
        {
            get { return _collisionBox; }
            set { _collisionBox = value; }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector2 _velocity;
        public Vector2 Velocity 
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        private Vector2 _origin;
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        private Vector2 _direction;
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private Vector2 _angle;
        public Vector2 Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        private float _scale;
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        private float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 delta;
        public Vector2 bounciness;
        //public int aniFrame;

        public Entity(Vector2 position)
        { 
            Position = position;
            Scale = 1.0f;
        }

        public virtual void Initialize() { }

        public virtual void LoadContent(Texture2D texture, float scale)
        {
            Texture = texture;
            Scale = scale;
            Origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
            CollisionBox = EntityHelper.CollisionBoxFromTexture(Position, Texture, Origin, Scale);
            Color = Color.White;
            //aniFrame = 0;
        }

        public virtual void LoadContent()
        {
            if (Texture != null)
            {
                Origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
                CollisionBox = EntityHelper.CollisionBoxFromTexture(Position, Texture, Origin, Scale);
            }
            Color = Color.White;
        }

        public virtual void Update(GameTime gameTime)
        {
            CollisionBox = EntityHelper.UpdateCollisionBox(CollisionBox, Position, Origin, Scale);
        }

        public void HandleCollisions(Rectangle levelCollisionBox, Rectangle entityCollisionBox)
        {            
            bounciness = EntityHelper.LevelBounce(CollisionBox, levelCollisionBox, 0.5f);
            delta = EntityHelper.MoveBackOnScreen(CollisionBox, levelCollisionBox);           
        }

        public void Translate(Vector2 amount)
        {
            Position += amount;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            //int rectWidth = Texture.Width;
            //int rectHeight = Texture.Height / 4;
            //Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 4), rectWidth, rectHeight);
            //Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            //spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
            //aniFrame++;
            //aniFrame %= 16;
        }              
    }
}
