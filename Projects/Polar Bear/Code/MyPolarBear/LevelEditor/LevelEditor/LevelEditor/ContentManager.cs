using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public class ContentManager
    {
        public static Dictionary<string, Texture2D> Textures;
        private static Dictionary<string, SpriteFont> Fonts;

        public ContentManager()
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
        }

        public static void Initialize()
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
        }

        public static void AddTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }

        public static void AddFont(string name, SpriteFont font)
        {
            Fonts.Add(name, font);
        }

        public static Texture2D GetTexture(string name)
        {
            return Textures[name];
        }

        public static SpriteFont GetFont(string name)
        {
            return Fonts[name];
        }
    }
}
