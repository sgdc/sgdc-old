using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear
{
    class DrawKeeper
    {
        private static DrawKeeper _instance = null;
        private List<Entity> Entities;
        private List<Entity> entsToAdd;
        private List<Entity> entsToRemove;

        private DrawKeeper()
        {
            Entities = new List<Entity>();
            entsToAdd = new List<Entity>();
            entsToRemove = new List<Entity>();
        }

        public static DrawKeeper getInstance()
        {
            if (_instance == null)
            {
                _instance = new DrawKeeper();
            }

            return _instance;
        }

        public void addEntity(Entity ent)
        {
            entsToAdd.Add(ent);
        }

        public void removeEntity(Entity ent)
        {
            entsToRemove.Add(ent);
        }

        public List<Entity> getEntities()
        {
            return Entities;
        }

        public void drawAll(SpriteBatch spriteBatch)
        {
            foreach (Entity ent in entsToAdd)
            {
                Entities.Add(ent);
            }
            entsToAdd.Clear();

            foreach (Entity ent in entsToRemove)
            {
                Entities.Remove(ent);
            }
            entsToRemove.Clear();

            foreach (Entity ent in Entities)
            {
                ent.Draw(spriteBatch);
            }
        }
    }
}
