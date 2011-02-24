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
            _transformMatrix = Matrix.Identity;
            _screenCenter = new Vector2(viewport.Width / 2, viewport.Height / 2);
            _position = new Vector2(_screenCenter.X, _screenCenter.Y);
        }

        internal void Update(GameTime gameTime)
        {
            _origin = _screenCenter / _scale;

            /*
            _transformMatrix = Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
                               Matrix.CreateRotationZ(_rotation) *
                               Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
                               Matrix.CreateScale(_scale);
             */
            float cos = (float)Math.Cos(_rotation);
            float sin = (float)Math.Sin(_rotation);
            _transformMatrix.M22 = _transformMatrix.M11 = cos * _scale;
            _transformMatrix.M21 = -(_transformMatrix.M12 = sin * _scale);
            _transformMatrix.M33 = _scale;
            _transformMatrix.M41 = ((-_position.X * cos) + (_position.Y * sin) + _origin.X) * _scale;
            _transformMatrix.M42 = ((-_position.X * sin) + (-_position.Y * cos) + _origin.Y) * _scale;

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

        /* Behind the optimization
        m11 = 1
        m22 = 1
        m33 = 1
        m44 = 1
        -------------------------------------

        m41 = -px
        m42 = -py

        -multiply

        m11 = cos
        m12 = sin
        m21 = -sin
        m22 = cos

        -multiply

        m41 = ox
        m42 = oy

        -multiply

        m11 = scale
        m22 = scale
        m33 = scale

        --------------------------------------
        --
        m11 = cos
        m12 = sin
        m21 = -sin
        m22 = cos
        m41 = (-px * cos) + (-py * -sin)
        m42 = (-px * sin) + (-py * cos)

        --------------------------------------
        --
        m11 = cos
        m12 = sin
        m21 = -sin
        m22 = cos
        m41 = (-px * cos) + (-py * -sin) + ox
        m42 = (-px * sin) + (-py * cos) + oy

        --------------------------------------
        --
        m11 = cos * scale
        m12 = sin * scale
        m21 = -sin * scale
        m22 = cos * scale
        m33 = scale
        m41 = ((-px * cos) + (-py * -sin) + ox) * scale
        m42 = ((-px * sin) + (-py * cos) + oy) * scale

        --------------------------------------
        m22 = m11 = cos * scale
        m21 = -(m12 = sin * scale)
        m33 = scale
        m41 = ((-px * cos) + (-py * -sin) + ox) * scale
        m42 = ((-px * sin) + (-py * cos) + oy) * scale
         */
    }
}
