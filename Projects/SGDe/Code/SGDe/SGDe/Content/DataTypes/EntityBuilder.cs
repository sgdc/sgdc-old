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
    internal class EntityBuilder
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

        internal Entity Create(Entity mod)
        {
            return Create(mod, null);
        }

        internal Entity Create(Entity mod, Dictionary<string, object> physics)
        {
            //Create a new Entity
            Entity ent = SGDE.Content.Readers.EntityReader.CreateEntityInstance(BaseEntity.GetType(), BaseEntity.args);
            ent.Enabled = BaseEntity.Enabled;
            //Copy over attributes
            BaseEntity.CopyTo(ref ent);
            SGDE.Content.Readers.EntityReader.ProcessPhysics(ref ent, this.physics, false);
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
                SGDE.Content.Readers.EntityReader.ProcessPhysics(ref ent, physics, this.physics != null && physics != null);
                ent.Enabled = mod.Enabled;
                if (mod.SpriteImage != null)
                {
                    SGDE.Graphics.Sprite sp = ent.SpriteImage;
                    mod.SpriteImage.CopySpriteToIn(ref sp, false);
                }
            }
            return ent;
        }
    }
}
