using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.GameScreens;
using MyPolarBear.Content;

namespace MyPolarBear
{
    class Text
    {
        private string _string;
        public string String
        {
            get { return _string; }
            set { _string = value; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private bool _active = false;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public Text(string text, Vector2 position)
        {
            String = text;
            Position = position;
            Color = Color.Black;
        }

        public Vector2 Size
        {
            get { return ContentManager.GetFont("Calibri").MeasureString(String); } 
        }

        public void Update()
        {
            if (Active && (Color == Color.Black))
                Color = Color.White;
            else if (!Active && (Color == Color.White))
                Color = Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.GetFont("Calibri"), String, Position, Color);
        }
    }
}