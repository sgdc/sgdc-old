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
        private Vector2 _rawPosition;
        private float _rotation;
        private float _scale;
        internal Matrix _transformMatrix;
        private Vector2 _origin;
        private Vector2 _screenCenter;
        private Vector4 _region;
#if XBOX
        private Vector2 _safeOrigin; //On a television the screen might extend off the actual screen. Use this to adjust the region so it will always be visible
#endif

        /// <summary>
        /// Get or set the position of the camera, located at the center of the screen.
        /// </summary>
        public Vector2 Position
        {
            get { return _rawPosition; }
            set
            {
                if (value != _rawPosition)
                {
                    _rawPosition = value;
                    UpdatePosition();
                }
            }
        }

        private void UpdatePosition()
        {
            if (this._region.X == float.PositiveInfinity && this._region.Y == float.PositiveInfinity && this._region.Z == float.NegativeInfinity && this._region.W == float.NegativeInfinity)
            {
                //Free movement, do whatever
                _position = _rawPosition;
            }
            else
            {
                if (this._region.X == float.PositiveInfinity && this._region.Z == float.NegativeInfinity)
                {
                    //Free horizontal movement
                    _position.X = _rawPosition.X;
                }
                else
                {
                    if ((this._region.X != float.PositiveInfinity) && 
#if XBOX
                        (_rawPosition.X + _screenCenter.X + this._safeOrigin.X > this._region.X))
#else
                        (_rawPosition.X + _screenCenter.X > this._region.X))
#endif
                    {
#if XBOX
                        _position.X = this._region.X - (_screenCenter.X + this._safeOrigin.X);
#else
                        _position.X = this._region.X - _screenCenter.X;
#endif
                    }
                    else
                    {
                        _position.X = _rawPosition.X;
                    }
                    if ((this._region.Z != float.NegativeInfinity) && 
#if XBOX
                        (_position.X - _screenCenter.X - this._safeOrigin.X < this._region.Z))
#else
                        (_position.X - _screenCenter.X < this._region.Z))
#endif
                    {
#if XBOX
                        _position.X = this._region.Z + _screenCenter.X + this._safeOrigin.X;
#else
                        _position.X = this._region.Z + _screenCenter.X;
#endif
                    }
                }
                if (this._region.Y == float.PositiveInfinity && this._region.W == float.NegativeInfinity)
                {
                    //Free vertical movement
                    _position.Y = _rawPosition.Y;
                }
                else
                {
                    if ((this._region.Y != float.PositiveInfinity) && 
#if XBOX
                        (_rawPosition.Y + _screenCenter.Y + this._safeOrigin.Y > this._region.Y))
#else
                        (_rawPosition.Y + _screenCenter.Y > this._region.Y))
#endif
                    {
#if XBOX
                        _position.Y = this._region.Y - (_screenCenter.Y + this._safeOrigin.Y);
#else
                        _position.Y = this._region.Y - _screenCenter.Y;
#endif
                    }
                    else
                    {
                        _position.Y = _rawPosition.Y;
                    }
                    if ((this._region.W != float.NegativeInfinity) && 
#if XBOX
                        (_position.Y - _screenCenter.Y - this._safeOrigin.Y < this._region.W))
#else
                        (_position.Y - _screenCenter.Y < this._region.W))
#endif
                    {
#if XBOX
                        _position.Y = this._region.W + _screenCenter.Y + this._safeOrigin.Y;
#else
                        _position.Y = this._region.W + _screenCenter.Y;
#endif
                    }
                }
            }
        }

        /// <summary>
        /// Get or set the horizontal bounds on the XY plane. The origin point is the upper-left corner of the game window. X is positive, Y is negative. Infinity is supported, NaN values will be ignored.
        /// </summary>
        public Vector2 HorizontalBounds
        {
            get
            {
                return new Vector2(this._region.X, this._region.Z);
            }
            set
            {
                bool change = false;
                if (!float.IsNaN(value.X))
                {
                    if (this._region.X != value.X)
                    {
                        change = true;
                    }
                    this._region.X = value.X;
                }
                if (!float.IsNaN(value.Y))
                {
                    if (this._region.Z != value.Y)
                    {
                        change = true;
                    }
                    this._region.Z = value.Y;
                }
                if (change)
                {
                    UpdatePosition();
                }
            }
        }

        /// <summary>
        /// Get or set the vertical bounds on the XY plane. The origin point is the upper-left corner of the game window. X is positive, Y is negative. Infinity is supported, NaN values will be ignored.
        /// </summary>
        public Vector2 VerticalBounds
        {
            get
            {
                return new Vector2(this._region.Y, this._region.W);
            }
            set
            {
                bool change = false;
                if (!float.IsNaN(value.X))
                {
                    if (this._region.Y != value.X)
                    {
                        change = true;
                    }
                    this._region.Y = value.X;
                }
                if (!float.IsNaN(value.Y))
                {
                    if (this._region.W != value.Y)
                    {
                        change = true;
                    }
                    this._region.W = value.Y;
                }
                if (change)
                {
                    UpdatePosition();
                }
            }
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
            _screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
#if XBOX
            _safeOrigin = new Vector2(viewport.X, viewport.Y);
#endif
            _rawPosition = new Vector2(_screenCenter.X, _screenCenter.Y);
            _region = new Vector4(float.PositiveInfinity); //X=Pos-X, Y=Pos-Y, Z=Neg-X, W=Neg-Y
            _region.Z = -_region.Z;
            _region.W = -_region.W;
            UpdatePosition();
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
