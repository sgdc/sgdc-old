using System;
using System.Collections;
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
        public uint currentLevel;
        private Boolean onGround;
        public enum PlayerDirection { Right, Left, StandingRight, StandingLeft };
        private PlayerDirection dir;
        private Boolean knockBack;
        private double knockBackTimer;
        private const double knockBackStagger = 500;
        private Boolean staggered;
        private const double knockBackMax = 3000;
        private static readonly uint[] levelFood = new uint[] {0, 20, 30};
        private ArrayList collectedFood;
        public float jumpMultiplier;

        public Player() : this(0, 0) { }

        public Player(Vector2 position) : this(position.X, position.Y) { }

        public Player(float x = 0, float y = 0)
            : base(x, y)
        {
            onGround = false;
            id = 1;
            knockBackTimer = 0;
            knockBack = false;
            staggered = false;
            collectedFood = new ArrayList();
            jumpMultiplier = 1.0f;
            currentLevel = 1;
        }

        public override void Initialize()
        {
 	        base.Initialize();
        }

        public override void  Update(GameTime gameTime)
        {
 	        base.Update(gameTime);

            if (GetTranslation().Y > 480)
            {
                SetTranslation(new Vector2(288, 384));
                this.knockBack = true;
                //staggered = true;

                if (collectedFood.Count > 0)
                {
                    collectedFood.RemoveAt(collectedFood.Count - 1);
                    if (collectedFood.Count > 0)
                        collectedFood.RemoveAt(collectedFood.Count - 1);
                }
            }
            
            Game1.foodText = "Food: " + collectedFood.Count + " / " + levelFood[currentLevel];

            SGDE.Game.CurrentGame.CameraControl.Position = this.GetTranslation();

            if (knockBack)
            {
                knockBackTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (staggered)
                {
                    switch (dir)
                    {
                        case PlayerDirection.StandingLeft:
                        case PlayerDirection.Left:
                            this.Translate(6, 0);
                            break;
                        case PlayerDirection.StandingRight:
                        case PlayerDirection.Right:
                            this.Translate(-6, 0);
                            break;
                    }
                }
                if (staggered && knockBackTimer > knockBackStagger)
                {
                    staggered = false;
                }
                if (knockBackTimer > knockBackMax)
                {
                    knockBackTimer = 0;
                    knockBack = false;
                }
            }

        }

        public void HandleInput(SGDE.Game game, InputComponent input)
        {
            SGDE.Input.Keyboard keyboard = (SGDE.Input.Keyboard)input;

            if (!staggered)
            {
                if (keyboard.IsKeyPressed(Keys.Left))
                {
                    this.Translate(-5, 0);
                    if (dir != PlayerDirection.Left)
                    {
                        if (!knockBack)
                            this.SpriteImage.SetAnimation("WalkLeft");
                        else
                            this.SpriteImage.SetAnimation("KnockBackLeft");
                        dir = PlayerDirection.Left;
                    }
                }
                else if (keyboard.IsKeyPressed(Keys.Right))
                {
                    this.Translate(5, 0);
                    if (dir != PlayerDirection.Right)
                    {
                        if (!knockBack)
                            this.SpriteImage.SetAnimation("WalkRight");
                        else
                            this.SpriteImage.SetAnimation("KnockBackRight");
                        dir = PlayerDirection.Right;
                    }
                }
                else
                {
                    this.SetVelocity(0, this.GetVelocity().Y);
                    switch (dir)
                    {
                        case PlayerDirection.Right:
                            if (!knockBack)
                                this.SpriteImage.SetAnimation("StandRight");
                            else
                                this.SpriteImage.SetAnimation("KnockBackRight");
                            dir = PlayerDirection.StandingRight;
                            break;
                        case PlayerDirection.Left:
                            if (!knockBack)
                                this.SpriteImage.SetAnimation("StandLeft");
                            else
                                this.SpriteImage.SetAnimation("KnockBackLeft");
                            dir = PlayerDirection.StandingLeft;
                            break;
                    }
                }

                if (keyboard.IsKeyPressed(Keys.Up))
                {
                    if (onGround && this.GetCollisionUnit().HasCollisions())
                    {
                        this.SetVelocity(this.GetVelocity().X, -10.0f*jumpMultiplier);
                        onGround = false;
                    }
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
                    if (other.IsSolid())
                    {
                        Vector2 intersect = GetIntersectionRectangle(this.GetCollisionUnit(), other);

                        switch (((Entity)other.GetParent()).GetID())
                        {
                            case 3:
                                if (((Food)other.GetParent()).IsEnabled())
                                {
                                    ((Food)(other.GetParent())).Disable();
                                    collectedFood.Add(((Food)(other.GetParent())));
                                }

                                break;
                            case 4:
                                if(!knockBack)
                                {
                                    knockBack = true;
                                    staggered = true;
                                    for (int i = collectedFood.Count-1, j = 0; i >= 0 && j < 3; i--, j++)
                                    {
                                        ((Food)collectedFood[i]).Enable();
                                        //((Food)collectedFood[i]).SetTranslation(new Vector2(this.GetTranslation().X - 33*(i+0.5f), this.GetTranslation().Y - 33*(i+0.5f)));
                                        ((Food)collectedFood[i]).EnablePhysics(true, true);
                                        if (((Food)collectedFood[i]).GetPhysicsBaby().GetForces().Y > 20)
                                            ((Food)collectedFood[i]).GetPhysicsBaby().AddForce(new Vector2(0, -20));
                                        switch (dir)
                                        {
                                            case PlayerDirection.StandingLeft:
                                            case PlayerDirection.Left:
                                                SpriteImage.SetAnimation("KnockBackLeft");
                                                ((Food)collectedFood[i]).SetVelocity(2*(j + 1), -3f * (j+1));
                                                ((Food)collectedFood[i]).SetTranslation(new Vector2(this.GetTranslation().X + 33, this.GetTranslation().Y - 33 ));
                                                break;
                                            case PlayerDirection.StandingRight:
                                            case PlayerDirection.Right:
                                                SpriteImage.SetAnimation("KnockBackRight");
                                                ((Food)collectedFood[i]).SetVelocity(-2*(j + 1), -3f * (j+1));
                                                ((Food)collectedFood[i]).SetTranslation(new Vector2(this.GetTranslation().X - 33, this.GetTranslation().Y - 33));
                                                break;
                                        }
                                        //((Food)collectedFood[i]).SetVelocity((float)-0.5*(i+1), (float)-0.5*i);
                                        collectedFood.RemoveAt(i);
                                    }
                                }
                                break;
                            default:
                                if (Math.Abs(intersect.X) > Math.Abs(intersect.Y) && !intersect.Equals(Vector2.Zero))
                                {
                                    if (intersect.Y <= 0)
                                    {
                                        this.SetVelocity(this.GetVelocity().X, Math.Min(this.GetVelocity().Y, 0));
                                        this.SetTranslation(new Vector2(this.GetTranslation().X, this.GetTranslation().Y + intersect.Y - 0.01f));
                                        onGround = true;
                                    }
                                    else
                                    {
                                        this.SetVelocity(this.GetVelocity().X, Math.Max(this.GetVelocity().Y, 0));
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

                                break;
                        }
                    }
                    else
                    {
                        if(((Entity)(other.GetParent())).GetID() == 8)
                            ((Powerup)(other.GetParent())).Activate(this);
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
            //if (cam.Y < screen.Height/2) cam.Y = screen.Height/2;
            //if (cam.Y > screen.Height/2) cam.Y = screen.Height/2;
            if (cam.Y != screen.Height/2) cam.Y = screen.Height/2;
            return cam;
        }

        public int TotalFood
        {
            get
            {
                return collectedFood.Count;
            }
        }

        public void DumpFood()
        {
            collectedFood.Clear();
        }

    }
}