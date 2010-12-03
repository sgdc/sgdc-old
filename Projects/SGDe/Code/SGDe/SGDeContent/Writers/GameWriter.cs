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

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class GameWriter : ContentTypeWriter<SGDeContent.DataTypes.Game>
    {
        protected override void Write(ContentWriter output, SGDeContent.DataTypes.Game value)
        {
            //Write out the actual game settings
            if (value.Width > 0)
            {
                output.Write(true);
                output.Write(value.Width);
            }
            else
            {
                output.Write(false);
            }
            if (value.Height > 0)
            {
                output.Write(true);
                output.Write(value.Height);
            }
            else
            {
                output.Write(false);
            }
            output.Write(value.Fullscreen);
            //Write out the resources
            //-Write out the maps
            output.Write(value.Maps.Count);
            foreach (object map in value.Maps)
            {
                if (map is Map)
                {
                    output.Write(true);
                    output.WriteRawObject(map as Map);
                }
                else
                {
                    output.Write(false);
                    output.WriteExternalReference(map as ExternalReference<Map>);
                }
            }
            //Write out the game settings (the compoenent that actually describes the game)
            output.Write(value.FirstRun);
            output.Write(value.MapOrderId.Count);
            for (int i = 0; i < value.MapOrderId.Count; i++)
            {
                output.Write(value.MapOrderId[i]);
                string name = value.MapOrderName[i];
                if (string.IsNullOrWhiteSpace(name))
                {
                    output.Write(false);
                }
                else
                {
                    output.Write(true);
                    output.Write(name);
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.GameReader).AssemblyQualifiedName;
        }
    }
}
