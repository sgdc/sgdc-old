using System;
using System.Collections.Generic;

namespace SGDE.Physics.Collision
{
   public partial class CollisionChief
   {
      protected struct CollisionGrid
      {
         public int NumXCells;
         public int NumYCells;
         public int NumTotalCells;
         public CollisionCell[,] Cells;

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
