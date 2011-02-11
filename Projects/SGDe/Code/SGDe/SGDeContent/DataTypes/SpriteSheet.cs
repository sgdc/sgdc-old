using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SGDeContent.DataTypes
{
    public class SpriteSheet : ProcessedContent
    {
        public List<string> Names;
        public List<int> TextureIDs;
        public List<object> Textures;
        public List<List<AnimationSet>> AnimationSets;

        public SpriteSheet()
        {
            Names = new List<string>();
            TextureIDs = new List<int>();
            Textures = new List<object>();
            AnimationSets = new List<List<AnimationSet>>();
        }
    }
}
