using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyPolarBear
{
    class DrawKeeper
    {
        private static DrawKeeper _instance = null;
        private List<Entity> Entities;
        private List<Entity> entsToAdd;
        private List<Entity> entsToRemove;

        private List<LevelElement> LevelElements;
        private List<LevelElement> levElesToAdd;
        private List<LevelElement> levElesToRemove;

        private DrawKeeper()
        {
            Entities = new List<Entity>();
            entsToAdd = new List<Entity>();
            entsToRemove = new List<Entity>();

            LevelElements = new List<LevelElement>();
            levElesToAdd = new List<LevelElement>();
            levElesToRemove = new List<LevelElement>();
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

        public void addLevelElement(LevelElement ele)
        {
            levElesToAdd.Add(ele);
        }

        public void removeEntity(Entity ent)
        {
            entsToRemove.Add(ent);
        }

        public void removeLevelElement(LevelElement ele)
        {
            levElesToRemove.Add(ele);
        }

        public List<Entity> getEntities()
        {
            return Entities;
        }

        public List<LevelElement> getLevelElements()
        {
            return LevelElements;
        }

        public void drawAll(SpriteBatch spriteBatch)
        {
            // add entities
            foreach (Entity ent in entsToAdd)
            {
                Entities.Add(ent);
            }
            entsToAdd.Clear();

            // add level elements
            foreach (LevelElement ele in levElesToAdd)
            {
                LevelElements.Add(ele);
            }
            levElesToAdd.Clear();

            // remove entities
            foreach (Entity ent in entsToRemove)
            {
                Entities.Remove(ent);
            }
            entsToRemove.Clear();

            // remove level elements
            foreach (LevelElement ele in levElesToRemove)
            {
                LevelElements.Remove(ele);
            }
            levElesToRemove.Clear();

            // draw level elements
            foreach (LevelElement ele in LevelElements)
            {
                spriteBatch.Draw(ele.Tex, ele.Position, Color.White);
            }

            // draw entities
            foreach (Entity ent in Entities)
            {
                ent.Draw(spriteBatch);
            }
        }
    }
}
