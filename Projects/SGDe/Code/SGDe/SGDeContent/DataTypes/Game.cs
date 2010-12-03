﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes
{
    public class Game : ProcessedContent
    {
        //Resources
        public List<int> MapIDs;
        public List<object> Maps;

        //Settings
        public int FirstRun;
        public List<int> MapOrderId;
        public List<string> MapOrderName;

        //Game settings
        public int Width, Height;
        public bool Fullscreen;

        public Game()
        {
            this.Maps = new List<object>();
            this.MapIDs = new List<int>();
            this.MapOrderId = new List<int>();
            this.MapOrderName = new List<string>();
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
                for (int i = 0; i < MapIDs.Count; i++)
                {
                    if (MapIDs[i] != i)
                    {
                        int index = MapIDs.IndexOf(i);
                        object map = Maps[index];
                        Maps.RemoveAt(index);
                        Maps.Insert(index, map);
                        MapIDs.RemoveAt(i);
                        MapIDs.Insert(i, i);
                    }
                }
            }
        }
    }
}