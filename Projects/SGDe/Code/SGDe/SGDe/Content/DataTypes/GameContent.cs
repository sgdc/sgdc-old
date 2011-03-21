using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE.Physics;
using Microsoft.Xna.Framework;
using SGDE.Graphics;

namespace SGDE.Content.DataTypes
{
    /// <summary>
    /// Container for all game content.
    /// </summary>
    public sealed class GameContent
    {
        #region Fields

        private int levelIndex, cLevel;

        internal List<MapContent> maps;
        internal List<MapSettings> mapSettings;
        internal List<int> mapOrder;
        internal List<string> mapName;

        internal int width, height;
        internal bool fullScreen, vsync, multisample;
        internal bool? fixedTime;
        internal TimeSpan frameTime;
#if WINDOWS_PHONE
        internal DisplayOrientation orientation;
#elif WINDOWS
        internal bool resizeable, mouseVisible;
#endif
#if WINDOWS_PHONE || WINDOWS
        internal string title;
#endif

        private List<Entity> tDrawEntities;
        private List<Entity> tUpdateEntities;

        #endregion

        internal GameContent()
        {
        }

        #region Properties and Events

        /// <summary>
        /// Get the current level of the Game.
        /// </summary>
        public int CurrentLevel
        {
            get
            {
                return levelIndex;
            }
            internal set
            {
                levelIndex = value;
                cLevel = mapOrder[levelIndex];
                if (tUpdateEntities != null)
                {
                    EventHandler<EventArgs> ue = new EventHandler<EventArgs>(this.UpdateOrderChanged);
                    EventHandler<EventArgs> de = new EventHandler<EventArgs>(this.DrawOrderChanged);
                    Input.InputManager man = Game.CurrentGame.imanager;
                    foreach (Entity en in tUpdateEntities)
                    {
                        if (en is Input.InputHandler)
                        {
                            man.RemoveHandler((Input.InputHandler)en);
                        }
                        en.UpdateOrderChanged -= ue;
                        en.SpriteImage.DrawOrderChanged -= de;
                    }
                    tUpdateEntities = null;
                    tDrawEntities = null;
                }
            }
        }

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

        internal List<Entity> UpdateEntities
        {
            get
            {
                List<Entity> ents = new List<Entity>(this.maps[cLevel].uEntities);
                if (tUpdateEntities != null)
                {
                    ents.AddRange(tUpdateEntities);
                }
                return ents;
            }
        }

        internal List<Entity> DrawEntities
        {
            get
            {
                List<Entity> ents = new List<Entity>(this.maps[cLevel].dEntities);
                if (tDrawEntities != null)
                {
                    ents.AddRange(tDrawEntities);
                }
                return ents;
            }
        }

        /// <summary>
        /// Get the current Map name. Will be null if none exists.
        /// </summary>
        public string MapName
        {
            get
            {
                return this.mapName[cLevel];
            }
        }

        #endregion

        #region Temporary Entities

        /// <summary>
        /// Adds a entity to the current level. When the level is changed, the added entities will be removed.
        /// </summary>
        /// <param name="ent">The entitiy to add.</param>
        /// <returns><code>true</code> if the entitiy was added, <code>false</code> if otherwise.</returns>
        public bool AddEntity(Entity ent)
        {
            if (ent != null)
            {
                lock (this)
                {
                    if (this.tUpdateEntities == null)
                    {
                        this.tUpdateEntities = new List<Entity>();
                        this.tDrawEntities = new List<Entity>();
                    }
                    if (!this.tUpdateEntities.Contains(ent))
                    {
                        if (ent is Input.InputHandler)
                        {
                            if (!Game.CurrentGame.imanager.AddNewHandler((Input.InputHandler)ent))
                            {
                                return false;
                            }
                        }
                        //Add to update entities
                        AddOrderChanged(ent, ref this.tUpdateEntities, new EventHandler<EventArgs>(this.UpdateOrderChanged), false);
                        //Add to draw entities
                        AddOrderChanged(ent, ref this.tDrawEntities, new EventHandler<EventArgs>(this.DrawOrderChanged), true);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Remove an entity from the current level.
        /// </summary>
        /// <param name="ent">The entity to remove.</param>
        /// <returns><code>true</code> if the entitiy was removed, <code>false</code> if otherwise.</returns>
        public bool RemoveEntity(Entity ent)
        {
            if (ent != null)
            {
                if (this.tUpdateEntities != null)
                {
                    lock (this)
                    {
                        if (this.tUpdateEntities.Contains(ent))
                        {
                            if (!Game.CurrentGame.imanager.RemoveHandler((Input.InputHandler)ent))
                            {
                                return false;
                            }
                            ent.UpdateOrderChanged -= new EventHandler<EventArgs>(this.UpdateOrderChanged);
                            this.tUpdateEntities.Remove(ent);
                            ent.SpriteImage.DrawOrderChanged -= new EventHandler<EventArgs>(this.DrawOrderChanged);
                            this.tDrawEntities.Remove(ent);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void UpdateOrderChanged(object sender, EventArgs e)
        {
            OrderChanged((Entity)sender, ref this.tUpdateEntities, OrderComparer.Update);
        }

        private void DrawOrderChanged(object sender, EventArgs e)
        {
            OrderChanged((Entity)sender, ref this.tDrawEntities, OrderComparer.Draw);
        }

        internal static void AddOrderChanged(Entity ent, ref List<Entity> entities, EventHandler<EventArgs> handle, bool sprite)
        {
            IComparer<Entity> comp = sprite ? OrderComparer.Draw : OrderComparer.Update;
            int index = entities.BinarySearch(ent, comp);
            if (index < 0)
            {
                index = ~index;
                while ((index < entities.Count) && (comp.Compare(entities[index], ent) == 0))
                {
                    index++;
                }
                entities.Insert(index, ent);
                if (sprite)
                {
                    ent.SpriteImage.DrawOrderChanged += handle;
                }
                else
                {
                    ent.UpdateOrderChanged += handle;
                }
            }
        }

        internal static void OrderChanged(Entity ent, ref List<Entity> entities, IComparer<Entity> comp)
        {
            entities.Remove(ent);
            int index = entities.BinarySearch(ent, comp);
            if (index < 0)
            {
                index = ~index;
                while ((index < entities.Count) && (comp.Compare(entities[index], ent) == 0))
                {
                    index++;
                }
                entities.Insert(index, ent);
            }
        }

        #region OrderComparer

        internal sealed class OrderComparer : IComparer<Entity>
        {
            #region Static

            private static OrderComparer up, dr;

            public static OrderComparer Update
            {
                get
                {
                    if (up == null)
                    {
                        up = new OrderComparer(false);
                    }
                    return up;
                }
            }

            public static OrderComparer Draw
            {
                get
                {
                    if (dr == null)
                    {
                        dr = new OrderComparer(true);
                    }
                    return dr;
                }
            }

            #endregion

            private bool sprite;

            private OrderComparer(bool sprite)
            {
                this.sprite = sprite;
            }

            public int Compare(Entity x, Entity y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }
                if (x != null)
                {
                    if (y == null)
                    {
                        return -1;
                    }
                    object ox = sprite ? x.SpriteImage : (object)x;
                    object oy = sprite ? y.SpriteImage : (object)y;
                    if (ox.Equals(oy))
                    {
                        return 0;
                    }
                    int xi = sprite ? x.SpriteImage.DrawOrder : x.UpdateOrder;
                    int yi = sprite ? y.SpriteImage.DrawOrder : y.UpdateOrder;
                    if (xi < yi)
                    {
                        return -1;
                    }
                }
                return 1;
            }
        }

        #endregion

        #endregion

        #region Levels

        /// <summary>
        /// Goto a specific level.
        /// </summary>
        /// <param name="level">The zero-based index of the level to go to.</param>
        public void GotoLevel(int level)
        {
            if (level < 0 || level >= mapOrder.Count)
            {
                throw new IndexOutOfRangeException(Messages.GameContent_LevelNotExist);
            }
            //Do this to prevent a game from trying to access map details while a level is changing.
            lock (this)
            {
                CurrentLevel = level;
            }
            LevelSwitch(Game.CurrentGame);
        }

        /// <summary>
        /// Go to the next level based on level order, if possible.
        /// </summary>
        /// <returns><code>true</code> if level was progressed. <code>false</code> if couldn't load level, usually because the level order is done.</returns>
        public bool NextLevel()
        {
            if (CurrentLevel + 1 == mapOrder.Count)
            {
                return false;
            }
            lock (this)
            {
                CurrentLevel++;
            }
            LevelSwitch(Game.CurrentGame);
            return true;
        }

        private void LevelSwitch(Game game)
        {
            MapContent map = this.maps[cLevel];
            MapSettings settings = this.mapSettings[levelIndex];

            if (settings != null)
            {
                //Setup camera
                if (settings.CameraPosition.HasValue)
                {
                    game.camera.SetTranslation(settings.CameraPosition.Value);
                    game.camera._lastTrans = Vector2.Zero; //Reset movement so we don't end up with a infinite loop
                }

                //Setup graphics
                Graphics2D gfx = game.Graphics2D;
                if (settings.OrderSeperation.HasValue)
                {
                    gfx.OrderSeperation = settings.OrderSeperation.Value;
                }
                if (settings.CentralOrder.HasValue)
                {
                    gfx.CentralOrder = settings.CentralOrder.Value;
                }
            }

            //Initialize content
            foreach (Entity en in map.uEntities)
            {
                if (en is IGameComponent)
                {
                    ((IGameComponent)en).Initialize();
                }
            }
        }

        #endregion

        #region Content setup and Assets

        /// <summary>
        /// Setup one-time game settings.
        /// </summary>
        /// <param name="game">The Game that will be set.</param>
        internal void Setup(Game game)
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
        internal T GetMapContent<T>(string did)
        {
            return this.maps[cLevel].GetElement<T>(did);
        }

        /// <summary>
        /// Process all Code elements.
        /// </summary>
        /// <param name="game">The Game to process the Code elements on.</param>
        internal void Process(Game game)
        {
            LevelSwitch(game);
            //TODO
        }

        #endregion
    }
}
