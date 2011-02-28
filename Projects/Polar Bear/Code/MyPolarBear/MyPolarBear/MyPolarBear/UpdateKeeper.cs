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

        private UpdateKeeper()
        {
            Entities = new List<Entity>();
            entsToAdd = new List<Entity>();
            entsToRemove = new List<Entity>();
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

        public void removeEntity(Entity ent)
        {
            entsToRemove.Add(ent);
        }

        public List<Entity> getEntities()
        {
            return Entities;
        }

        public void updateAll(GameTime gameTime)
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
                ent.Update(gameTime);
            }
        }
    }
}
