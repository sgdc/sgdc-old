using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a GameContent class
    /// </summary>
    public class GameReader : ContentTypeReader<GameContent>
    {
        /// <summary>
        /// Read a GameContent.
        /// </summary>
        protected override GameContent Read(ContentReader input, GameContent existingInstance)
        {
            GameContent content = new GameContent(); //existingInstance == null ? new GameContent() : existingInstance;
            if (ContentUtil.CurrentGameContent != null)
            {
                throw new InvalidOperationException(Messages.GameReader_OnlyOneGameContent);
            }
            ContentUtil.CurrentGameContent = content;
            //Read game settings (screen resoultion, etc.)
            GraphicsDeviceManager manager = (GraphicsDeviceManager)input.ContentManager.ServiceProvider.GetService(typeof(Microsoft.Xna.Framework.IGraphicsDeviceManager));
            if (input.ReadBoolean())
            {
                content.width = input.ReadInt32();
            }
            else
            {
                content.width = manager.PreferredBackBufferWidth;
            }
            if (input.ReadBoolean())
            {
                content.height = input.ReadInt32();
            }
            else
            {
                content.height = manager.PreferredBackBufferHeight;
            }
            content.fullScreen = input.ReadBoolean();
            //Read SpriteSheet
            ContentUtil.PrepTempDID();
            SpriteManager.GetInstance(input.ContentManager); //Not fully necessery but useful
            Dictionary<string, object> did = new Dictionary<string, object>();
            ContentUtil.FinishTempDID(ref did);
            //Read Game content
            int count = input.ReadInt32();
            content.maps = new List<MapContent>(count);
            for (int i = 0; i < count; i++)
            {
                MapContent mapContent;
                if (input.ReadBoolean())
                {
                    mapContent = input.ReadRawObject<MapContent>();
                }
                else
                {
                    mapContent = input.ReadExternalReference<MapContent>();
                }
                if (did.Count > 0)
                {
                    Dictionary<string, object>.Enumerator en = did.GetEnumerator();
                    while (en.MoveNext())
                    {
                        KeyValuePair<string, object> value = en.Current;
                        mapContent.developerTypes.Add(value.Key, value.Value);
                    }
                }
                content.maps.Add(mapContent);
            }
            //Read game settings (map order, etc.)
            content.CurrentLevel = input.ReadInt32();
            count = input.ReadInt32();
            content.mapOrder = new List<int>(count);
            content.mapName = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                content.mapOrder.Add(input.ReadInt32());
                if (input.ReadBoolean())
                {
                    content.mapName.Add(input.ReadString());
                }
                else
                {
                    content.mapName.Add(null);
                }
            }
            return content;
        }
    }
}
