using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE.Physics;

namespace SGDE.Content.DataTypes
{
    /// <summary>
    /// Used internally
    /// </summary>
    public class GameContent
    {
        /// <summary>
        /// Get the current level of the Game.
        /// </summary>
        public int CurrentLevel { get; internal set; }

        internal List<MapContent> maps;
        internal List<int> mapOrder;
        internal List<string> mapName;

        internal int width, height;
        internal bool fullScreen, vsync, multisample;
        internal bool? fixedTime;
        internal TimeSpan frameTime;
#if WINDOWS_PHONE
        internal Microsoft.Xna.Framework.DisplayOrientation orientation;
#elif WINDOWS
        internal bool resizeable, mouseVisible;
#endif
#if WINDOWS_PHONE || WINDOWS
        internal string title;
#endif

        /// <summary>
        /// Get the number of levels.
        /// </summary>
        public int NumberOfLevels
        {
            get
            {
                return maps.Count;
            }
        }

        /// <summary>
        /// Get the Entities for the specified map.
        /// </summary>
        public List<Entity> Entities
        {
            get
            {
                return this.maps[CurrentLevel].entities;
            }
        }

        /// <summary>
        /// Get the current Map name. Will be null if none exists.
        /// </summary>
        public string MapName
        {
            get
            {
                return this.mapName[CurrentLevel];
            }
        }

        /// <summary>
        /// Goto a specific level.
        /// </summary>
        /// <param name="level">The zero-based index of the level to go to.</param>
        public void GotoLevel(int level)
        {
            if (level < 0 || level >= maps.Count)
            {
                throw new IndexOutOfRangeException(Messages.GameContent_LevelNotExist);
            }
            //Do this to prevent a game from trying to access map details while a level is changing.
            lock (this)
            {
                CurrentLevel = level;
            }
        }

        /// <summary>
        /// Go to the next level based on level order, if possible.
        /// </summary>
        /// <returns><code>true</code> if level was progressed. <code>false</code> if couldn't load level, usually because the level order is done.</returns>
        public bool NextLevel()
        {
            int index = mapOrder.IndexOf(CurrentLevel);
            if (index + 1 == mapOrder.Count)
            {
                return false;
            }
            lock (this)
            {
                CurrentLevel = mapOrder[index + 1];
            }
            return true;
        }

        /// <summary>
        /// Setup one-time game settings.
        /// </summary>
        /// <param name="game">The Game that will be set.</param>
        public void Setup(ref Game game)
        {
            game.graphics.PreferredBackBufferWidth = this.width;
            game.graphics.PreferredBackBufferHeight = this.height;
            game.graphics.IsFullScreen = this.fullScreen;
            game.graphics.SynchronizeWithVerticalRetrace = this.vsync;
            if (this.fixedTime.HasValue)
            {
                game.IsFixedTimeStep = this.fixedTime.Value;
                if (this.fixedTime.Value)
                {
                    game.TargetElapsedTime = this.frameTime;
                }
            }
#if WINDOWS_PHONE
            game.graphics.SupportedOrientations = this.orientation;
#elif WINDOWS
            game.Window.AllowUserResizing = this.resizeable;
            game.IsMouseVisible = this.mouseVisible;
#endif
#if WINDOWS_PHONE || WINDOWS
            game.Window.Title = this.title;
#endif
            game.graphics.ApplyChanges();
        }

        /// <summary>
        /// Get content from the current map that has an developer ID (DID) assigned to it.
        /// </summary>
        /// <typeparam name="T">The type of content to load.</typeparam>
        /// <param name="did">The developer ID of the game content.</param>
        /// <returns>The game content, if it exists, or the default value of the requested type.</returns>
        public T GetMapContent<T>(string did)
        {
            return this.maps[CurrentLevel].GetElement<T>(did);
        }

        /// <summary>
        /// Process all Code elements.
        /// </summary>
        /// <param name="game">The Game to process the Code elements on.</param>
        public void Process(ref Game game)
        {
            //TODO
        }
    }
}
