using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDeContent.DataTypes
{
    public class Animation : ProcessedContent
    {
        public bool BuiltIn;
        public int ID;
        public List<AnimationSet> Sets;
    }
}
