using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;
using SGDE.Physics;
using SGDE.SceneManagement;
using Microsoft.Xna.Framework.Input;

namespace SGDE
{
    /// <summary>
    /// Base game that wraps much of the game's functions so they can be adjusted from the Game editor.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Static

        private static Game cGame;

        /// <summary>
        /// Gets the current running game.
        /// </summary>
        public static Game CurrentGame { get { return cGame; } }

        #endregion

        #region Instance

        internal GraphicsDeviceManager graphics;
        internal GameContent gameContent;

        private bool loaded;
        private string gameContentName;

        //REPLACE WITH ACTUAL INPUT SYSTEM
        private KeyboardState cstate, ostate;

        #endregion

        /// <summary>
        /// Get the current SpriteBatch.
        /// </summary>
        public SpriteBatch SpriteBatch { get { return SpriteManager.spriteBat; } }

        /// <summary>
        /// Get or set the name of the game content that will be loaded. If this value is set after the <see cref="Initialize()"/> function has executed it will be ignored. If the value is null, empty, or made 
        /// of white-spaces it will be ignored as well. If this value references a game content that doesn't exist it will throw an exception.
        /// </summary>
        public string GameContentName
        {
            get
            {
                return this.gameContentName;
            }
            set
            {
                if (loaded)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    //Ignore
                    return;
                }
                this.gameContentName = value;
            }
        }

        /// <summary>
        /// Create a new Game.
        /// </summary>
        protected Game()
        {
            if (cGame != null)
            {
                throw new InvalidOperationException(Messages.Game_TooManyGames);
            }
            cGame = this;
            loaded = false;
            gameContentName = "Game";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initialize the game components.
        /// </summary>
        protected override void Initialize()
        {
            //TODO: Since Physics is a Singleton, it can be added as a IUpdatable so that it gets updates without needing to be invoked.
            //Other "services" to put here: State manager, GUI components, input systems, code processor (if it is a GameComponent then it has a reference to the game, content, etc.), etc.

            base.Initialize();

            loaded = true;
        }

        /// <summary>
        /// Load a SGDE game.
        /// </summary>
        protected void LoadGame()
        {
            //Sprite.SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteManager.spriteBat = new SpriteBatch(GraphicsDevice);

            gameContent = Content.Load<GameContent>(gameContentName);
            Game t = this;
            gameContent.Setup(ref t);
            gameContent.Process(ref t);
        }

        /// <summary>
        /// Get loaded content from this Game.
        /// </summary>
        /// <typeparam name="T">The data type that is expected to be returned.</typeparam>
        /// <param name="gameElement">The developer-defined name for a component.</param>
        /// <returns>The requested content, if it exisst, or the default value of the content.</returns>
        public T GetContent<T>(string gameElement)
        {
            return gameContent.GetMapContent<T>(gameElement);
        }

        /// <summary>
        /// Update the game.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            ostate = cstate;

            //Something that requires key states

            cstate = Keyboard.GetState();

            //Could possibly do/call UpdateGame here. Leave it up to dev for now.
            base.Update(gameTime);
        }

        /// <summary>
        /// Update this Game's loaded content.
        /// </summary>
        /// <param name="gameTime">The GameTime since the last update.</param>
        protected void UpdateGame(GameTime gameTime)
        {
            PhysicsPharaoh pharaoh = PhysicsPharaoh.GetInstance();
            lock (pharaoh)
            {
                pharaoh.Update(gameTime);
            }
            lock (gameContent)
            {
                foreach (Entity entity in gameContent.Entities)
                {
                    entity.Update(gameTime);
                }
                //Very inefficient manner of processing. Leave it for now.
                foreach (Entity e in SceneManager.GetInstance().GetKeyboardListeners())
                {
                    e.HandleInput(cstate, this);
                }
            }
        }

        /// <summary>
        /// Draw the game.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            //Could possibly do/call DrawGame here. Leave it up to dev for now.
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw this Game's loaded content.
        /// </summary>
        /// <param name="gameTime">The GameTime since the last draw.</param>
        protected void DrawGame(GameTime gameTime)
        {
            lock (gameContent)
            {
                foreach (Entity entity in gameContent.Entities)
                {
                    entity.Draw(gameTime);
                }
            }
        }
    }
}
