using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SGDeContent.DataTypes;
using SGDeContent.DataTypes.Code;

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class SpriteSheetWriter : ContentTypeWriter<SpriteSheet>
    {
        protected override void Write(ContentWriter output, SpriteSheet value)
        {
            output.WriteObject(value.Textures);
            output.Write(value.AnimationSets.Count);
            foreach (List<AnimationSet> set in value.AnimationSets)
            {
                if (set == null)
                {
                    output.Write(-1);
                    continue;
                }
                output.Write(set.Count);
                foreach (AnimationSet aset in set)
                {
                    output.WriteObject(aset);
                    if (aset.Did.ContainsKey(aset))
                    {
                        output.Write(aset.Did[aset]);
                    }
                    else
                    {
                        output.Write(string.Empty);
                    }
                }
            }
            output.WriteObject(value.Fonts);
        }

        public override int TypeVersion
        {
            get
            {
                return 2;
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.SpriteManagerReader).AssemblyQualifiedName;
        }
    }
}
