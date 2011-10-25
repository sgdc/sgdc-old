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
using MyPolarBear.Audio;

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
        SoundManager audioManager;

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
            audioManager = new SoundManager();
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
            ContentManager.AddTexture("Heart", Content.Load<Texture2D>("Images/Heart"));
            ContentManager.AddTexture("Reticule", Content.Load<Texture2D>("Images/Reticule"));            
            ContentManager.AddTexture("Meter", Content.Load<Texture2D>("Images/Meter"));
            ContentManager.AddTexture("FullHeart", Content.Load<Texture2D>("Images/FullHeart"));
            ContentManager.AddTexture("EmptyHeart", Content.Load<Texture2D>("Images/EmptyHeart"));
            ContentManager.AddTexture("NormalSelected", Content.Load<Texture2D>("Images/NormalSelected"));
            ContentManager.AddTexture("IceSelected", Content.Load<Texture2D>("Images/IceSelected"));
            ContentManager.AddTexture("FireSelected", Content.Load<Texture2D>("Images/FireSelected"));
            ContentManager.AddTexture("Background", Content.Load<Texture2D>("Images/BackgroundDirt"));
            ContentManager.AddTexture("ForestBoss", Content.Load<Texture2D>("Images/ForestBoss"));
            ContentManager.AddTexture("FireAttack", Content.Load<Texture2D>("Images/FireAttack"));
            ContentManager.AddTexture("IceAttack", Content.Load<Texture2D>("Images/IceAttack"));

            // Arctic SpriteSheets           
            ContentManager.AddTexture("IceWalkingBack", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingBack"));
            ContentManager.AddTexture("IceWalkingFront", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingFront"));
            ContentManager.AddTexture("IceWalkingRight", Content.Load<Texture2D>("SpriteSheets/Arctic/walkingRight"));

            // Nimbus SpriteSheets
            //ContentManager.AddTexture("NimbusAttackRight", Content.Load<Texture2D>("SpriteSheets/Nimbus/nimbusAttackRightt"));           

            // Normal SpriteSheets            
            ContentManager.AddTexture("UrsoWalkingFront", Content.Load<Texture2D>("SpriteSheets/Urso/ursoWalkingFront"));
            ContentManager.AddTexture("UrsoWalkingBack", Content.Load<Texture2D>("SpriteSheets/Urso/ursoWalkingBack"));
            ContentManager.AddTexture("UrsoWalkingRight", Content.Load<Texture2D>("SpriteSheets/Urso/ursoWalkingRight"));

            // Pyrus SpriteSheets
            ContentManager.AddTexture("FireWalkingFront", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkFront"));
            ContentManager.AddTexture("FireWalkingBack", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkingBack"));
            ContentManager.AddTexture("FireWalkingRight", Content.Load<Texture2D>("SpriteSheets/Pyrus/walkRight"));

            //Animal SpriteSheets
            ContentManager.AddTexture("TigerIdle", Content.Load<Texture2D>("SpriteSheets/Animals/tigerIdle"));
            ContentManager.AddTexture("TigerWalkRight", Content.Load<Texture2D>("SpriteSheets/Animals/tigerWalkRight"));
            ContentManager.AddTexture("TigerWalkBack", Content.Load<Texture2D>("SpriteSheets/Animals/tigerWalkBack"));
            ContentManager.AddTexture("TigerWalkFront", Content.Load<Texture2D>("SpriteSheets/Animals/tigerWalkFront"));
            ContentManager.AddTexture("LionIdle", Content.Load<Texture2D>("SpriteSheets/Animals/lionIdle"));
            ContentManager.AddTexture("LionWalkRight", Content.Load<Texture2D>("SpriteSheets/Animals/lionWalkRight"));
            ContentManager.AddTexture("LionWalkBack", Content.Load<Texture2D>("SpriteSheets/Animals/lionWalkBack"));
            ContentManager.AddTexture("LionWalkFront", Content.Load<Texture2D>("SpriteSheets/Animals/lionWalkFront"));
            ContentManager.AddTexture("PantherIdle", Content.Load<Texture2D>("SpriteSheets/Animals/pantherIdle"));            
            ContentManager.AddTexture("PantherWalkRight", Content.Load<Texture2D>("SpriteSheets/Animals/pantherWalkRight"));                        
            ContentManager.AddTexture("PantherWalkBack", Content.Load<Texture2D>("SpriteSheets/Animals/pantherWalkBack"));                        
            ContentManager.AddTexture("PantherWalkFront", Content.Load<Texture2D>("SpriteSheets/Animals/pantherWalkFront"));

            //Boss SpriteSheets
            ContentManager.AddTexture("ForestBossIdle", Content.Load<Texture2D>("SpriteSheets/Bosses/forestBossIdle"));
            ContentManager.AddTexture("ForestBossAttack", Content.Load<Texture2D>("SpriteSheets/Bosses/forestBossAttack"));

            // Other Bear SpriteSheets
            ContentManager.AddTexture("BrownBearWalkBack", Content.Load<Texture2D>("SpriteSheets/Bears/brownbearwalkback"));
            ContentManager.AddTexture("BrownBearWalkFront", Content.Load<Texture2D>("SpriteSheets/Bears/brownbearwalkfront"));
            ContentManager.AddTexture("BrownBearWalkRight", Content.Load<Texture2D>("SpriteSheets/Bears/brownbearwalkright"));
            ContentManager.AddTexture("WoodBearWalkBack", Content.Load<Texture2D>("SpriteSheets/Bears/woodbearwalkback"));
            ContentManager.AddTexture("WoodBearWalkFront", Content.Load<Texture2D>("SpriteSheets/Bears/woodbearwalkfront"));
            ContentManager.AddTexture("WoodBearWalkRight", Content.Load<Texture2D>("SpriteSheets/Bears/woodbearwalkright"));

            // Level elements
            ContentManager.AddTexture("Boulder", Content.Load<Texture2D>("LevelElements/boulder"));
            ContentManager.AddTexture("Blocks", Content.Load<Texture2D>("LevelElements/blocks"));
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
            ContentManager.AddTexture("Sand", Content.Load<Texture2D>("LevelElements/sand"));
            ContentManager.AddTexture("Water2", Content.Load<Texture2D>("LevelElements/water2"));
            ContentManager.AddTexture("Ice", Content.Load<Texture2D>("LevelElements/ice"));
            ContentManager.AddTexture("Grass", Content.Load<Texture2D>("LevelElements/grass"));
            ContentManager.AddTexture("Flowers", Content.Load<Texture2D>("LevelElements/flowers"));
            ContentManager.AddTexture("Bush", Content.Load<Texture2D>("LevelElements/bush"));
            ContentManager.AddTexture("Stump", Content.Load<Texture2D>("LevelElements/stump"));
            ContentManager.AddTexture("GrassBig", Content.Load<Texture2D>("LevelElements/grassBig"));
            ContentManager.AddTexture("Tree3", Content.Load<Texture2D>("LevelElements/tree3"));
            ContentManager.AddTexture("BabyPlant", Content.Load<Texture2D>("LevelElements/babyplant"));

            // Commands
            ContentManager.AddTexture("AttackCommand", Content.Load<Texture2D>("Commands/attackCommand"));
            ContentManager.AddTexture("PlantCommand", Content.Load<Texture2D>("Commands/plantCommand"));
            ContentManager.AddTexture("SeedCommand", Content.Load<Texture2D>("Commands/seedCommand"));
            ContentManager.AddTexture("ListeningCommand", Content.Load<Texture2D>("Commands/listening"));
            ContentManager.AddTexture("ProblemCommand", Content.Load<Texture2D>("Commands/problem"));

            // Fonts
            ContentManager.AddFont("Calibri", Content.Load<SpriteFont>("Fonts/Calibri"));

            // Sounds
            SoundManager.AddMusic("Techno", Content.Load<SoundEffect>("Sounds/Techno"), true);
            SoundManager.AddMusic("Music", Content.Load<SoundEffect>("Sounds/Music"), false);

            Components.Add(screenManager);
            screenManager.Initialize();

            base.LoadContent();
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
