using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Pathfinding
{
    public class ANode : IComparable<ANode>
    {
        public static readonly int NOT_SPECIAL = 0;
        public static readonly int SEED_SOURCE = 1;
        public static readonly int PLANT_AREA = 2;

        public int Type;
        public bool bWalkable;
        public Vector2 Location;
        public int FCost;
        public int GCost;
        public int HCost;
        public ANode Parent;

        public bool bOpen;
        public bool bClosed;

        public ANode(Vector2 location)
        {
            Location = location;

            Type = NOT_SPECIAL;
            FCost = 0;
            GCost = 0;
            HCost = 0;
            Parent = null;
            bWalkable = true;
            bOpen = false;
            bClosed = false;
        }

        public int CompareTo(ANode other)
        {
            if (other == null)
            {
                return 1;
            }

            return FCost - other.FCost;
        }
    }
}
