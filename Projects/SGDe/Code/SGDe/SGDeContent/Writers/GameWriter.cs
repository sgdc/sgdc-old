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
            output.Write(value.VSync);
            output.Write(value.Multisample);
            output.WriteObject(value.FixedTime);
            if (value.FixedTime.HasValue && value.FixedTime.Value)
            {
                output.WriteObject(value.FrameTime);
            }
            switch (output.TargetPlatform)
            {
                case TargetPlatform.WindowsPhone:
                    output.WriteObject(value.Orientation);
                    break;
                case TargetPlatform.Windows:
                    output.Write(value.WindowResize);
                    output.Write(value.MouseVisible);
                    break;
            }
            switch (output.TargetPlatform)
            {
                case TargetPlatform.WindowsPhone:
                case TargetPlatform.Windows:
                    output.Write(value.Title);
                    break;
            }
            //Write out the resources
            //-Write out SpriteSheet
            output.WriteExternalReference(value.SpriteSheet);
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
                output.WriteObject(value.MapSettings[i]);
            }
            output.Write(value.FirstRun);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.GameReader).AssemblyQualifiedName;
        }
    }
}
