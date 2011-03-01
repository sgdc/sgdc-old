using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE;
using SGDE.Physics.Collision;
using SGDE.Input;

namespace Tyrus_and_Randall
{
    class Player : Entity, InputHandler
    {

        private Boolean onGround;
        private int totalFood;

        public Player() : this(0, 0) { }

        public Player(Vector2 position) : this(position.X, position.Y) { }

        public Player(float x = 0, float y = 0)
            : base(x, y)
        {
            onGround = false;
            id = 1;
            totalFood = 0;
        }

        public override void Initialize()
        {
 	        base.Initialize();
        }

        public override void  Update(GameTime gameTime)
        {
 	        base.Update(gameTime);
            Game1.CurrentGame.CameraControl.Position = this.CameraBound();
        }

        public void HandleInput(SGDE.Game game, InputComponent input)
        {
            SGDE.Input.Keyboard keyboard = (SGDE.Input.Keyboard)input;

            if (keyboard.IsKeyPressed(Keys.Left))
                this.Translate(-5, 0);
            if (keyboard.IsKeyPressed(Keys.Right))
                this.Translate(5, 0);
            if (keyboard.IsKeyPressed(Keys.Up))
            {
                if (onGround && this.GetCollisionUnit().HasCollisions())
                {
                    this.SetVelocity(this.GetVelocity().X, -8.0f);
                    onGround = false;
                }
            }
            if (keyboard.IsKeyPressed(Keys.Escape))
                game.Exit();
        }

        public InputType Handles
        {
            get { return InputType.Keyboard; }
        }

        public PlayerIndex Index
        {
            get { return Microsoft.Xna.Framework.PlayerIndex.One; }
        }

        public bool IndexSpecific
        {
            get { return false; }
        }
       protected override void SetUpCollision()
        {
            SetCollisionUnit(new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.Width, image.Height), CollisionUnit.CollisionType.COLLISION_BOX, null, false));
            this.mCollisionUnit.SetSolid(true);
            //this.mPhysBaby.AddCollisionUnit(this.mCollisionUnit);
            //AddChild(GetCollisionUnit());
        }

        public override void CollisionChange()
        {
            onGround = false;
            if (!mPhysBaby.IsStatic())
            {
                foreach (CollisionUnit other in GetCollisionUnit().GetCollisions())
                {
                    if (other.IsSolid() && ((Entity)other.GetParent()).GetID() != 3 )// 3 = food
                    {
                        Vector2 intersect = GetIntersectionRectangle(this.GetCollisionUnit(), other);

                        if (Math.Abs(intersect.X) > Math.Abs(intersect.Y) && !intersect.Equals(Vector2.Zero))
                        {
                            this.SetVelocity(this.GetVelocity().X, 0.0f);
                            if (intersect.Y <= 0)
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X, this.GetTranslation().Y + intersect.Y - 0.01f));
                                onGround = true;
                            }
                            else
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X, this.GetTranslation().Y + intersect.Y + 0.01f));
                            }
                        }
                        else if (!intersect.Equals(Vector2.Zero))
                        {
                            this.SetVelocity(0.0f, this.GetVelocity().Y);
                            if (intersect.X <= 0)
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X + intersect.X - 0.01f, this.GetTranslation().Y));
                            }
                            else
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X + intersect.X + 0.01f, this.GetTranslation().Y));
                            }
                        }
                    }
                    else if (((Entity)other.GetParent()).GetID() == 3)// 3 = food
                    {
                        totalFood++;
                        ((Food)(other.GetParent())).Disable();
                    }
                }

            }
        }

        protected static Vector2 GetIntersectionRectangle(CollisionUnit A, CollisionUnit B)
        {
            // Calculate half sizes.
            float halfWidthA = (A.GetLowerRight().X - A.GetUpperLeft().X) / 2.0f;
            float halfHeightA = (A.GetLowerRight().Y - A.GetUpperLeft().Y) / 2.0f;
            float halfWidthB = (B.GetLowerRight().X - B.GetUpperLeft().X) / 2.0f;
            float halfHeightB = (B.GetLowerRight().Y - B.GetUpperLeft().Y) / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(A.GetUpperLeft().X + halfWidthA, A.GetUpperLeft().Y + halfHeightA);
            Vector2 centerB = new Vector2(B.GetUpperLeft().X + halfWidthB, B.GetUpperLeft().Y + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        private Vector2 CameraBound()
        {
            Rectangle screen = Game1.CurrentGame.Window.ClientBounds;
            Vector2 cam = this.GetTranslation();
            if (cam.X < screen.Width/2) cam.X = screen.Width/2;
            if (cam.Y < screen.Height/2) cam.Y = screen.Height/2;
            return cam;
        }
    }
}