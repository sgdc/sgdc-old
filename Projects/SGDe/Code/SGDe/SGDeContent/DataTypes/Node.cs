using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SGDeContent.DataTypes
{
    public class Node : ProcessedContent
    {
        public Vector2 Translation, Scale;
        public SGDeContent.DataTypes.Code.Code tx, ty, sx, sy;
        public float Rotation;
        public List<Node> Children;

        public Node()
        {
            Children = new List<Node>();
        }
    }
}
