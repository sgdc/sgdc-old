﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeContent.DataTypes
{
    public class Map : DeveloperIDContent, IComparer<int[]>
    {
        //Resources
        public List<int> EntityID;
        public List<object> Entities;

        //Map
        public List<object[]> MapComponents;
        public List<int> SortUpdate;
        public List<int> SortDraw;

        //Physics
        public bool Physics;
        public SGDeContent.DataTypes.Code.Code Physics_CellSize_Width, Physics_CellSize_Height, Physics_World_Width, Physics_World_Height;
        public Vector2 Physics_Gravity;

        public Map()
        {
            EntityID = new List<int>();
            Entities = new List<object>();
            MapComponents = new List<object[]>();
            SortUpdate = new List<int>();
            SortDraw = new List<int>();
            Physics = true;
        }

        public void Validate(ContentProcessorContext context)
        {
            //First check to see if out of order
            bool process = false;
            for (int i = 0; i < EntityID.Count; i++)
            {
                if (EntityID[i] != i)
                {
                    process = true;
                    break;
                }
            }
            if (process)
            {
                //Need to sort the entities. This way it provides easy validation when loading in game.
                List<int> ids = new List<int>();
                List<object> tent = new List<object>();
                for (int i = 0; i < EntityID.Count; i++)
                {
                    ids.Add(i);
                    tent.Add(Entities[EntityID.IndexOf(i)]);
                }
                EntityID.Clear();
                EntityID.AddRange(ids);
                Entities.Clear();
                Entities.AddRange(tent);
            }

            process = false;
            List<int> usedComponents = new List<int>();
            for (int i = 0; i < MapComponents.Count; i++)
            {
                object[] component = MapComponents[i];
                switch (component.Length)
                {
                    /*case 1:
                        //Operation component
                        break;*/
                    case 2:
                        //Entity position
                        int id = (int)component[0];
                        if (!usedComponents.Contains(id))
                        {
                            if (EntityID.Contains(id))
                            {
                                usedComponents.Add(id);
                            }
                            else
                            {
                                throw new InvalidContentException(Messages.EntityIDDoesNotExist);
                            }
                        }
                        break;
                }
            }
            if (usedComponents.Count != EntityID.Count)
            {
                //Remove unused entities
                
                //First make sure it doesn't have a developer defined ID and isn't an external reference (since we can't check those)
                List<int> remove = new List<int>();
                for (int i = 0; i < EntityID.Count; i++)
                {
                    if (!usedComponents.Contains(EntityID[i]))
                    {
                        int id = EntityID[i];
                        if (!(Entities[id] is ExternalReference<Entity>))
                        {
                            if (((Entity)Entities[id]).Did.Count == 0)
                            {
                                remove.Add(id);
                            }
                        }
                        if (!remove.Contains(id))
                        {
                            usedComponents.Add(id);
                        }
                    }
                }

                if (usedComponents.Count != EntityID.Count)
                {
                    //Now that all (possibly) required and required components are accounted for, remove the unused ones
                    remove.Reverse(); //This makes it easier to go through
                    foreach (int removeID in remove)
                    {
                        if (EntityID[EntityID.Count - 1] == removeID)
                        {
                            //Simple, just remove the entity
                            Entities.RemoveAt(EntityID.Count - 1);
                            EntityID.RemoveAt(EntityID.Count - 1);
                        }
                        else
                        {
                            //Complex, need to restructure the entity
                            List<int> ignore = new List<int>();
                            for (int i = EntityID.Count - 1; i >= 0; i--)
                            {
                                int id = EntityID[i];
                                if (id == removeID)
                                {
                                    EntityID.RemoveAt(i);
                                    Entities.RemoveAt(i);
                                    break;
                                }
                                else
                                {
                                    for (int k = 0; k < MapComponents.Count; k++)
                                    {
                                        if (!ignore.Contains(k))
                                        {
                                            object[] component = MapComponents[k];
                                            switch (component.Length)
                                            {
                                                /*case 1:
                                                    //Operation component
                                                    break;*/
                                                case 2:
                                                    //Entity position
                                                    int idi = (int)component[0];
                                                    if (idi == id)
                                                    {
                                                        ignore.Add(k);
                                                        component[0] = idi - 1;
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    EntityID[i]--;
                                }
                            }
                        }
                    }
                }
            }

            //Double check physics
            if (Physics)
            {
                if (Physics_CellSize_Height == null)
                {
                    Physics_CellSize_Height = new Code.Code();
                    Physics_CellSize_Height.Constant = true;
                    Physics_CellSize_Height.ConstantValue = 0;
                }
                if (Physics_CellSize_Width == null)
                {
                    Physics_CellSize_Width = new Code.Code();
                    Physics_CellSize_Width.Constant = true;
                    Physics_CellSize_Width.ConstantValue = 0;
                }
                if (Physics_World_Height == null)
                {
                    Physics_World_Height = new Code.Code();
                    Physics_World_Height.Constant = true;
                    Physics_World_Height.ConstantValue = 0;
                }
                if (Physics_World_Width == null)
                {
                    Physics_World_Width = new Code.Code();
                    Physics_World_Width.Constant = true;
                    Physics_World_Width.ConstantValue = 0;
                }
            }

            //Sort the components
            List<int[]> items = new List<int[]>();

            for (int i = 0; i < MapComponents.Count; i++)
            {
                object[] component = MapComponents[i];
                switch (component.Length)
                {
                    case 1:
                        //Operation component
                        items.Add(new int[] { i, 0 });
                        break;
                    case 2:
                        //Entity position
                        items.Add(new int[] { i, GetUpdateOrder(i, (int)component[0]) });
                        break;
                }
            }
            items.Sort(this);
            for (int i = 0; i < MapComponents.Count; i++)
            {
                this.SortUpdate.Add(items[i][0]);
            }

            items.Clear();

            for (int i = 0; i < MapComponents.Count; i++)
            {
                object[] component = MapComponents[i];
                switch (component.Length)
                {
                    case 1:
                        //Operation component
                        items.Add(new int[] { i, 0 });
                        break;
                    case 2:
                        //Entity position
                        items.Add(new int[] { i, GetDrawOrder(i, (int)component[0]) });
                        break;
                }
            }
            items.Sort(this);
            for (int i = 0; i < MapComponents.Count; i++)
            {
                this.SortDraw.Add(items[i][0]);
            }
        }

        private int GetUpdateOrder(int index, int id)
        {
            int order = 0;
            bool gotOrder = false;
            //First try map component
            object[] component = this.MapComponents[index];
            switch (component.Length)
            {
                case 2:
                    if (component[1] != null)
                    {
                        if (component[1] is Entity)
                        {
                            order = ((Entity)component[1]).UpdateOrder;
                            gotOrder = true;
                        }
                    }
                    break;
            }
            if (!gotOrder)
            {
                //Next try getting it from the original component
                if (this.Entities[id] is Entity)
                {
                    order = ((Entity)this.Entities[id]).UpdateOrder;
                    gotOrder = true;
                }
            }
            return order;
        }

        private int GetDrawOrder(int index, int id)
        {
            int order = 0;
            bool gotOrder = false;
            //First try map component
            object[] component = this.MapComponents[index];
            switch (component.Length)
            {
                case 2:
                    if (component[1] != null)
                    {
                        if (component[1] is Entity)
                        {
                            Sprites.Sprite sp = ((Entity)component[1]).Sprite;
                            if (sp != null)
                            {
                                order = sp.DrawOrder;
                                gotOrder = true;
                            }
                        }
                    }
                    break;
            }
            if (!gotOrder)
            {
                //Next try getting it from the original component
                if (this.Entities[id] is Entity)
                {
                    Sprites.Sprite sp = ((Entity)this.Entities[id]).Sprite;
                    if (sp != null)
                    {
                        order = sp.DrawOrder;
                        gotOrder = true;
                    }
                }
            }
            return order;
        }

        public int Compare(int[] x, int[] y)
        {
            return x[1] - y[1];
        }
    }
}
