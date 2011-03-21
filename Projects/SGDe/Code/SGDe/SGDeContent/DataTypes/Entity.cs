using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SGDeContent.DataTypes.Sprites;

namespace SGDeContent.DataTypes
{
    public class Entity : DeveloperIDContent
    {
        //Node data
        public bool NonDefaultNode;
        public Node Node;

        public bool Enabled;
        public int UpdateOrder;
        public string Name;

        //Physics
        public Dictionary<string, object> Physics;

        //Sprite information
        public Sprite Sprite;
        public Vector2 SOffset;

        //Entity type data
        public string SpecialType;
        public List<object> Args;
        public Queue<string> ArgTypes;
    }
}
