using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Demo
{
    class Camera
    {
        private Vector2 _position;
        private float _rotation;
        private float _scale;        
        private Matrix _transformMatrix;        
        private Vector2 _origin;
        private Vector2 _screenCenter;        

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public Matrix TransformMatrix
        {
            get { return _transformMatrix; }
        }               

        public Camera(Viewport viewport)
        {
            _scale = 1;
            _rotation = 0;
            _screenCenter = new Vector2(viewport.Width / 2, viewport.Height / 2);
        }

        public void Update()
        {
            _origin = _screenCenter / _scale;

            _transformMatrix = Matrix.Identity *
                               Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
                               Matrix.CreateRotationZ(_rotation) *
                               Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
                               Matrix.CreateScale(_scale);                        

            /* 
             *  Set the camera's position to the player's position in order to focus on and follow him.
             */
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
         *  Camera can be moved independently or can follow the player. 
         *  Only implemented for 2D since none of our games will be in 3D.
         */
    }
}
