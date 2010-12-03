using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE.Physics;

namespace SGDE.Content.DataTypes
{
    /// <summary>
    /// Used internally.
    /// </summary>
    public class MapContent
    {
        internal List<EntityBuilder> builders;

        internal List<Entity> entities;

        internal Dictionary<string, object> developerTypes;

        /// <summary>
        /// Create a new MapContent object.
        /// </summary>
        public MapContent()
        {
            developerTypes = new Dictionary<string, object>();
        }

        internal T GetElement<T>(string did)
        {
            if (developerTypes.ContainsKey(did))
            {
                object obj = developerTypes[did];
                if (obj is T)
                {
                    return (T)obj;
                }
                if (obj is SceneNode)
                {
                    SceneNode node = obj as SceneNode;
                    return node.GetAsType<T>();
                }
            }
            return default(T);
        }
    }
}
