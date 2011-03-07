﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework;

namespace SGDeContent.DataTypes
{
    public class Game : ProcessedContent
    {
        //Resources
        public List<int> MapIDs;
        public List<object> Maps;
        public List<Vector2?> MapCameraStart;

        //Settings
        public int FirstRun;
        public List<int> MapOrderId;
        public List<string> MapOrderName;
        public ExternalReference<SpriteSheet> SpriteSheet;

        //Game settings
        public int Width, Height;
        public bool Fullscreen, VSync, WindowResize, Multisample, MouseVisible;
        public bool? FixedTime;
        public DisplayOrientation Orientation;
        public string Title;
        public TimeSpan FrameTime;

        public Game()
        {
            this.Maps = new List<object>();
            this.MapIDs = new List<int>();
            this.MapCameraStart = new List<Vector2?>();
            this.MapOrderId = new List<int>();
            this.MapOrderName = new List<string>();

            this.Multisample = true; //Hey, if it's avalible, great. If not, then it's ignored by XNA.
            this.Title = string.Empty;
            this.FrameTime = TimeSpan.Zero;
        }

        public void Sort()
        {
            //First check to see if out of order
            bool sort = false;
            for (int i = 0; i < MapIDs.Count; i++)
            {
                if (MapIDs[i] != i)
                {
                    sort = true;
                    break;
                }
            }
            if (sort)
            {
                //Need to sort the maps. This way it provides easy validation when loading in game.
                List<int> ids = new List<int>();
                List<object> tmaps = new List<object>();
                List<Vector2?> tmapCameraStart = new List<Vector2?>();
                for (int i = 0; i < MapIDs.Count; i++)
                {
                    int index = MapIDs.IndexOf(i);
                    ids.Add(i);
                    tmaps.Add(Maps[index]);
                    tmapCameraStart.Add(MapCameraStart[index]);
                }
                MapIDs.Clear();
                MapIDs.AddRange(ids);
                Maps.Clear();
                Maps.AddRange(tmaps);
                MapCameraStart.Clear();
                MapCameraStart.AddRange(tmapCameraStart);
            }
        }
    }
}
