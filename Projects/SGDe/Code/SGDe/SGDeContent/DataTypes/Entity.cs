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

        public string Name;

        //Physics
        public Dictionary<string, object> Physics;

        //Sprite information
        public Sprite Sprite;

        //Entity type data
        public string SpecialType;
        public List<object> Args;
        public Queue<string> ArgTypes;
    }
}
