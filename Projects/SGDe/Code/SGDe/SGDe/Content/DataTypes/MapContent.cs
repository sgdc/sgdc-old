using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE.Physics;
using Microsoft.Xna.Framework;

namespace SGDE.Content.DataTypes
{
    /// <summary>
    /// Used internally.
    /// </summary>
    internal class MapContent
    {
        //Physics
        internal bool enPhysics;
        internal Vector2 physicsGravity, physicsWorldSize, physicsCellSize;

        //Entities
        internal List<EntityBuilder> builders;

        internal List<Entity> dEntities;
        internal List<Entity> uEntities;

        internal Dictionary<string, object> developerTypes;

        /// <summary>
        /// Create a new MapContent object.
        /// </summary>
        public MapContent()
        {
            developerTypes = new Dictionary<string, object>();
        }

        internal void UpdateOrderChanged(object sender, EventArgs e)
        {
            GameContent.OrderChanged((Entity)sender, ref this.uEntities, GameContent.OrderComparer.Update);
        }

        internal void DrawOrderChanged(object sender, EventArgs e)
        {
            GameContent.OrderChanged((Entity)sender, ref this.dEntities, GameContent.OrderComparer.Draw);
        }

        internal void Sort()
        {
            //This shouldn't take too long since the content pipeline semi-orders the entities before they are written
            uEntities.Sort(GameContent.OrderComparer.Update);
            dEntities.Sort(GameContent.OrderComparer.Draw);
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

    internal class MapSettings
    {
        public Vector2? CameraPosition;
        public float? CameraRotation;
        public Vector2? CameraScale;
        public Vector4? CameraBounds;
        public float? OrderSeperation;
        public int? CentralOrder;
    }
}
