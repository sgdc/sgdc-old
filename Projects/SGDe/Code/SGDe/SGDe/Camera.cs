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
    public sealed class Camera : SceneNode
    {
        internal Vector2 _lastTrans;
        private Vector2 _position;
        internal Matrix _transformMatrix;
        private Vector2 _origin;
        private Vector2 _screenCenter;
        private Vector4 _region;
#if XBOX
        private Vector2 _safeOrigin; //On a television the screen might extend off the actual screen. Use this to adjust the region so it will always be visible
#endif

        private void UpdatePosition()
        {
            if (this._region.X == float.PositiveInfinity && this._region.Y == float.PositiveInfinity && this._region.Z == float.NegativeInfinity && this._region.W == float.NegativeInfinity)
            {
                //Free movement, do whatever
                _position = mTranslation;
            }
            else
            {
                if (this._region.X == float.PositiveInfinity && this._region.Z == float.NegativeInfinity)
                {
                    //Free horizontal movement
                    _position.X = mTranslation.X;
                }
                else
                {
                    if ((this._region.X != float.PositiveInfinity) && 
#if XBOX
                        (mTranslation.X + _screenCenter.X + this._safeOrigin.X > this._region.X))
#else
                        (mTranslation.X + _screenCenter.X > this._region.X))
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
                        _position.X = mTranslation.X;
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
                    _position.Y = mTranslation.Y;
                }
                else
                {
                    if ((this._region.Y != float.PositiveInfinity) && 
#if XBOX
                        (mTranslation.Y + _screenCenter.Y + this._safeOrigin.Y > this._region.Y))
#else
                        (mTranslation.Y + _screenCenter.Y > this._region.Y))
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
                        _position.Y = mTranslation.Y;
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
        /// Translate the camera.
        /// </summary>
        /// <param name="translation">The translation for the camera.</param>
        public override void Translate(Vector2 translation)
        {
            this._lastTrans = translation;
            if (translation != Vector2.Zero)
            {
                base.Translate(translation);
                UpdatePosition();
            }
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
            _transformMatrix = Matrix.Identity;
            _screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
#if XBOX
            _safeOrigin = new Vector2(viewport.X, viewport.Y);
#endif
            mTranslation = new Vector2(_screenCenter.X, _screenCenter.Y);
            _region = new Vector4(float.PositiveInfinity); //X=Pos-X, Y=Pos-Y, Z=Neg-X, W=Neg-Y
            _region.Z = -_region.Z;
            _region.W = -_region.W;
            UpdatePosition();
        }

        internal void Update(GameTime gameTime)
        {
            _origin = _screenCenter / mScale;

            /*
            _transformMatrix = Matrix.CreateTranslation(-_position.X, -_position.Y, 0) *
                               Matrix.CreateRotationZ(_rotation) *
                               Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
                               Matrix.CreateScale(_scale);
             */
            float cos = (float)Math.Cos(mRotation);
            float sin = (float)Math.Sin(mRotation);
            _transformMatrix.M11 = cos * mScale.X;
            _transformMatrix.M12 = sin * mScale.Y;
            _transformMatrix.M21 = -sin * mScale.X;
            _transformMatrix.M22 = cos * mScale.Y;
            _transformMatrix.M33 = 0;
            _transformMatrix.M41 = ((-_position.X * cos) + (_position.Y * sin) + _origin.X) * mScale.X;
            _transformMatrix.M42 = ((-_position.X * sin) + (-_position.Y * cos) + _origin.Y) * mScale.Y;

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

        m11 = xScale
        m22 = yScale
        m33 = 0

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
        m11 = cos * xScale
        m12 = sin * yScale
        m21 = -sin * xScale
        m22 = cos * yScale
        m33 = 0
        m41 = ((-px * cos) + (-py * -sin) + ox) * xScale
        m42 = ((-px * sin) + (-py * cos) + oy) * yScale
         */
    }
}
