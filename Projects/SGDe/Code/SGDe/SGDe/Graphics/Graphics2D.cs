using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SGDE.Graphics
{
    /// <summary>
    /// 2D graphics processor.
    /// </summary>
    public sealed class Graphics2D : SpriteBatch
    {
        private Game game;
        private SpriteFont font;
        private string fontName;
        private float seperation;

        internal Graphics2D(Game game)
            : base(game.GraphicsDevice)
        {
            this.game = game;
        }

        #region Properties

        /// <summary>
        /// Get or set layer seperation. This is a linear scale based on draw order and counts the difference in pixel movement. For example if the order separation is 2, then for every two pixels that the
        /// <see cref="CentralOrder"/> moves, the layer behind (lower order number) it moves one pixel. The layer in front of it moves four pixels.
        /// </summary>
        public float OrderSeperation
        {
            get
            {
                return this.seperation;
            }
            set
            {
                if (!(float.IsNaN(value) || float.IsInfinity(value)))
                {
                    this.seperation = value;
                }
            }
        }

        /// <summary>
        /// Get or set the central layer. This is the layer that always moves one-for-one with the camera.
        /// </summary>
        public int CentralOrder { get; set; }

        /// <summary>
        /// Get or set the font to use for DrawString operations. If the name is not valid then the font will be null. This can cause errors if a DrawSTring function is used.
        /// </summary>
        public string Font
        {
            get
            {
                return this.fontName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (this.fontName != null && this.fontName.Equals(value))
                    {
                        return;
                    }
                    this.fontName = value;
                    this.font = SpriteManager.GetInstance().GetFont(this.fontName);
                    if (this.font == null)
                    {
                        this.fontName = null;
                    }
                }
            }
        }

        #endregion

        #region DrawString

        /// <summary>
        /// Draw a String.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public void DrawString(string text, Vector2 position, Color color)
        {
            DrawString(this.font, text, position, color);
        }

        /// <summary>
        /// Draw a String.
        /// </summary>
        /// <param name="spriteFont">A font for diplaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public new void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            if (spriteFont == null)
            {
                spriteFont = this.font;
            }
            base.DrawString(spriteFont, text, position, color);
        }

        /// <summary>
        /// Draw a StringBuilder.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public void DrawString(StringBuilder text, Vector2 position, Color color)
        {
            base.DrawString(this.font, text, position, color);
        }

        /// <summary>
        /// Draw a StringBuilder.
        /// </summary>
        /// <param name="spriteFont">A font for diplaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        public new void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            if (spriteFont == null)
            {
                spriteFont = this.font;
            }
            base.DrawString(spriteFont, text, position, color);
        }

        /// <summary>
        /// Draw a String.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public void DrawString(string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(this.font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a String.
        /// </summary>
        /// <param name="spriteFont">A font for diplaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public new void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                spriteFont = this.font;
            }
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a String.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public void DrawString(string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(this.font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a String.
        /// </summary>
        /// <param name="spriteFont">A font for diplaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public new void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                spriteFont = this.font;
            }
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a StringBuilder.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public void DrawString(StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(this.font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a StringBuilder.
        /// </summary>
        /// <param name="spriteFont">A font for diplaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public new void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                spriteFont = this.font;
            }
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a StringBuilder.
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public void DrawString(StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            base.DrawString(this.font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draw a StringBuilder.
        /// </summary>
        /// <param name="spriteFont">A font for diplaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer.</param>
        public new void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (spriteFont == null)
            {
                spriteFont = this.font;
            }
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        #endregion
    }
}
