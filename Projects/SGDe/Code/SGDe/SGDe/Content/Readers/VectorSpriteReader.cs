using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using SGDE.Graphics;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a VectorSprite class
    /// </summary>
    internal class VectorSpriteReader : SpriteReader<VectorSprite>
    {
        public override VectorSprite CreateInstance()
        {
            return new VectorSprite();
        }

        public override void HandleSpecific(ContentReader input, int id, VectorSprite instance)
        {
            //TODO
        }

        //TODO: Apply the read in values seperetly so that EntityBuilder can process them
    }
}
