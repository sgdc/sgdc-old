using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;
using SGDE.Graphics.SVG;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a SVG class
    /// </summary>
    internal class SVGReader : ContentTypeReader<SVG>
    {
        /// <summary>
        /// Read a SVG.
        /// </summary>
        protected override SVG Read(ContentReader input, SVG existingInstance)
        {
            return null;
        }
    }
}
