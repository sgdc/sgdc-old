using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SGDE;
using SGDE.Physics;
using SGDE.Graphics;

namespace TestDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        BounceBall redBall;
        List<Entity> blueBalls;
        BounceBox box;
        BounceBox movingBox;
        PhysicsPharaoh mPhysicsPharaoh;
        Vector2 worldSize;
        Vector2 cellSize;
        Texture2D mGridTexture;
        Texture2D mHitTexture;
        PortalBox pBox1;
        PortalBox pBox2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            blueBalls = new List<Entity>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            worldSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //cellSize = new Vector2(worldSize.X / 10, worldSize.Y / 10);
            cellSize = new Vector2(25, 25);
            mPhysicsPharaoh = new PhysicsPharaoh(worldSize, cellSize);
            mPhysicsPharaoh.SetGravity(new Vector2(0, 9));

            redBall = new BounceBall(150, 130);
            redBall.Velocity(1, 1);
            //redBall.GetPhysicsBaby().AddForce(new Vector2(0, 9));
            //redBall.SetColor(Color.Red);

            pBox1 = new PortalBox(80, 0, pBox2, false);
            pBox2 = new PortalBox(600, (int)worldSize.Y - 50, pBox1, true);

            pBox1.EnablePhysics(mPhysicsPharaoh, true, true);
            pBox2.EnablePhysics(mPhysicsPharaoh, true, true);

            pBox1.GetPhysicsBaby().AddForce(mPhysicsPharaoh.GetGravity() * -1);
            pBox2.GetPhysicsBaby().AddForce(mPhysicsPharaoh.GetGravity() * -1);

            Entity ball;

            ball = new Entity(0, 0);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            ball = new Entity((int)worldSize.X - 75, (int)worldSize.Y - 150);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            ball = new BounceBall(600, 100);
            ball.Velocity(2, 1);
            blueBalls.Add(ball);

            ball = new BounceBall(650, 300);
            ball.Velocity(-2, 1);
            blueBalls.Add(ball);

            for (int i = 4; i < worldSize.X / 80; i++)
            {
                ball = new Entity(i * 80, 0);
                ball.GetPhysicsBaby().SetStatic(true);
                blueBalls.Add(ball);

                ball = new Entity((i - 3) * 80, (int)worldSize.Y - (350 - i * 30));
                ball.GetPhysicsBaby().SetStatic(true);
                blueBalls.Add(ball);
            }

            ball = new Entity(0, 120);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            ball = new Entity(0, 240);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            ball = new Entity((int)worldSize.X - 70, 120);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            ball = new Entity((int)worldSize.X - 70, 240);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            //ball = new Entity(240, 200);
            //ball.GetPhysicsBaby().SetStatic(true);
            //blueBalls.Add(ball);
            box = new BounceBox(100, 400);
            box.GetPhysicsBaby().SetStatic(true);
            //blueBalls.Add(ball);

            //ball = new Entity(380, 200);
            //ball.Velocity(2, 3);
            //blueBalls.Add(ball);
            movingBox = new BounceBox(450, 100);
            movingBox.Velocity(-2, 0);
            movingBox.EnablePhysics(mPhysicsPharaoh, true, true);
            movingBox.GetPhysicsBaby().AddForce(mPhysicsPharaoh.GetGravity() * -1);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Sprite.spriteBatch = new SpriteBatch(GraphicsDevice);

            redBall.LoadContent(this.Content, "darthvaderballsmall");
            redBall.EnablePhysics(mPhysicsPharaoh, true, true);

            box.LoadContent(this.Content, "DomoKunSmall");
            box.EnablePhysics(mPhysicsPharaoh, true, true);

            movingBox.LoadContent(this.Content, "images");
            //movingBox.EnablePhysics(mPhysicsPharaoh, true, true);

            pBox1.LoadContent(this.Content, "portal");
            pBox2.LoadContent(this.Content, "DomoKunSmall");

            foreach (Entity ball in blueBalls)
            {
                ball.LoadContent(this.Content, "yodaballsmalltransparent");
                ball.EnablePhysics(mPhysicsPharaoh, true, true);
            }

            mGridTexture = this.Content.Load<Texture2D>("CollisionGridCell");
            mHitTexture = this.Content.Load<Texture2D>("star");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mPhysicsPharaoh.Update(gameTime);
            redBall.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Sprite.spriteBatch.Begin();
            //mPhysicsPharaoh.DrawCollisionGrid(Sprite.spriteBatch, mGridTexture);
            redBall.Draw();
            box.Draw();
            movingBox.Draw();
            pBox1.Draw();
            pBox2.Draw();
            foreach (Entity ball in blueBalls)
            {
                ball.Draw();
            }
            redBall.DrawHitSpot(mHitTexture);
            Sprite.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
