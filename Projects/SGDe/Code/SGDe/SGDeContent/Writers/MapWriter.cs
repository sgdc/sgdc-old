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
    public class MapWriter : ContentTypeWriter<Map>
    {
        protected override void Write(ContentWriter output, Map value)
        {
            //Write out Physics
            output.Write(value.Physics);
            if (value.Physics)
            {
                output.WriteRawObject(value.Physics_CellSize_Width);
                output.WriteRawObject(value.Physics_CellSize_Height);
                output.WriteRawObject(value.Physics_World_Width);
                output.WriteRawObject(value.Physics_World_Height);
                output.Write(value.Physics_Gravity);
            }
            int count = value.EntityID.Count;
            //Write out resources
            output.Write(count);
            value.DeveloperIDWriter(output, value.Entities);
            for (int i = 0; i < count; i++)
            {
                object entity = value.Entities[i];
                if (entity is Entity)
                {
                    output.Write(true);
                    output.WriteRawObject(entity as Entity);
                }
                else
                {
                    output.Write(false);
                    output.WriteExternalReference(entity as ExternalReference<Entity>);
                }
                value.DeveloperIDWriter(output, entity);
            }
            //Write out Map
            count = value.MapComponents.Count;
            output.Write(count);
            for (int i = 0; i < count; i++)
            {
                object[] entities = value.MapComponents[i];
                output.Write(entities.Length - 1);
                switch (entities.Length)
                {
                    case 1:
                        output.WriteRawObject(entities[0] as Code);
                        break;
                    case 2:
                        output.Write((int)entities[0]);
                        output.WriteObject(entities[1]);
                        break;
                }
                value.DeveloperIDWriter(output, entities);
            }
            //Write out sorting order
            output.WriteObject(value.SortUpdate);
            output.WriteObject(value.SortDraw);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.MapReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    internal class MapSettingsWriter : ContentTypeWriter<SGDE.Content.DataTypes.MapSettings>
    {
        protected override void Write(ContentWriter output, SGDE.Content.DataTypes.MapSettings value)
        {
            output.WriteObject(value.CameraPosition);
            output.WriteObject(value.CameraRotation);
            output.WriteObject(value.CameraScale);
            output.WriteObject(value.CameraBounds);
            output.WriteObject(value.OrderSeperation);
            output.WriteObject(value.CentralOrder);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.MapSettingsReader).AssemblyQualifiedName;
        }
    }
}
