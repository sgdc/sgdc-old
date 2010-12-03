using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SGDeContent.DataTypes
{
    public class SpriteMap : ProcessedContent
    {
        public List<string> Names;
        public List<int> TextureIDs;
        public List<ExternalReference<TextureContent>> Textures;
        public List<List<AnimationSet>> AnimationSets;

        public SpriteMap()
        {
            Names = new List<string>();
            TextureIDs = new List<int>();
            Textures = new List<ExternalReference<TextureContent>>();
            AnimationSets = new List<List<AnimationSet>>();
        }
    }
}
