using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MyPolarBear.Input;
using MyPolarBear.GameObjects;
using MyPolarBear.GameScreens;
using MyPolarBear.Content;

namespace MyPolarBear
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;
        InputManager inputManager;
        ContentManager contentManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenManager.SCREENWIDTH;
            graphics.PreferredBackBufferHeight = ScreenManager.SCREENHEIGHT;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            
            screenManager = new ScreenManager(this);
            inputManager = new InputManager();
            contentManager = new ContentManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Images
            //ContentManager.AddTexture("PolarBear", Content.Load<Texture2D>("Images/PolarBear"));
            //ContentManager.AddTexture("IcePowerPolarBear", Content.Load<Texture2D>("Images/IcePowerPolarBear"));
            //ContentManager.AddTexture("FirePowerPolarBear", Content.Load<Texture2D>("Images/FirePowerPolarBear"));
            //ContentManager.AddTexture("LightningPowerPolarBear", Content.Load<Texture2D>("Images/LightningPowerPolarBear"));
            ContentManager.AddTexture("Heart", Content.Load<Texture2D>("Images/Heart"));
            ContentManager.AddTexture("IcePowerHeart", Content.Load<Texture2D>("Images/IcyHeart"));
            ContentManager.AddTexture("FirePowerHeart", Content.Load<Texture2D>("Images/FieryHeart"));
            ContentManager.AddTexture("LightningPowerHeart", Content.Load<Texture2D>("Images/LightningHeart"));
            ContentManager.AddTexture("Reticule", Content.Load<Texture2D>("Images/Reticule"));
            ContentManager.AddTexture("BasicTerrain", Content.Load<Texture2D>("Images/BasicTerrain"));
            ContentManager.AddTexture("Meter", Content.Load<Texture2D>("Images/Meter"));
            ContentManager.AddTexture("FullHeart", Content.Load<Texture2D>("Images/FullHeart"));
            ContentManager.AddTexture("EmptyHeart", Content.Load<Texture2D>("Images/EmptyHeart"));
            ContentManager.AddTexture("NormalSelected", Content.Load<Texture2D>("Images/NormalSelected"));
            ContentManager.AddTexture("IceSelected", Content.Load<Texture2D>("Images/IceSelected"));
            ContentManager.AddTexture("FireSelected", Content.Load<Texture2D>("Images/FireSelected"));
            ContentManager.AddTexture("Background", Content.Load<Texture2D>("Images/Background"));
            ContentManager.AddTexture("ForestBoss", Content.Load<Texture2D>("Images/ForestBoss"));

            // Arctic SpriteSheets
            ContentManager.AddTexture("IceWaveBack", Content.Load<Texture2D>("SpriteSheets/Arctic/icewaveBack"));
            ContentManager.AddTexture("IceWaveFront", Content.Load<Texture2D>("SpriteSheets/Arctic/icewaveFront"));
            ContentManager.AddTexture("IceWaveRight", Content.Load<Texture2D>("SpriteSheets/Arctic/icewaveRight"));
            ContentManager.AddTexture("IceWalkingBack", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingBack"));
            ContentManager.AddTexture("IceWalkingFront", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingFront"));
            ContentManager.AddTexture("IceWalkingRight", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingRight"));

            // Nimbus SpriteSheets
            ContentManager.AddTexture("NimbusAttackRight", Content.Load<Texture2D>("SpriteSheets/Nimbus/nimbusAttackRightt"));           

            // Normal SpriteSheets
            ContentManager.AddTexture("ShootHeartRight", Content.Load<Texture2D>("SpriteSheets/Normal/shootheartRight"));
            ContentManager.AddTexture("WalkLeft", Content.Load<Texture2D>("SpriteSheets/Normal/walkingLeft"));
            ContentManager.AddTexture("WalkRight", Content.Load<Texture2D>("SpriteSheets/Normal/walkingRight"));

            // Pyrus SpriteSheets
            ContentManager.AddTexture("FireBallBack", Content.Load<Texture2D>("SpriteSheets/Pyrus/fireballBack"));
            ContentManager.AddTexture("FireBallFront", Content.Load<Texture2D>("SpriteSheets/Pyrus/fireballFront"));
            ContentManager.AddTexture("FireBallRight", Content.Load<Texture2D>("SpriteSheets/Pyrus/fireballRight"));
            ContentManager.AddTexture("FireWalkingFront", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkFront"));
            ContentManager.AddTexture("FireWalkingBack", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkingBack"));
            ContentManager.AddTexture("FireWalkingRight", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkRight"));

            // Level elements
            ContentManager.AddTexture("Boulder", Content.Load<Texture2D>("LevelElements/boulder"));
            ContentManager.AddTexture("HardRock", Content.Load<Texture2D>("LevelElements/hardRock"));
            ContentManager.AddTexture("Lake", Content.Load<Texture2D>("LevelElements/lake"));
            ContentManager.AddTexture("SoftGround", Content.Load<Texture2D>("LevelElements/softGround"));
            ContentManager.AddTexture("Tree", Content.Load<Texture2D>("LevelElements/tree"));
            ContentManager.AddTexture("Tree2", Content.Load<Texture2D>("LevelElements/tree2"));
            ContentManager.AddTexture("Water", Content.Load<Texture2D>("LevelElements/water"));
            ContentManager.AddTexture("WaterSandBottom", Content.Load<Texture2D>("LevelElements/waterSandBottom"));
            ContentManager.AddTexture("WaterSandBottomLeft", Content.Load<Texture2D>("LevelElements/WaterSandBottomLeft"));
            ContentManager.AddTexture("WaterSandBottomRight", Content.Load<Texture2D>("LevelElements/WaterSandBottomRight"));
            ContentManager.AddTexture("WaterSandLeft", Content.Load<Texture2D>("LevelElements/WaterSandLeft"));
            ContentManager.AddTexture("WaterSandRight", Content.Load<Texture2D>("LevelElements/WaterSandRight"));
            ContentManager.AddTexture("WaterSandTop", Content.Load<Texture2D>("LevelElements/WaterSandTop"));
            ContentManager.AddTexture("WaterSandTopLeft", Content.Load<Texture2D>("LevelElements/WaterSandTopLeft"));
            ContentManager.AddTexture("WaterSandTopRight", Content.Load<Texture2D>("LevelElements/WaterSandTopRight"));

            // Fonts
            ContentManager.AddFont("Calibri", Content.Load<SpriteFont>("Fonts/Calibri"));
            
            base.LoadContent();

            Components.Add(screenManager);
            screenManager.Initialize();
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SteelBlue);

            base.Draw(gameTime);
        }
    }
}
