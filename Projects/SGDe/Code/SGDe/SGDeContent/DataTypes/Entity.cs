using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        public int SpriteID;
        public object SpriteDID;
        public int AnimationID = -1;
        public bool AnimationLocal;
        public Color? Tint;
        public bool HasOverride;
        public SGDE.Graphics.Sprite.SpriteAttributes OverrideAttributes;
        public bool HasRegion;
        public int RegionBegin, RegionEnd;
        public bool BuiltInAnimation;
        public List<AnimationSet> Animations;

        //Entity type data
        public string SpecialType;
        public List<object> Args;
        public Queue<string> ArgTypes;
    }
}
