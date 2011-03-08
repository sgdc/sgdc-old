using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.GameScreens;
using MyPolarBear.Input;
using MyPolarBear.Content;

namespace MyPolarBear.GameObjects
{
    class PolarBear : Entity
    {        
        public enum Power
        {
            Normal,
            Ice,
            Fire,
            Lighting
        }

        public static Power power;
        public static int MaxHealth;
        public static int CurHealth;
        public static int MaxHitPoints;
        public static int CurHitPoints;


        public bool IsColliding = false;
        public bool bHasSeed = false;

        public bool bMoving;


        private int timeProjectileFired;

        private Vector2 mScale;


        public PolarBear(Vector2 position)
            : base(position)
        {
            power = Power.Ice;
            //mAnimator = new Animator();
            Scale = 1;
            mScale = new Vector2(Scale, Scale);
            MaxHealth = 100;
            CurHealth = 75;
            MaxHitPoints = 5;
            CurHitPoints = 3;
        }

        public override void LoadContent()
        {
            //Texture = ContentManager.GetTexture("IceWalkingRight");

            Animation ani = new Animation(ContentManager.GetTexture("IceWalkingRight"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("IceWalkRight", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingRight"), 5, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("IceWalkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingFront"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("IceWalkFront", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingBack"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("IceWalkBack", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingRight"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("FireWalkRight", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingRight"), 4, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("FireWalkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingFront"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("FireWalkFront", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingBack"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("FireWalkBack", ani);

            bMoving = false;
            mAnimator.PlayAnimation("IceWalkFront", false);

            base.LoadContent();

            CollisionBox = new Rectangle(CollisionBox.X, CollisionBox.Y, 25, 25);
        }

        public override void Update(GameTime gameTime)
        {
            bMoving = false;

            MathHelper.Clamp((float)CurHitPoints, 0, 5);
            MathHelper.Clamp((float)CurHealth, 0, 100);
            MathHelper.Clamp((float)MaxHitPoints, 0, 5);
            MathHelper.Clamp((float)MaxHealth, 0, 100);
            
            if (InputManager.Keyboard.IsKeyPressed(Keys.A) || InputManager.GamePad.IsButtonPressed(Buttons.LeftThumbstickLeft))
            {

                //Velocity = new Vector2(-5.0f, 0.0f);
                //if (!IsColliding)
                   // Translate(new Vector2(-5.0f, 0.0f));

                //Translate(new Vector2(-5.0f, 0.0f));
                Velocity = new Vector2(-5, 0);
                bMoving = true;


                switch (power)
                {
                    case Power.Normal:
                        break;
                    case Power.Ice:
                        mAnimator.PlayAnimation("IceWalkLeft", false); break;
                    case Power.Fire:
                        mAnimator.PlayAnimation("FireWalkLeft", false); break;
                    case Power.Lighting:
                        break;
                }
            }
            if (InputManager.Keyboard.IsKeyPressed(Keys.D) || InputManager.GamePad.IsButtonPressed(Buttons.LeftThumbstickRight))
            {

                //Velocity = new Vector2(5.0f, 0.0f);
                //if (!IsColliding)
                   // Translate(new Vector2(5.0f, 0.0f));
                //Translate(new Vector2(5.0f, 0.0f));
                Velocity = new Vector2(5, 0);
                bMoving = true;


                switch (power)
                {
                    case Power.Normal:
                        break;
                    case Power.Ice:
                        mAnimator.PlayAnimation("IceWalkRight", false); break;
                    case Power.Fire:
                        mAnimator.PlayAnimation("FireWalkRight", false); break;
                    case Power.Lighting:
                        break;
                }               
            }
            if (InputManager.Keyboard.IsKeyPressed(Keys.W) || InputManager.GamePad.IsButtonPressed(Buttons.LeftThumbstickUp))
            {

                //Velocity = new Vector2(0.0f, -5.0f);
               // if (!IsColliding)
                 //   Translate(new Vector2(0.0f, -5.0f));

                //Translate(new Vector2(0.0f, -5.0f));
                Velocity = new Vector2(0, -5);
                bMoving = true;


                switch (power)
                {
                    case Power.Normal:
                        break;
                    case Power.Ice:
                        mAnimator.PlayAnimation("IceWalkBack", false); break;
                    case Power.Fire:
                        mAnimator.PlayAnimation("FireWalkBack", false); break;
                    case Power.Lighting:
                        break;
                }                
            }
            if (InputManager.Keyboard.IsKeyPressed(Keys.S) || InputManager.GamePad.IsButtonPressed(Buttons.LeftThumbstickDown))
            {

                //Velocity = new Vector2(0.0f, 5.0f);
                //if (!IsColliding)
                  //  Translate(new Vector2(0.0f, 5.0f));

                //Translate(new Vector2(0.0f, 5.0f));
                Velocity = new Vector2(0, 5);
                bMoving = true;


                switch (power)
                {
                    case Power.Normal:
                        break;
                    case Power.Ice:
                        mAnimator.PlayAnimation("IceWalkFront", false); break;
                    case Power.Fire:
                        mAnimator.PlayAnimation("FireWalkFront", false); break;
                    case Power.Lighting:
                        break;
                }
            } 

            if (!bMoving)
            {
                Velocity = new Vector2(0, 0);
            }

            if (InputManager.GamePad.IsButtonReleased(Buttons.DPadUp))
                power = Power.Normal;
            if (InputManager.GamePad.IsButtonReleased(Buttons.DPadLeft))
                power = Power.Ice;
            if (InputManager.GamePad.IsButtonReleased(Buttons.DPadRight))
                power = Power.Fire;            

            if (InputManager.Keyboard.IsKeyReleased(Keys.Space))
                SwitchPowers();

            //if (InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Left) != Vector2.Zero)
              //  Translate(InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Left) * 5);

            if (InputManager.GamePad.IsButtonReleased(Buttons.RightShoulder))
                SwitchPowers();

            if (InputManager.Mouse.IsButtonReleased(MouseComponent.MouseButton.Left))
            {
                Projectile projectile = ShootProjectile(InputManager.Mouse.GetCurrentMousePosition() - ScreenManager.camera.ScreenCenter);                
                projectile.LoadContent();
                projectile.IsAlive = true;
                UpdateKeeper.getInstance().addEntity(projectile);
                DrawKeeper.getInstance().addEntity(projectile);
            }

            if (InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Right).Length() >= .5)
            {
                Projectile projectile = ShootProjectile(InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Right));
                if (gameTime.TotalGameTime.TotalMilliseconds - timeProjectileFired >= 500)
                {
                    InputManager.GamePad.StartVibration();
                    projectile.LoadContent();
                    UpdateKeeper.getInstance().addEntity(projectile);
                    DrawKeeper.getInstance().addEntity(projectile);
                    projectile.IsAlive = true;
                    timeProjectileFired = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else
            {
                InputManager.GamePad.StopVibration();
            }

            // collide with level elements
            Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

            foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
            {
                //if (CollisionBox.Intersects(ele.CollisionRect))
                if (travelRect.Intersects(ele.CollisionRect))
                {
                    //if (Velocity.X > 0 && ele.CollisionRect.Left + 6 > CollisionBox.Right)
                    //{
                    //    Velocity = Vector2.Zero;
                    //    Position += new Vector2(CollisionBox.Right - (ele.CollisionRect.Left + 5), 0);
                    //}

                    //if (Velocity.X < 0 && ele.CollisionRect.Right - 6 < CollisionBox.Left)
                    //{
                    //    Velocity = Vector2.Zero;
                    //    Position += new Vector2(5, 0);
                    //}

                    //if (Velocity.Y > 0 && ele.CollisionRect.Top + 6 > CollisionBox.Bottom)
                    //{
                    //    Velocity = Vector2.Zero;
                    //    Position += new Vector2(0, -5);
                    //}

                    //if (Velocity.Y < 0 && ele.CollisionRect.Bottom - 6 < CollisionBox.Top)
                    //{
                    //    Velocity = Vector2.Zero;
                    //    Position += new Vector2(0, 5);
                    //}

                    //if (Velocity.X > 0 && CollisionBox.Right + Velocity.X > ele.CollisionRect.Left)
                    //{
                        Velocity = Vector2.Zero;
                    //}

                        if (ele.Type.Equals("Tree") || ele.Type.Equals("Tree2"))
                        {
                            if (InputManager.Keyboard.IsKeyPressed(Keys.T) || InputManager.GamePad.IsButtonPressed(Buttons.A))
                            {
                                bHasSeed = true;
                            }
                        }

                        if (ele.Type.Equals("SoftGround") && bHasSeed)
                        {
                            if (InputManager.Keyboard.IsKeyPressed(Keys.T) || InputManager.GamePad.IsButtonPressed(Buttons.A))
                            {
                                ele.Type = "Tree";
                                ele.Tex = ContentManager.GetTexture("Tree");
                            }
                        }

                }
            }

            Position += Velocity;
            //CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, , CollisionBox.Height);


            base.Update(gameTime);
        }

        public void SwitchPowers()
        {
            switch (power)
            {
                case Power.Normal: power = Power.Ice;
                    break;
                case Power.Ice: power = Power.Fire;
                    break;
                case Power.Fire: power = Power.Lighting;
                    break;
                case Power.Lighting: power = Power.Normal;
                    break;
            }
        }

        public Projectile ShootProjectile(Vector2 direction)
        {
            return new Projectile(Position, 10.0f, direction, power);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            //int rectWidth = Texture.Width;
            //int rectHeight = Texture.Height / 5;
            //Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 8), rectWidth, rectHeight);

            //rectWidth *= 2;
            //rectHeight *= 2;
            //Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            //spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);

            ////if (isFiring)
            ////{
            //    aniFrame++;
            //    if (aniFrame % 40 == 0)
            //    {
            //        isFiring = false;
            //        aniFrame = 0;
            //    }
            ////}
            mScale.X = Scale;
            mScale.Y = Scale;

            //spriteBatch.Draw(ContentManager.GetTexture("HardRock"), CollisionBox, Color.White);
            mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);
        }
    }
}
