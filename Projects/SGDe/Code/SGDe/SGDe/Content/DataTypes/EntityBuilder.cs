using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SGDE.Content.DataTypes
{
    /// <summary>
    /// Used Internally
    /// </summary>
    public class EntityBuilder
    {
        private Entity BaseEntity;
        private Dictionary<string, object> physics;

        internal EntityBuilder(Entity baseEntity, Dictionary<string, object> physics)
        {
            this.BaseEntity = baseEntity;
            this.physics = physics;
        }

        internal Entity Create()
        {
            return Create(null, null);
        }

        internal Entity Create(Dictionary<string, object> physics)
        {
            return Create(null, physics);
        }

        internal Entity Create(Entity mod)
        {
            return Create(mod, null);
        }

        internal Entity Create(Entity mod, Dictionary<string, object> physics)
        {
            //Create a new Entity
            Entity ent = (Entity)Activator.CreateInstance(BaseEntity.GetType(), BaseEntity.args);
            //Copy over attributes
            BaseEntity.CopyTo(ref ent);
            SGDE.Content.Readers.EntityReader.ProcessPhysics(ref ent, this.physics);
            if (mod != null)
            {
                //Apply modifications
                if (mod.GetRotation() != ent.GetRotation())
                {
                    ent.SetRotation(mod.GetRotation());
                }
                if (mod.GetScale() != ent.GetScale())
                {
                    ent.SetScale(mod.GetScale());
                }
                if (mod.GetTranslation() != ent.GetTranslation())
                {
                    ent.SetTranslation(mod.GetTranslation());
                }
                SGDE.Content.Readers.EntityReader.ProcessPhysics(ref ent, physics);
                //mod.CopySpriteTo(ref ent); //Copies modified sprite information over
            }
            return ent;
        }
    }
}
