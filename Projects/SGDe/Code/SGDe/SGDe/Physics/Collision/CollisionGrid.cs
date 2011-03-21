using System;
using System.Collections.Generic;

namespace SGDE.Physics.Collision
{
    public partial class CollisionChief
    {
        /// <summary>
        /// The collision grid which all collision operations take place within.
        /// </summary>
        protected struct CollisionGrid
        {
            internal int NumXCells;
            internal int NumYCells;
            internal int NumTotalCells;
            internal CollisionCell[,] Cells;

            /// <summary>
            /// Instantiate a new CollisionGrid.
            /// </summary>
            /// <param name="numXCells">The number of cells horizontal within the grid.</param>
            /// <param name="numYCells">The number of cells vertically within the grid.</param>
            public CollisionGrid(int numXCells, int numYCells)
            {
                NumXCells = numXCells;
                NumYCells = numYCells;
                NumTotalCells = NumXCells * NumYCells;

                Cells = new CollisionCell[NumXCells, NumYCells];
                for (int i = 0; i < numXCells; i++)
                {
                    for (int j = 0; j < numYCells; j++)
                    {
                        Cells[i, j] = new CollisionCell();
                    }
                }
            }
        }
    }
}
