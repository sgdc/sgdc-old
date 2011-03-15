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
        //Rectangle levelRect;

        List<Element> menuElements;
        List<Element> addedElements;

        Element currElement;
        Texture2D cursor;
        Texture2D menuPane;
        Texture2D backGround;

        Texture2D trashCan;
        Rectangle trashRect;

        Texture2D save;
        Texture2D saving;
        Texture2D currSave;
        Rectangle saveRect;
        bool bSaving;

        Vector2 selectionOffset;
        Vector2 cameraPos;

        Element selectedElement;
        Texture2D selectTex;
        //Rectangle selectEleRect;

        MouseState currMouseState;
        MouseState prevMouseState;

        //ContentManager mContentManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
       
            Content.RootDirectory = "Content";

            //mContentManager = new ContentManager();
            ContentManager.Initialize();
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
            menuElements = new List<Element>();
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
            backGround = Content.Load<Texture2D>("backgroundDirt");

            trashCan = Content.Load<Texture2D>("trashcan");
            trashRect = new Rectangle(graphics.PreferredBackBufferWidth - 70, graphics.PreferredBackBufferHeight - 70, 50, 50);

            save = Content.Load<Texture2D>("save");
            saving = Content.Load<Texture2D>("saving");
            currSave = save;
            saveRect = new Rectangle(menuRect.X + 50, menuRect.Height - 80, save.Width, save.Height);

            selectTex = Content.Load<Texture2D>("selectionBox");

            ContentManager.AddTexture("Tree", Content.Load<Texture2D>("tree"));
            ContentManager.AddTexture("Tree2", Content.Load<Texture2D>("tree2"));
            ContentManager.AddTexture("Water", Content.Load<Texture2D>("water"));
            ContentManager.AddTexture("WaterSandRight", Content.Load<Texture2D>("waterSandRight"));
            ContentManager.AddTexture("WaterSandLeft", Content.Load<Texture2D>("waterSandLeft"));
            ContentManager.AddTexture("WaterSandTop", Content.Load<Texture2D>("waterSandTop"));
            ContentManager.AddTexture("WaterSandBottom", Content.Load<Texture2D>("waterSandBottom"));
            ContentManager.AddTexture("WaterSandTopRight", Content.Load<Texture2D>("waterSandTopRight"));
            ContentManager.AddTexture("WaterSandTopLeft", Content.Load<Texture2D>("waterSandTopLeft"));
            ContentManager.AddTexture("WaterSandBottomRight", Content.Load<Texture2D>("waterSandBottomRight"));
            ContentManager.AddTexture("WaterSandBottomLeft", Content.Load<Texture2D>("waterSandBottomLeft"));
            ContentManager.AddTexture("Boulder", Content.Load<Texture2D>("boulder"));
            ContentManager.AddTexture("Blocks", Content.Load<Texture2D>("blocks"));
            ContentManager.AddTexture("SoftGround", Content.Load<Texture2D>("softGround"));
            ContentManager.AddTexture("Lake", Content.Load<Texture2D>("lake"));
            ContentManager.AddTexture("HardRock", Content.Load<Texture2D>("hardRock"));
            ContentManager.AddTexture("Sand", Content.Load<Texture2D>("sand"));
            ContentManager.AddTexture("Water2", Content.Load<Texture2D>("water2"));
            ContentManager.AddTexture("Grass", Content.Load<Texture2D>("grass"));
            ContentManager.AddTexture("Flowers", Content.Load<Texture2D>("flowers"));
            ContentManager.AddTexture("Bush", Content.Load<Texture2D>("bush"));
            ContentManager.AddTexture("Stump", Content.Load<Texture2D>("stump"));
            ContentManager.AddTexture("GrassBig", Content.Load<Texture2D>("grassBig"));
            ContentManager.AddTexture("Tree3", Content.Load<Texture2D>("tree3"));

            Element menuEle;
            int x = 40;
            int y = 40;
            foreach (KeyValuePair<string, Texture2D> pair in ContentManager.Textures)
            {
                menuEle = new Element(new Vector2(x, y), pair.Key, pair.Value);
                menuEle.CollisionRect = new Rectangle(menuEle.CollisionRect.X, menuEle.CollisionRect.Y, 25, 25);
                menuElements.Add(menuEle);

                if (x >= 130)
                {
                    y += 45;
                    x = 40;
                }
                else
                {
                    x += 45;
                }
            }

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

            prevMouseState = currMouseState;
            currMouseState = Mouse.GetState();

            // check menu element press
            if (currElement == null)
            {
                foreach (Element ele in menuElements)
                {
                    if (ele.CollisionRect.Contains(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        currElement = new Element(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), ele.Type, ele.Tex);
                        selectionOffset = new Vector2(Mouse.GetState().X - ele.Position.X, Mouse.GetState().Y - ele.Position.Y);
                        //bCurrNew = true;
                    }
                }
            }

            // currElement still null
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

            // drag/drop
            if (currElement != null)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    currElement.Position = new Vector2(Mouse.GetState().X + cameraPos.X - selectionOffset.X, Mouse.GetState().Y + cameraPos.Y - selectionOffset.Y);
                    currElement.CollisionRect.X = (int)currElement.Position.X;
                    currElement.CollisionRect.Y = (int)currElement.Position.Y;
                }
                else
                {
                    if (!trashRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        addedElements.Add(currElement);
                    }
          
                    currElement = null;
                }
            }

            // select menu element for easy placing
            if (currMouseState != null && currMouseState.RightButton == ButtonState.Pressed 
                && prevMouseState != null && prevMouseState.RightButton == ButtonState.Released)
            {
                if (menuRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    foreach (Element ele in menuElements)
                    {
                        if (ele.CollisionRect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            selectedElement = ele;
                        }
                    }
                }
                else
                {
                    if (selectedElement != null)
                    {
                        Vector2 pos = new Vector2(Mouse.GetState().X - selectedElement.Tex.Width / 2, Mouse.GetState().Y - selectedElement.Tex.Height / 2);
                        pos += cameraPos;
                        addedElements.Add(new Element(pos, selectedElement.Type, selectedElement.Tex));
                    }
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
                tex = ContentManager.GetTexture(type);

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

            spriteBatch.Draw(backGround, new Rectangle(-2048 - (int)cameraPos.X, 0 - (int)cameraPos.Y, 2048, 2048), Color.White);
            spriteBatch.Draw(backGround, new Rectangle(-2048 - (int)cameraPos.X, -2048 - (int)cameraPos.Y, 2048, 2048), Color.White);
            spriteBatch.Draw(backGround, new Rectangle(0 - (int)cameraPos.X, -2048 - (int)cameraPos.Y, 2048, 2048), Color.White);
            spriteBatch.Draw(backGround, new Rectangle(0 - (int)cameraPos.X, 0 - (int)cameraPos.Y, 2048, 2048), Color.White);

            // draw level elements
            foreach (Element ele in addedElements)
            {
                spriteBatch.Draw(ele.Tex, ele.Position - cameraPos, Color.White);
            }

            // draw menu elements
            spriteBatch.Draw(menuPane, menuRect, Color.White);

            foreach (Element ele in menuElements)
            {
                spriteBatch.Draw(ele.Tex, ele.CollisionRect, Color.White);
            }

            // draw selection box
            if (selectedElement != null)
            {
                spriteBatch.Draw(selectTex, selectedElement.CollisionRect, Color.White);
            }

            // draw save button
            spriteBatch.Draw(currSave, new Vector2(menuRect.X + 50, menuRect.Height - 100), Color.White);

            // draw trash can
            spriteBatch.Draw(trashCan, trashRect, Color.White);

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
