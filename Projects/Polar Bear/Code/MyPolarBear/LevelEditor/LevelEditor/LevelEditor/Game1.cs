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
using System.IO;

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle menuRect;
        Rectangle levelRect;

        Element tree;
        Element water;
        Element waterSandRight;
        Element waterSandLeft;
        Element waterSandTop;
        Element waterSandBottom;
        Element waterSandTopRight;
        Element waterSandTopLeft;
        Element waterSandBottomRight;
        Element waterSandBottomLeft;
        Element tree2;
        Element lake;
        Element boulder;
        Element softGround;
        Element hardRock;

        Rectangle lakeRect;

        List<Element> elements;
        List<Element> addedElements;

        Element currElement;
        //bool bCurrNew;
        Texture2D cursor;
        Texture2D menuPane;
        Texture2D backGround;

        Texture2D save;
        Texture2D saving;
        Texture2D currSave;
        Rectangle saveRect;
        bool bSaving;

        Vector2 selectionOffset;
        Vector2 cameraPos;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
       
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            elements = new List<Element>();
            addedElements = new List<Element>();
            currElement = null;
            //bCurrNew = true;

            menuRect = new Rectangle(0, 0, 200, graphics.PreferredBackBufferHeight);
            cameraPos = Vector2.Zero;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            cursor = Content.Load<Texture2D>("cursor");
            menuPane = Content.Load<Texture2D>("menupane");
            backGround = Content.Load<Texture2D>("background");
            save = Content.Load<Texture2D>("save");
            saving = Content.Load<Texture2D>("saving");
            currSave = save;

            saveRect = new Rectangle(menuRect.X + 50, menuRect.Height - 80, save.Width, save.Height);

            tree = new Element(new Vector2(40, 40), "Tree", Content.Load<Texture2D>("tree"));
            water = new Element(new Vector2(110, 40), "Water", Content.Load<Texture2D>("water"));
            waterSandRight = new Element(new Vector2(40, 110), "WaterSandRight", Content.Load<Texture2D>("waterSandRight"));
            waterSandLeft = new Element(new Vector2(110, 110), "WaterSandLeft", Content.Load<Texture2D>("waterSandLeft"));
            waterSandTop = new Element(new Vector2(40, 180), "WaterSandTop", Content.Load<Texture2D>("waterSandTop"));
            waterSandBottom = new Element(new Vector2(110, 180), "WaterSandBottom", Content.Load<Texture2D>("waterSandBottom"));
            waterSandTopRight = new Element(new Vector2(40, 250), "WaterSandTopRight", Content.Load<Texture2D>("waterSandTopRight"));
            waterSandTopLeft = new Element(new Vector2(110, 250), "WaterSandTopLeft", Content.Load<Texture2D>("waterSandTopLeft"));
            waterSandBottomRight = new Element(new Vector2(40, 320), "WaterSandBottomRight", Content.Load<Texture2D>("waterSandBottomRight"));
            waterSandBottomLeft = new Element(new Vector2(110, 320), "WaterSandBottomLeft", Content.Load<Texture2D>("waterSandBottomLeft"));
            tree2 = new Element(new Vector2(40, 390), "Tree2", Content.Load<Texture2D>("tree2"));
            boulder = new Element(new Vector2(110, 390), "Boulder", Content.Load<Texture2D>("boulder"));
            softGround = new Element(new Vector2(40, 460), "SoftGround", Content.Load<Texture2D>("softGround"));
            lake = new Element(new Vector2(110, 460), "Lake", Content.Load<Texture2D>("lake"));
            hardRock = new Element(new Vector2(40, 530), "HardRock", Content.Load<Texture2D>("hardRock"));

            lakeRect = new Rectangle((int)lake.Position.X, (int)lake.Position.Y, 50, 50);

            elements.Add(tree);
            elements.Add(water);
            elements.Add(waterSandRight);
            elements.Add(waterSandLeft);
            elements.Add(waterSandTop);
            elements.Add(waterSandBottom);
            elements.Add(waterSandTopRight);
            elements.Add(waterSandTopLeft);
            elements.Add(waterSandBottomRight);
            elements.Add(waterSandBottomLeft);
            elements.Add(tree2);
            elements.Add(boulder);
            elements.Add(softGround);
            elements.Add(lake);
            elements.Add(hardRock);

            LoadLevel();
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

            // TODO: Add your update logic here
            if (currElement == null)
            {
                foreach (Element ele in elements)
                {
                    if (ele.CollisionRect.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        currElement = new Element(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), ele.Type, ele.Tex);
                        selectionOffset = new Vector2(Mouse.GetState().X - ele.Position.X, Mouse.GetState().Y - ele.Position.Y);
                        //bCurrNew = true;
                    }
                }
            }

            if (currElement == null)
            {
                foreach (Element ele in addedElements)
                {
                    if (ele.CollisionRect.Contains(Mouse.GetState().X + (int)cameraPos.X, Mouse.GetState().Y + (int)cameraPos.Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        currElement = ele;
                        addedElements.Remove(ele);
                        selectionOffset = new Vector2(Mouse.GetState().X - ele.Position.X + cameraPos.X, Mouse.GetState().Y - ele.Position.Y + cameraPos.Y);
                        //bCurrNew = false;
                        break;
                    }
                }
            }

            if (currElement != null)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    //currElement.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    //currElement.CollisionRect.X = Mouse.GetState().X;
                    //currElement.CollisionRect.Y = Mouse.GetState().Y;

                    currElement.Position = new Vector2(Mouse.GetState().X + cameraPos.X - selectionOffset.X, Mouse.GetState().Y + cameraPos.Y - selectionOffset.Y);
                    //currElement.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    currElement.CollisionRect.X = (int)currElement.Position.X;
                    currElement.CollisionRect.Y = (int)currElement.Position.Y;
                }
                else
                {
                    //currElement.Position = new Vector2(currElement.Position.X + cameraPos.X, currElement.Position.Y + cameraPos.Y);
                    //if (bCurrNew)
                    //{
                        addedElements.Add(currElement);
                    //}
          
                    currElement = null;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                cameraPos = new Vector2(cameraPos.X + 5, cameraPos.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                cameraPos = new Vector2(cameraPos.X - 5, cameraPos.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                cameraPos = new Vector2(cameraPos.X, cameraPos.Y - 5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                cameraPos = new Vector2(cameraPos.X, cameraPos.Y + 5);
            }

            if (bSaving)
            {
                SaveLevel();
                bSaving = false;
                currSave = save;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && saveRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                currSave = saving;
                //Save();
                //currSave = save;
                bSaving = true;
            }

            base.Update(gameTime);
        }

        private void SaveLevel()
        {
            bool bFailed = true;
            StreamWriter fileWriter = null;

            while (bFailed)
            {
                try 
                {
                    fileWriter = new StreamWriter("C:/level.txt");
                    bFailed = false;
                }
                catch (IOException e)
                {
                    bFailed = true;
                }
            }
            

            foreach (Element ele in addedElements)
            {
                fileWriter.WriteLine(ele.Type);
                fileWriter.WriteLine(ele.Position.X);
                fileWriter.WriteLine(ele.Position.Y);
            }

            fileWriter.Close();
        }

        private void LoadLevel()
        {
            if (!File.Exists("C:/level.txt"))
            {
                return;
            }

            StreamReader fileReader = new StreamReader("C:/level.txt");
            string fileLine = fileReader.ReadLine();
            Element ele;
            int x;
            int y;
            string type;
            Texture2D tex = null;

            while (fileLine != null)
            {
                type = fileLine;
                fileLine = fileReader.ReadLine();
                x = Convert.ToInt32(fileLine);
                fileLine = fileReader.ReadLine();
                y = Convert.ToInt32(fileLine);

                switch (type)
                {
                    case "Tree":
                        tex = tree.Tex;
                        break;
                    case "Water":
                        tex = water.Tex;
                        break;
                    case "WaterSandBottom":
                        tex = waterSandBottom.Tex;
                        break;
                    case "WaterSandBottomLeft":
                        tex = waterSandBottomLeft.Tex;
                        break;
                    case "WaterSandBottomRight":
                        tex = waterSandBottomRight.Tex;
                        break;
                    case "WaterSandLeft":
                        tex = waterSandLeft.Tex;
                        break;
                    case "WaterSandRight":
                        tex = waterSandRight.Tex;
                        break;
                    case "WaterSandTop":
                        tex = waterSandTop.Tex;
                        break;
                    case "WaterSandTopLeft":
                        tex = waterSandTopLeft.Tex;
                        break;
                    case "WaterSandTopRight":
                        tex = waterSandTopRight.Tex;
                        break;
                    case "Tree2":
                        tex = tree2.Tex;
                        break;
                    case "Boulder":
                        tex = boulder.Tex;
                        break;
                    case "SoftGround":
                        tex = softGround.Tex;
                        break;
                    case "Lake":
                        tex = lake.Tex;
                        break;
                    case "HardRock":
                        tex = hardRock.Tex;
                        break;
                    default:
                        break;
                }

                ele = new Element(new Vector2(x, y), type, tex);
                addedElements.Add(ele);

                fileLine = fileReader.ReadLine();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // draw background
            spriteBatch.Draw(backGround, new Vector2(-2042, 0) - cameraPos, Color.White);
            spriteBatch.Draw(backGround, new Vector2(-2042, -2042) - cameraPos, Color.White);
            spriteBatch.Draw(backGround, new Vector2(0, -2042) - cameraPos, Color.White);
            spriteBatch.Draw(backGround, new Vector2(0, 0) - cameraPos, Color.White);

            // draw level elements
            foreach (Element ele in addedElements)
            {
                spriteBatch.Draw(ele.Tex, ele.Position - cameraPos, Color.White);
            }

            // draw menu elements
            spriteBatch.Draw(menuPane, menuRect, Color.White);

            foreach (Element ele in elements)
            {
                if (ele.Type.Equals("Lake"))
                {
                    spriteBatch.Draw(ele.Tex, lakeRect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(ele.Tex, ele.Position, Color.White);
                }
            }

            spriteBatch.Draw(currSave, new Vector2(menuRect.X + 50, menuRect.Height - 100), Color.White);

            // draw current element and cursor
            if (currElement != null)
            {
                spriteBatch.Draw(currElement.Tex, currElement.Position - cameraPos, Color.White);
            }

            spriteBatch.Draw(cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
