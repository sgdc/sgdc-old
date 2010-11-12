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
        PhysicsPharaoh mPhysicsPharaoh;
        Vector2 worldSize;
        Vector2 cellSize;
        Texture2D mGridTexture;

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
            cellSize = new Vector2(85, 85);
            mPhysicsPharaoh = new PhysicsPharaoh(worldSize, cellSize);

            redBall = new BounceBall(150, 130);
            redBall.Velocity(2, 2);
            //redBall.SetColor(Color.Red);
            
            Entity ball;
            for (int i = 0; i < worldSize.X / 80; i++)
            {
                ball = new Entity(i * 80, 0);
                ball.GetPhysicsBaby().SetStatic(true);
                blueBalls.Add(ball);

                ball = new Entity(i * 80, (int)worldSize.Y - 120);
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

            ball = new Entity(240, 200);
            ball.GetPhysicsBaby().SetStatic(true);
            blueBalls.Add(ball);

            ball = new Entity(300, 200);
            ball.Velocity(2, 2);
            blueBalls.Add(ball);

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

            foreach (Entity ball in blueBalls)
            {
                ball.LoadContent(this.Content, "yodaballsmalltransparent");
                ball.EnablePhysics(mPhysicsPharaoh, true, true);
            }

            mGridTexture = this.Content.Load<Texture2D>("CollisionGridCell");
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
            foreach (Entity ball in blueBalls)
            {
                ball.Draw();
            }
            Sprite.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
