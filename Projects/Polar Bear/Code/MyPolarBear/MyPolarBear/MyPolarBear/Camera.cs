using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear
{
    public class Camera
    {                                                                 

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        private float _scale; 
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        private Matrix _transformMatrix;   
        public Matrix TransformMatrix
        {            
            get { return _transformMatrix; }
            set { _transformMatrix = value; }
        }        

        private Vector2 _screenCenter;
        public Vector2 ScreenCenter
        {
            get { return _screenCenter; }
            set { _screenCenter = value; }
        }

        private Vector2 _origin;
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        private Entity _focusEntity;
        public Entity FocusEntity
        {
            get { return _focusEntity; }
            set { _focusEntity = value; }
        }

        private bool _isFocused = false;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { _isFocused = value; }
        }

        public Vector2 TopLeft;

        public GraphicsDevice _graphics;

        public Camera(GraphicsDevice graphics, bool isFocused)
        {
            Scale = 1;
            Rotation = 0;
            ScreenCenter = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);
            TopLeft = Vector2.Zero;
            Position = new Vector2(ScreenCenter.X, ScreenCenter.Y);
            TransformMatrix = Matrix.Identity;
            _graphics = graphics;
            IsFocused = isFocused;            
        }

        public void Update()
        {
            Origin = ScreenCenter / Scale;          

            TransformMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                              Matrix.CreateRotationZ(Rotation) *
                              Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                              Matrix.CreateScale(Scale);

            TopLeft.X = Position.X - _graphics.Viewport.Width / 2;
            TopLeft.Y = Position.Y - _graphics.Viewport.Height / 2;
        }

        public void FollowEntity()
        {
            if (IsFocused)
                Position = FocusEntity.Position;
        }

        public void ShowMenu()
        {
            Position = Vector2.Zero;
        }

        public void Translate(Vector2 amount)
        {
            Position += amount;
        }

        public void Rotate(float amount)
        {
            Rotation += amount;
        }

        public void Zoom(float amount)
        {
            Scale += amount;
        }

        /*  How to Use:
         * 
         *  Declare:
         *  Camera camera;
         *  
         *  Initialize:
         *  camera = new Camera(GraphicsDevice.Viewport);
         *  
         *  Update:
         *  camera.Update();
         *  
         *  Draw:
         *  Pass in the TransformMatrix to the spriteBatch.Begin function:
         *  spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.TransformMatrix);
         * 
         *  Camera can be moved independently or can follow the player by setting IsFocused to true. 
         *  Only implemented for 2D since none of our games will be in 3D.
         */
    }
}
