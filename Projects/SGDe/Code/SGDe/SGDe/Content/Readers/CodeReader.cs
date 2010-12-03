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
    /// Read and process a Code class
    /// </summary>
    public class CodeReader : ContentTypeReader<SGDE.Content.Code.Code>
    {
        /// <summary>
        /// Read a Code.
        /// </summary>
        protected override SGDE.Content.Code.Code Read(ContentReader input, SGDE.Content.Code.Code existingInstance)
        {
            SGDE.Content.Code.Code code = new SGDE.Content.Code.Code();
            if (input.ReadBoolean())
            {
                code.ConstantValue = input.ReadObject<object>();
            }
            else
            {
                //TODO
            }
            return code;
        }
    }
}
