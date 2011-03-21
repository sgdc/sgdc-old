using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace SGDeContent.DataTypes.Sprites
{
    public abstract class Sprite : ProcessedContent
    {
        public bool Visible;
        public int DrawOrder;
        public int SpriteID;
        public Color? Tint;

        public bool OffsetOrigin;
        public int AnimationID = -1;
        public bool AnimationLocal;
        public bool HasOverride;
        public SGDE.Graphics.Sprite.SpriteAttributes OverrideAttributes;
        public bool HasRegion;
        public int RegionBegin, RegionEnd;
        public bool BuiltInAnimation;
        public List<AnimationSet> Animations;
    }
}
