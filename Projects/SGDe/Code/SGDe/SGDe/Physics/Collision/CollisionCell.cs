using System;
using System.Collections.Generic;

namespace SGDE.Physics.Collision
{
    public partial class CollisionChief
    {
        /// <summary>
        /// A cell of the CollisionGrid. This contains all the CollisionUnits that intercect this cell.
        /// </summary>
        protected partial class CollisionCell
        {
            /// <summary>
            /// A list of all the collision units that make up this cell.
            /// </summary>
            internal List<CollisionUnit> collisionMembers;

            /// <summary>
            /// Instantiate a new CollisionCell.
            /// </summary>
            public CollisionCell()
            {
                collisionMembers = new List<CollisionUnit>();
            }

            /// <summary>
            /// Add a new CollisionUnit to this CollisionCell.
            /// </summary>
            /// <param name="unit">The CollisionUnit to add.</param>
            public void AddCollisionUnit(CollisionUnit unit)
            {
                collisionMembers.Add(unit);
            }

            /// <summary>
            /// Remove a CollisionUnit from this CollisionCell.
            /// </summary>
            /// <param name="unit">The CollisionUnit to remove.</param>
            public void RemoveCollisionUnit(CollisionUnit unit)
            {
                collisionMembers.Remove(unit);
            }
        }
    }
}
