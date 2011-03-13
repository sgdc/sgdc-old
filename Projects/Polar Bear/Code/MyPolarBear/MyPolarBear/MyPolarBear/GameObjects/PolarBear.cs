using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.GameScreens;
using MyPolarBear.Input;
using MyPolarBear.Content;
using MyPolarBear.Pathfinding;

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
        public static int MaxHitPoints;
        public static int CurHitPoints;        
        public static int NumSeeds = 0;
        public static int NumWater = 0;

        public bool bMoving;

        List<Vector2> path;
        public bool bPathfinding = false;
        int pathPos;

        private int timeProjectileFired;

        private Vector2 mScale;


        public PolarBear(Vector2 position)
            : base(position)
        {
            power = Power.Normal;            
            Scale = 1;
            mScale = new Vector2(Scale, Scale);
            MaxHitPoints = 5;
            CurHitPoints = 3;
        }

        public override void LoadContent()
        {
            //Texture = ContentManager.GetTexture("IceWalkingRight");

            Animation ani = new Animation(ContentManager.GetTexture("WalkRight"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("NormalWalkRight", ani);

            ani = new Animation(ContentManager.GetTexture("WalkRight"), 2, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("NormalWalkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingRight"), 5, 8, 0, true, SpriteEffects.None);
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
            mAnimator.PlayAnimation("NormalWalkRight", false);

            base.LoadContent();

            CollisionBox = new Rectangle(CollisionBox.X, CollisionBox.Y, 25, 25);
        }

        public override void Update(GameTime gameTime)
        {
            bMoving = false;            
            
            if (InputManager.Keyboard.IsKeyPressed(Keys.A) || InputManager.GamePad.IsButtonPressed(Buttons.LeftThumbstickLeft))
            {
                Velocity = new Vector2(-3, 0);
                bMoving = true;

                switch (power)
                {
                    case Power.Normal:
                        mAnimator.PlayAnimation("NormalWalkLeft", false); break;
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
                Velocity = new Vector2(3, 0);
                bMoving = true;

                switch (power)
                {
                    case Power.Normal:
                        mAnimator.PlayAnimation("NormalWalkRight", false); break;
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
                Velocity = new Vector2(0, -3);
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
                Velocity = new Vector2(0, 3);
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

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {                
                if (travelRect.Intersects(element.CollisionRect))
                {
                    if (!(element.Type.Equals("Grass") || element.Type.Equals("GrassBig") || element.Type.Equals("Ice")
                    || element.Type.Equals("SoftGround") || element.Type.Equals("BabyPlant")))
                    {
                        Velocity = Vector2.Zero;
                    }

                    if (element.Type.Equals("Bush"))
                    {
                        if (InputManager.Keyboard.IsKeyReleased(Keys.T) || InputManager.GamePad.IsButtonReleased(Buttons.A))
                        {
                            NumSeeds++;

                            // teach follower
                            foreach (Entity ene in UpdateKeeper.getInstance().getEntities())
                            {
                                if (ene is Enemy && ((Enemy)ene).CurrentState == Enemy.State.Following)
                                {
                                    ((Enemy)ene).bHasSeenSeedGather = true;
                                }
                            }
                        }
                    }

                    if (element.Type.Equals("SoftGround") && NumSeeds > 0)
                    {
                        if (InputManager.Keyboard.IsKeyReleased(Keys.T) || InputManager.GamePad.IsButtonReleased(Buttons.A))
                        {            
                            element.Type = "BabyPlant";
                            element.Tex = ContentManager.GetTexture("BabyPlant");
                            NumSeeds--;
                            AGrid.GetInstance().addResource(element);
                            GameScreen.CurWorldHealth++;   

                            // teach follower
                            foreach (Entity ene in UpdateKeeper.getInstance().getEntities())
                            {
                                if (ene is Enemy && ((Enemy)ene).CurrentState == Enemy.State.Following && ((Enemy)ene).bHasSeenSeedGather)
                                {
                                    ((Enemy)ene).bHasSeenPlanting = true;
                                    ((Enemy)ene).CurrentState = Enemy.State.Planting;
                                }
                            }
                        }
                    }
                }
                if (GameScreen.CurWorldHealth == GameScreen.MaxWorldHealth && element.Type.Equals("Boulder"))
                {
                    UpdateKeeper.getInstance().removeLevelElement(element);
                    DrawKeeper.getInstance().removeLevelElement(element);
                }
            }


            // go to position test
            if (InputManager.Keyboard.IsKeyPressed(Keys.X) && path == null)
            {
                //path = AGrid.GetInstance().getPath(Position, new Vector2(500, 500));
                path = AGrid.GetInstance().getPath(Position, ANode.SEED_SOURCE);
                bPathfinding = true;
                pathPos = 0;
            }
            if (bPathfinding)
            {
                if (path != null && path.Count > 0)
                {
                    if ((int)Position.X < (int)path[pathPos].X + 10 && (int)Position.X > (int)path[pathPos].X - 10
                        && (int)Position.Y < (int)path[pathPos].Y + 10 && (int)Position.Y > (int)path[pathPos].Y - 10)
                    {
                        //path.RemoveAt(0);
                        pathPos++;
                    }

                    if (path.Count > pathPos)
                    {
                        Vector2 dir = path[pathPos] - Position;
                        dir.Normalize();
                        Velocity = dir * 10;
                    }
                    else
                    {
                        path = null;
                        pathPos = 0;
                        bPathfinding = false;
                    }
                }
                else
                {
                    bPathfinding = false;
                }
            }

            NumSeeds = (int)MathHelper.Clamp((float)NumSeeds, 0, 10);
            CurHitPoints = (int)MathHelper.Clamp((float)CurHitPoints, 0, 5);
            GameScreen.CurWorldHealth = (int)MathHelper.Clamp((float)GameScreen.CurWorldHealth, 0, 100);

            Position += Velocity;

            float horizontalBoundary = MathHelper.Clamp(Position.X, -GameScreen.WORLDWIDTH / 2, GameScreen.WORLDWIDTH / 2);
            float verticalBoundary = MathHelper.Clamp(Position.Y, -GameScreen.WORLDHEIGHT / 2, GameScreen.WORLDHEIGHT / 2);

            Position = new Vector2(horizontalBoundary, verticalBoundary);


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
                case Power.Fire: power = Power.Normal;
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
