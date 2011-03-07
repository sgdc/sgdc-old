using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyPolarBear
{
    class UpdateKeeper
    {
        private static UpdateKeeper _instance = null;
        private List<Entity> Entities;
        private List<Entity> entsToAdd;
        private List<Entity> entsToRemove;

        private List<LevelElement> LevelElements;
        private List<LevelElement> levElesToAdd;
        private List<LevelElement> levElesToRemove;

        private UpdateKeeper()
        {
            Entities = new List<Entity>();
            entsToAdd = new List<Entity>();
            entsToRemove = new List<Entity>();

            LevelElements = new List<LevelElement>();
            levElesToAdd = new List<LevelElement>();
            levElesToRemove = new List<LevelElement>();
        }

        public static UpdateKeeper getInstance()
        {
            if (_instance == null)
            {
                _instance = new UpdateKeeper();
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

        public void updateAll(GameTime gameTime)
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

            // update entities
            foreach (Entity ent in Entities)
            {
                ent.Update(gameTime);
            }
        }
    }
}
