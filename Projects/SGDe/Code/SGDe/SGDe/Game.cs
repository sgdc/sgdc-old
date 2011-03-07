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
using Microsoft.Xna.Framework.Input;
using SGDE.Input;

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
        internal Camera camera;

        private bool loaded, useContent;
        private string gameContentName;

        internal InputManager imanager;

        #endregion

        #region Properties

        /// <summary>
        /// Get the current SpriteBatch.
        /// </summary>
        [Obsolete("Use Graphics2D instead")]
        public SpriteBatch SpriteBatch { get { return SpriteManager.gfx; } }

        /// <summary>
        /// Get the current Graphics2D system.
        /// </summary>
        public Graphics2D Graphics2D { get { return SpriteManager.gfx; } }

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
#if WINDOWS
                if (string.IsNullOrWhiteSpace(value))
#else
                if (string.IsNullOrEmpty(value.Trim()))
#endif
                {
                    //Ignore
                    return;
                }
                this.gameContentName = value;
            }
        }

        /// <summary>
        /// Get this game's content, such as levels and Entities.
        /// </summary>
        public GameContent GameContent
        {
            get
            {
                return this.gameContent;
            }
        }

        /// <summary>
        /// Get this Game's InputManager.
        /// </summary>
        public InputManager InputManager
        {
            get
            {
                return this.imanager;
            }
        }

        /// <summary>
        /// Get the camera control for this Game.
        /// </summary>
        public Camera CameraControl
        {
            get
            {
                return this.camera;
            }
        }

        /// <summary>
        /// Get or set it the content system should be used.
        /// </summary>
        protected bool UseContentSystem
        {
            get
            {
                return this.useContent;
            }
            set
            {
                if (this.loaded)
                {
                    return;
                }
                this.useContent = value;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Create a new Game.
        /// </summary>
        protected Game()
        {
            if (Game.cGame != null)
            {
                throw new InvalidOperationException(Messages.Game_TooManyGames);
            }
            cGame = this;
            loaded = false;
            useContent = true;
            gameContentName = "Game";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            imanager = new InputManager();
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

        #endregion

        #region Content

        /// <summary>
        /// Load game content. If overriden, call this functions before attempting to load any other content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            //Sprite.SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteManager.gfx = new Graphics.Graphics2D(this);

            if (useContent)
            {
                gameContent = Content.Load<GameContent>(gameContentName);
                gameContent.Setup(this);
            }
            camera = new Camera(GraphicsDevice.Viewport);
            if (useContent)
            {
                gameContent.Process(this);
            }
        }

        /// <summary>
        /// Get loaded content from this Game. Can only be used if content system is used.
        /// </summary>
        /// <typeparam name="T">The data type that is expected to be returned.</typeparam>
        /// <param name="gameElement">The developer-defined name for a component.</param>
        /// <returns>The requested content, if it exists, or the default value of the content.</returns>
        public T GetContent<T>(string gameElement)
        {
            return gameContent.GetMapContent<T>(gameElement);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update the game.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            PhysicsPharaoh pharaoh = PhysicsPharaoh.GetInstance();
            lock (pharaoh)
            {
                pharaoh.Update(gameTime);
            }
            if (useContent)
            {
                lock (gameContent)
                {
                    foreach (Entity entity in gameContent.Entities)
                    {
                        if (entity.Enabled)
                        {
                            entity.Update(gameTime);
                        }
                    }
                }
            }
            imanager.Update(this, gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draw the game. The order that draw operations occur in: <see cref="PreBeginSpriteBatch"/>, <see cref="BeginSpriteBatch"/>, <see cref="PreDraw"/>, draw game, <see cref="PostDraw"/>, <see cref="EndSpriteBatch"/>, <see cref="PostEndSpriteBatch"/>, <see cref="UIDraw"/>.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            PreBeginSpriteBatch(gameTime);

            BeginSpriteBatch(gameTime);

            PreDraw(gameTime);

            if (useContent)
            {
                lock (gameContent)
                {
                    foreach (Entity entity in gameContent.Entities)
                    {
                        entity.Draw(gameTime);
                    }
                }
            }
            else
            {
                AltDraw(gameTime);
            }

            PostDraw(gameTime);

            EndSpriteBatch(gameTime);

            PostEndSpriteBatch(gameTime);

            SpriteManager.gfx.Begin();

            UIDraw(gameTime);

            SpriteManager.gfx.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// The first operation in the draw pipeline. Default action is the clear the screen to Cornflower blue.
        /// </summary>
        protected virtual void PreBeginSpriteBatch(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        /// <summary>
        /// The second operation in the draw pipeline. Begin the SpriteBatch operation. This is primarily a helper function to handle camera.
        /// </summary>
        protected virtual void BeginSpriteBatch(GameTime gameTime)
        {
            SpriteManager.gfx.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera._transformMatrix);
        }

        /// <summary>
        /// The third operation in the draw pipeline.
        /// </summary>
        protected virtual void PreDraw(GameTime gameTime)
        {
        }

        /// <summary>
        /// This is the draw function called when the content system isn't used.
        /// </summary>
        protected virtual void AltDraw(GameTime gameTime)
        {
        }

        /// <summary>
        /// The fifth operation in the draw pipeline.
        /// </summary>
        protected virtual void PostDraw(GameTime gameTime)
        {
        }

        /// <summary>
        /// The sixth operation in the draw pipeline. End the SpriteBatch operation.
        /// </summary>
        protected virtual void EndSpriteBatch(GameTime gameTime)
        {
            SpriteManager.gfx.End();
        }

        /// <summary>
        /// The seventh operation in the draw pipeline.
        /// </summary>
        protected virtual void PostEndSpriteBatch(GameTime gameTime)
        {
        }

        /// <summary>
        /// The edth operation in the draw pipeline. SpriteBatch.Begin is called before this function call and SpriteBatch.End is called afterwards. No scaling, rotation, or translation are done on this step.
        /// </summary>
        protected virtual void UIDraw(GameTime gameTime)
        {
        }

        #endregion
    }
}
