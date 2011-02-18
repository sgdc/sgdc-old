using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes.Sprites.SVG
{
    public abstract class Core : ProcessedContent
    {
        public string ID;

        protected Core()
            : this(null)
        {
        }

        protected Core(string id)
        {
            this.ID = id;
        }
    }
}
