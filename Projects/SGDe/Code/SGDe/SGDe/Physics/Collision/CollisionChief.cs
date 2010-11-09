using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Physics.Collision
{
    public class CollisionChief
    {
        private Vector2 mWorldSize;
        private Vector2 mCellSize;
        private CollisionGrid mCollisionGrid;
        private List<CollisionUnit> mUnitsToUpdate;
        private int mCurrTimeStamp;

        protected class CollisionCell
        {
            public List<CollisionUnit> collisionMembers;

            public CollisionCell()
            {
                collisionMembers = new List<CollisionUnit>();
            }

            public void AddCollisionUnit(CollisionUnit unit)
            {
                collisionMembers.Add(unit);
            }

            public void RemoveCollisionUnit(CollisionUnit unit)
            {
                collisionMembers.Remove(unit);
            }
        }

        protected class CollisionGrid
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

        public CollisionChief(Vector2 worldSize, Vector2 cellSize)
        {
            int numXCells;
            int numYCells;

            mWorldSize = worldSize;
            mCellSize = cellSize;

            numXCells = (int)(mWorldSize.X / mCellSize.X);
            if (mWorldSize.X % mCellSize.X > 0)
            {
                numXCells++;
            }

            numYCells = (int)(mWorldSize.Y / mCellSize.Y);
            if (mWorldSize.Y % mCellSize.Y > 0)
            {
                numYCells++;
            }

            mCollisionGrid = new CollisionGrid(numXCells, numYCells);
            mUnitsToUpdate = new List<CollisionUnit>();
            mCurrTimeStamp = 0;
        }

        public void Update()
        {
            mCurrTimeStamp++;
            UpdateCollisions();
        }

        private void UpdateCollisions()
        {
            Vector2 circleCenter;
            int circleRadius;
            Point topLeftCell;
            Point bottomRightCell;

            foreach (CollisionUnit unit in mUnitsToUpdate)
            {
                if (unit.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION)
                {
                    circleCenter = unit.GetCircleCenter();
                    circleRadius = unit.GetCircleRadius();
                    topLeftCell = CalculateCircleTopLeftCell(circleCenter, circleRadius);
                    bottomRightCell = CalculateCircleBottomRightCell(circleCenter, circleRadius);

                    unit.ClearCollisions();
                    unit.ClearCheckedUnits();

                    // for all touching cells
                    for (int i = topLeftCell.X; i <= bottomRightCell.X; i++)
                    {
                        for (int j = topLeftCell.Y; j <= bottomRightCell.Y; j++)
                        {
                            foreach (CollisionUnit other in mCollisionGrid.Cells[i, j].collisionMembers)
                            {
                                if (other != unit && !(other.GetLastCheckedBy() == unit && other.GetCheckTimeStamp() == mCurrTimeStamp))
                                {
                                    unit.UpdateCollisionsWith(other);
                                    other.SetLastCheckedBy(unit);
                                    other.SetCheckTimeStamp(mCurrTimeStamp);
                                }
                            }
                        }
                    }
                }
                else if (unit.GetCollisionType() == CollisionUnit.LINE_COLLISION)
                {
                    // TODO
                }

                unit.NeedsCollisionUpdate(false);
            }

            mUnitsToUpdate.Clear();
        }

        public void UpdateUnit(CollisionUnit unit)
        {
            mUnitsToUpdate.Add(unit);
        }

        public void AddCollisionUnit(CollisionUnit unit)
        {
            Vector2 circleCenter;
            int circleRadius;
            Vector2 lineStart;
            Vector2 lineEnd;
            Point topLeftCell;
            Point bottomRightCell;


            if (unit.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION)
            {
                circleCenter = unit.GetCircleCenter();
                circleRadius = unit.GetCircleRadius();

                // put in appropriate cells
                topLeftCell = CalculateCircleTopLeftCell(circleCenter, circleRadius);
                bottomRightCell = CalculateCircleBottomRightCell(circleCenter, circleRadius);

                for (int i = topLeftCell.X; i <= bottomRightCell.X; i++)
                {
                    for (int j = topLeftCell.Y; j <= bottomRightCell.Y; j++)
                    {
                        mCollisionGrid.Cells[i,j].AddCollisionUnit(unit);
                    }
                }

            }
            else if (unit.GetCollisionType() == CollisionUnit.LINE_COLLISION)
            {
                lineStart = unit.GetLineStart();
                lineEnd = unit.GetLineEnd();

                // TODO: put in appropriate cells

            }
        }

        public void TranslateCollisionUnit(CollisionUnit unit, float x, float y)
        {
            int circleRadius;
            Vector2 oldCircleCenter;
            Vector2 oldLineStart;
            Vector2 oldLineEnd;
            Point oldTopLeftCell;
            Point oldBottomRightCell;
            Vector2 newCircleCenter;
            Point newTopLeftCell;
            Point newBottomRightCell;

            if (unit.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION)
            {
                circleRadius = unit.GetCircleRadius();
                oldCircleCenter = unit.GetCircleCenter();
                newCircleCenter = new Vector2(oldCircleCenter.X + x, oldCircleCenter.Y + y);

                // calculate containing cells
                oldTopLeftCell = CalculateCircleTopLeftCell(oldCircleCenter, circleRadius);
                oldBottomRightCell = CalculateCircleBottomRightCell(oldCircleCenter, circleRadius);

                newTopLeftCell = CalculateCircleTopLeftCell(newCircleCenter, circleRadius);
                newBottomRightCell = CalculateCircleBottomRightCell(newCircleCenter, circleRadius);

                // remove from cells no longer within and add to new cells that unit is within
                if (x > 0)
                {
                    // remove
                    for (int i = oldTopLeftCell.X; i < newTopLeftCell.X; i++)
                    {
                        for (int j = oldTopLeftCell.Y; j < oldBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    for (int i = oldBottomRightCell.X + 1; i <= newBottomRightCell.X; i++)
                    {
                        for (int j = newTopLeftCell.Y; j <= newBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                        }
                    }
                }
                else if (x < 0)
                {
                    // remove
                    for (int i = newBottomRightCell.X; i < oldBottomRightCell.X; i++)
                    {
                        for (int j = oldTopLeftCell.Y; j < oldBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    for (int i = newTopLeftCell.X; i < oldTopLeftCell.X; i++)
                    {
                        for (int j = newTopLeftCell.Y; j <= newBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                        }
                    }
                }

                if (y > 0)
                {
                    // remove
                    for (int i = oldTopLeftCell.Y; i < newTopLeftCell.Y; i++)
                    {
                        for (int j = oldTopLeftCell.X; j < oldBottomRightCell.X; j++)
                        {
                            mCollisionGrid.Cells[j, i].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    for (int i = oldBottomRightCell.Y + 1; i <= newBottomRightCell.Y; i++)
                    {
                        // avoid double adds
                        if (x > 0)
                        {
                            for (int j = newTopLeftCell.X; j <= oldBottomRightCell.X; j++)
                            {
                                mCollisionGrid.Cells[j, i].AddCollisionUnit(unit);
                            }
                        }
                        else
                        {
                            for (int j = oldTopLeftCell.X; j <= newBottomRightCell.X; j++)
                            {
                                mCollisionGrid.Cells[j, i].AddCollisionUnit(unit);
                            }
                        }
                    }
                }
                else if (y < 0)
                {
                    // remove
                    for (int i = newBottomRightCell.Y; i < oldBottomRightCell.Y; i++)
                    {
                        for (int j = oldTopLeftCell.X; j < oldBottomRightCell.X; j++)
                        {
                            mCollisionGrid.Cells[j, i].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    for (int i = newTopLeftCell.Y; i < oldTopLeftCell.Y; i++)
                    {
                        // avoid double adds
                        if (x > 0)
                        {
                            for (int j = newTopLeftCell.X; j <= oldBottomRightCell.X; j++)
                            {
                                mCollisionGrid.Cells[j, i].AddCollisionUnit(unit);
                            }
                        }
                        else
                        {
                            for (int j = oldTopLeftCell.X; j <= newBottomRightCell.X; j++)
                            {
                                mCollisionGrid.Cells[j, i].AddCollisionUnit(unit);
                            }
                        }
                    }
                }

            }
            else if (unit.GetCollisionType() == CollisionUnit.LINE_COLLISION)
            {
                oldLineStart = unit.GetLineStart();
                oldLineEnd = unit.GetLineEnd();

                // TODO: put in appropriate cells

            }
        }

        public void RotateCollisionUnit(CollisionUnit unit, ushort rotation)
        {
            // TODO
        }

        public void ScaleCollisionUnit(CollisionUnit unit, Vector2 scale)
        {
            // TODO
        }

        private Point CalculateCircleTopLeftCell(Vector2 circleCenter, int circleRadius)
        {
            Point topLeftCell = new Point();

            // calculate containing cells
            topLeftCell.X = (int)circleCenter.X - circleRadius;
            topLeftCell.X = topLeftCell.X / (int)mCellSize.X;
            if (topLeftCell.X < 0)
            {
                topLeftCell.X = 0;
            }

            topLeftCell.Y = (int)circleCenter.Y - circleRadius;
            topLeftCell.Y = topLeftCell.Y / (int)mCellSize.Y;
            if (topLeftCell.Y < 0)
            {
                topLeftCell.Y = 0;
            }

            return topLeftCell;
        }

        private Point CalculateCircleBottomRightCell(Vector2 circleCenter, int circleRadius)
        {
            Point bottomRightCell = new Point();

            bottomRightCell.X = (int)circleCenter.X + circleRadius;
            bottomRightCell.X = bottomRightCell.X / (int)mCellSize.X;
            if (bottomRightCell.X >= mCollisionGrid.NumXCells)
            {
                bottomRightCell.X = mCollisionGrid.NumXCells - 1;
            }

            bottomRightCell.Y = (int)circleCenter.Y + circleRadius;
            bottomRightCell.Y = bottomRightCell.Y / (int)mCellSize.Y;
            if (bottomRightCell.Y >= mCollisionGrid.NumYCells)
            {
                bottomRightCell.Y = mCollisionGrid.NumYCells - 1;
            }

            return bottomRightCell;
        }
    }
}
