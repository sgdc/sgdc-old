using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE
{
    /// <summary>
    /// Camera control class.
    /// </summary>
    public sealed class Camera
    {
        private Vector2 _position;
        private float _rotation;
        private float _scale;        
        internal Matrix _transformMatrix;        
        private Vector2 _origin;
        private Vector2 _screenCenter;        

        /// <summary>
        /// Get or set the position of the camera.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Get or set the rotation of the camera in radians.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Get or set the scale of the camera.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Get the transform matrix of the camera.
        /// </summary>
        public Matrix TransformMatrix
        {
            get { return _transformMatrix; }
        }

        internal Camera(Viewport viewport)
        {
            _scale = 1;
            _rotation = 0;
            _screenCenter = new Vector2(viewport.Width / 2, viewport.Height / 2);
            _position = new Vector2(_screenCenter.X, _screenCenter.Y);
        }

        internal void Update(GameTime gameTime)
        {
            _origin = _screenCenter / _scale;

            _transformMatrix = Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
                               Matrix.CreateRotationZ(_rotation) *
                               Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
                               Matrix.CreateScale(_scale);

            /* 
             *  Set the camera's position to the player's position in order to focus on and follow him.
             */
        }

        /*  How to Use: //It's all handled in game
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
