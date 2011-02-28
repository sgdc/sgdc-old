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

using MyPolarBear.Input;
using MyPolarBear.GameObjects;
using MyPolarBear.GameScreens;




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
        public static Dictionary<String, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static SpriteFont gameFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 1300;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            
            screenManager = new ScreenManager(this);
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
            textures.Add("Images/IcePowerPolarBear", Content.Load<Texture2D>("Images/IcePowerPolarBear"));
            textures.Add("Images/FirePowerPolarBear", Content.Load<Texture2D>("Images/FirePowerPolarBear"));
            textures.Add("Images/GrassPowerPolarBear", Content.Load<Texture2D>("Images/GrassPowerPolarBear"));
            textures.Add("Images/Heart", Content.Load<Texture2D>("Images/Heart"));
            textures.Add("Images/IcyHeart", Content.Load<Texture2D>("Images/IcyHeart"));
            textures.Add("Images/FieryHeart", Content.Load<Texture2D>("Images/FieryHeart"));
            textures.Add("Images/GrassyHeart", Content.Load<Texture2D>("Images/GrassyHeart"));
            textures.Add("Images/WorldMap", Content.Load<Texture2D>("Images/WorldMap"));
            textures.Add("Images/Reticule", Content.Load<Texture2D>("Images/Reticule"));
            textures.Add("Images/BasicTerrain", Content.Load<Texture2D>("Images/BasicTerrain"));

            // Arctic
            textures.Add("SpriteSheets/Arctic/icewaveBack", Content.Load<Texture2D>("SpriteSheets/Arctic/icewaveBack"));
            textures.Add("SpriteSheets/Arctic/icewaveFront", Content.Load<Texture2D>("SpriteSheets/Arctic/icewaveFront"));
            textures.Add("SpriteSheets/Arctic/icewaveRight", Content.Load<Texture2D>("SpriteSheets/Arctic/icewaveRight"));
            textures.Add("SpriteSheets/Arctic/walkingBack", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingBack"));
            textures.Add("SpriteSheets/Arctic/walkingFront", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingFront"));
            textures.Add("SpriteSheets/Arctic/walkingRight", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingRight"));

            // Nimbus
            textures.Add("SpriteSheets/Nimbus/nimbusAttackRightt", Content.Load<Texture2D>("SpriteSheets/Nimbus/nimbusAttackRightt"));

            // Normal
            textures.Add("SpriteSheets/Normal/shootheartRight", Content.Load<Texture2D>("SpriteSheets/Normal/shootheartRight"));
            textures.Add("SpriteSheets/Normal/walkLeft3", Content.Load<Texture2D>("SpriteSheets/Normal/walkLeft3"));
            textures.Add("SpriteSheets/Normal/walkRight2", Content.Load<Texture2D>("SpriteSheets/Normal/walkRight2"));

            // Pyrus
            textures.Add("SpriteSheets/Pyrus/fireballBack", Content.Load<Texture2D>("SpriteSheets/Pyrus/fireballBack"));
            textures.Add("SpriteSheets/Pyrus/fireballFront", Content.Load<Texture2D>("SpriteSheets/Pyrus/fireballFront"));
            textures.Add("SpriteSheets/Pyrus/fireballRight", Content.Load<Texture2D>("SpriteSheets/Pyrus/fireballRight"));
            textures.Add("SpriteSheets/Pyrus/walkFront", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkFront"));
            textures.Add("SpriteSheets/Pyrus/walkingBack", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkingBack"));
            textures.Add("SpriteSheets/Pyrus/walkRight", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkRight"));

            gameFont = Content.Load<SpriteFont>("Fonts/Calibri");
            
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
