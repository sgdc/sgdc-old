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
using SGDE.Audio;

namespace TestDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : SGDE.Game
    {
        BounceBall redBall;
        Texture2D mGridTexture;
        Texture2D mHitTexture;
        bool showCollision;

        internal SoundQueue mySoundQueue = new SoundQueue();
        internal SoundEffect ping;
        internal Song song;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            ping = Content.Load<SoundEffect>("ping");
            song = Content.Load<Song>("05 - Kids");

            showCollision = false;

            redBall = base.GetContent<BounceBall>("Vader");
            redBall.HandleCollisions = true;

            mHitTexture = base.GetContent<Texture2D>("Star");
            mGridTexture = base.GetContent<Texture2D>("Grid");
            base.GetContent<PortalBox>("PortalIn").SetOther(base.GetContent<PortalBox>("PortalOut"));

            mySoundQueue.PlayMusic(song, 0);
        }

        public void ToggleCollision()
        {
            showCollision = !showCollision;
        }

        protected override void PreBeginSpriteBatch(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);
        }

        protected override void PreDraw(GameTime gameTime)
        {
            if (showCollision)
            {
                PhysicsPharaoh.GetInstance().DrawCollisionGrid(mGridTexture);
            }
        }

        protected override void PostDraw(GameTime gameTime)
        {
            redBall.DrawHitSpot(this.SpriteBatch, mHitTexture);
        }
    }
}
