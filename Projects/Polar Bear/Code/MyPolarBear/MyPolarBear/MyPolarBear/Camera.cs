using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.GameScreens;

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

        public Vector2 GetPosition()
        {
            return _position;
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

        private Vector2 _topLeft;
        public Vector2 TopLeft
        {
            get { return _topLeft; }
            set { _topLeft = value; }       
        }

        private GraphicsDevice _graphicsDevice;
        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
            set { _graphicsDevice = value; }
        }

        public Camera(GraphicsDevice graphicsDevice, bool isFocused)
        {
            Scale = 1;
            Rotation = 0;
            ScreenCenter = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            TopLeft = Vector2.Zero;
            Position = new Vector2(ScreenCenter.X, ScreenCenter.Y);
            TransformMatrix = Matrix.Identity;
            GraphicsDevice = graphicsDevice;
            IsFocused = isFocused;            
        }

        public void Update()
        {
            Origin = ScreenCenter / Scale;          

            TransformMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                              Matrix.CreateRotationZ(Rotation) *
                              Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                              Matrix.CreateScale(Scale);

            TopLeft = new Vector2(Position.X - GraphicsDevice.Viewport.Width / 2, Position.Y - GraphicsDevice.Viewport.Height / 2);
        }

        public void FollowEntity()
        {
            if (IsFocused)            
                Position = FocusEntity.Position;

            float horizontalBoundary = MathHelper.Clamp(Position.X, -GameScreen.WORLDWIDTH / 2 + ScreenManager.SCREENWIDTH / 2, GameScreen.WORLDWIDTH / 2 - ScreenManager.SCREENWIDTH / 2);
            float verticalBoundary = MathHelper.Clamp(Position.Y, -GameScreen.WORLDHEIGHT / 2 + ScreenManager.SCREENHEIGHT / 2, GameScreen.WORLDHEIGHT / 2 - ScreenManager.SCREENHEIGHT / 2);

            Position = new Vector2(horizontalBoundary, verticalBoundary);
                
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
