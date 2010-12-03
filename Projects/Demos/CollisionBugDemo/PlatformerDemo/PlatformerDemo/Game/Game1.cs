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
      Vector2 worldSize;
      Vector2 cellSize;
      GraphicsDeviceManager graphics;

      BounceBall redBall;
      TestPlayerEntity player;
      List<Entity> blueBalls;
      
      Texture2D mGridTexture;
      Texture2D mHitTexture;

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
         cellSize = new Vector2(25, 25);
         SGDE.Physics.PhysicsPharaoh.GetInstance().Initialize(worldSize, cellSize);
         SGDE.Physics.PhysicsPharaoh.GetInstance().SetGravity(new Vector2(0, 9));

         redBall = new BounceBall(150, 130);
         redBall.SetVelocity(1, 1);

         Entity ball;

         ball = new StaticBall(0, 0);
         blueBalls.Add(ball);

         ball = new StaticBall((int)worldSize.X - 75, (int)worldSize.Y - 150);
         blueBalls.Add(ball);

         ball = new BounceBall(600, 100);
         ball.SetVelocity(2, 1);
         blueBalls.Add(ball);

         ball = new BounceBall(650, 300);
         ball.SetVelocity(-2, 1);
         blueBalls.Add(ball);

         for (int i = 0; i < worldSize.X / 80; i++)
         {
            ball = new StaticBall(i * 80, 0);
            blueBalls.Add(ball);

            ball = new StaticBall(i * 80, 350);
            blueBalls.Add(ball);
         }

         ball = new StaticBall(0, 120);
         blueBalls.Add(ball);

         ball = new StaticBall(0, 240);
         blueBalls.Add(ball);

         ball = new StaticBall((int)worldSize.X - 70, 120);
         blueBalls.Add(ball);

         ball = new StaticBall((int)worldSize.X - 70, 240);
         blueBalls.Add(ball);

         player = new TestPlayerEntity(450, 100);
         player.SetColor(Color.Green);

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
         redBall.EnablePhysics(true, true);

         player.LoadContent(this.Content, "star");

         foreach (Entity ball in blueBalls)
         {
            ball.LoadContent(this.Content, "yodaballsmalltransparent");
            ball.EnablePhysics(true, true);
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

         SGDE.Physics.PhysicsPharaoh.GetInstance().Update(gameTime);
         redBall.Update(gameTime);

         foreach (Entity e in SceneManager.GetInstance( ).GetKeyboardListeners( )) e.HandleInput(Keyboard.GetState(), this.Content);

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
            redBall.Draw();
            foreach (Entity ball in blueBalls)
               ball.Draw();
            redBall.DrawHitSpot(mHitTexture);
            player.Draw();
         Sprite.spriteBatch.End();

         base.Draw(gameTime);
      }
   }
}
