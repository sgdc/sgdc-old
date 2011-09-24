using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Physics.Collision
{
    /// <summary>
    /// The central class to compare collisions and determine if they actually collide or not.
    /// </summary>
    public partial class CollisionChief
    {
        private static CollisionChief mInstance;

        private Vector2 mWorldSize;
        private Vector2 mCellSize;
        private CollisionGrid mCollisionGrid;
        private List<CollisionUnit> mUnitsToUpdate;
        private List<Entity> mEntitiesToNotify;
        private int mCurrTimeStamp;

        private CollisionChief() { }

        /// <summary>
        /// Update the CollisionChief.
        /// </summary>
        public void Update()
        {
            List<Entity> entities = null;
            if (Game.CurrentGame.gameContent != null)
            {
                entities = Game.CurrentGame.gameContent.UpdateEntities;
            }

            mCurrTimeStamp++;
            UpdateCollisions(entities);
            NotifyEntities(entities);
        }

        private void UpdateCollisions(List<Entity> entities)
        {
            if (mUnitsToUpdate == null)
            {
                return;
            }
            Vector2 circleCenter;
            int circleRadius;
            Point topLeftCell;
            Point bottomRightCell;

            foreach (CollisionUnit unit in mUnitsToUpdate)
            {
                unit.ClearCollisions();
                unit.ClearCheckedUnits();
                unit.CollisionsChanged(false);

                if (entities != null && !entities.Contains(unit.GetOwner()))
                {
                    continue;
                }

                if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE || unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_BOX)
                {
                    if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE)
                    {
                        circleCenter = unit.GetCircleCenter();
                        circleRadius = unit.GetCircleRadius();
                        topLeftCell = CalculateCircleTopLeftCell(circleCenter, circleRadius);
                        bottomRightCell = CalculateCircleBottomRightCell(circleCenter, circleRadius);
                    }
                    else
                    {
                        topLeftCell = CalculateCellPosition(unit.GetUpperLeft());
                        bottomRightCell = CalculateCellPosition(unit.GetLowerRight());
                    }

                    // for all touching cells
                    for (int i = topLeftCell.X; i <= bottomRightCell.X; i++)
                    {
                        for (int j = topLeftCell.Y; j <= bottomRightCell.Y; j++)
                        {
                            foreach (CollisionUnit other in mCollisionGrid.Cells[i, j].collisionMembers)
                            {
                                if (other != unit && !(other.GetLastCheckedBy() == unit && other.GetCheckTimeStamp() == mCurrTimeStamp) && 
                                    (Graphics.SpriteManager.gfx.OrderSeperation == 0 || (unit.GetOwner().SpriteImage.DrawOrder == other.GetOwner().SpriteImage.DrawOrder)))// && other.GetFullCheckTimeStamp() < mCurrTimeStamp)
                                {
                                    other.CollisionsChanged(false);

                                    unit.UpdateCollisionsWith(other);
                                    other.SetLastCheckedBy(unit);
                                    other.SetCheckTimeStamp(mCurrTimeStamp);

                                    if (other.CollisionsChanged(mCurrTimeStamp))
                                    {
                                        mEntitiesToNotify.Add(other.GetOwner());
                                    }
                                }
                            }
                        }
                    }
                }
                else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_LINE)
                {
                    // TODO
                }

                unit.NeedsCollisionUpdate(false);
                unit.SetFullCheckTimeStamp(mCurrTimeStamp);

                if (unit.CollisionsChanged(mCurrTimeStamp))
                {
                    mEntitiesToNotify.Add(unit.GetOwner());
                }
            }

            mUnitsToUpdate.Clear();
        }

        // notify entities of collision change
        private void NotifyEntities(List<Entity> entities)
        {
            if (mEntitiesToNotify != null)
            {
                foreach (Entity ent in mEntitiesToNotify)
                {
                    if (entities != null && !entities.Contains(ent))
                    {
                        continue;
                    }

                    ent.CollisionChange();
                }

                mEntitiesToNotify.Clear();
            }
        }

        /// <summary>
        /// Add a specific CollisionUnit to a queue of units to update.
        /// </summary>
        /// <param name="unit">The CollisionUnit to get updated.</param>
        public void UpdateUnit(CollisionUnit unit)
        {
            mUnitsToUpdate.Add(unit);
        }

        /// <summary>
        /// Add a new CollisionUnit to the CollisionChief.
        /// </summary>
        /// <param name="unit">The CollisionUnit to add to the CollisionChief.</param>
        public void AddCollisionUnit(CollisionUnit unit)
        {
            Vector2 circleCenter;
            int circleRadius;
            Vector2 lineStart;
            Vector2 lineEnd;
            Point topLeftCell;
            Point bottomRightCell;

            if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE)
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
            else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_LINE)
            {
                lineStart = unit.GetLineStart();
                lineEnd = unit.GetLineEnd();

                // TODO: put in appropriate cells

            }
            else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_BOX)
            {
                topLeftCell = CalculateCellPosition(unit.GetUpperLeft());
                bottomRightCell = CalculateCellPosition(unit.GetLowerRight());

                for (int i = topLeftCell.X; i <= bottomRightCell.X; i++)
                {
                    for (int j = topLeftCell.Y; j <= bottomRightCell.Y; j++)
                    {
                        mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                    }
                }
            }
        }

        /// <summary>
        /// Remove a CollisionUnit from the CollisionChief.
        /// </summary>
        /// <param name="unit">The CollisionUnit to remove.</param>
        public void RemoveCollisionUnit(CollisionUnit unit)
        {
            Vector2 circleCenter;
            int circleRadius;
            Vector2 lineStart;
            Vector2 lineEnd;
            Point topLeftCell;
            Point bottomRightCell;

            if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE)
            {
                circleCenter = unit.GetCircleCenter();
                circleRadius = unit.GetCircleRadius();

                // remove from appropriate cells
                topLeftCell = CalculateCircleTopLeftCell(circleCenter, circleRadius);
                bottomRightCell = CalculateCircleBottomRightCell(circleCenter, circleRadius);

                for (int i = topLeftCell.X; i <= bottomRightCell.X; i++)
                {
                    for (int j = topLeftCell.Y; j <= bottomRightCell.Y; j++)
                    {
                        mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                    }
                }

            }
            else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_LINE)
            {
                lineStart = unit.GetLineStart();
                lineEnd = unit.GetLineEnd();

                // TODO: remove from appropriate cells

            }
            else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_BOX)
            {
                topLeftCell = CalculateCellPosition(unit.GetUpperLeft());
                bottomRightCell = CalculateCellPosition(unit.GetLowerRight());

                for (int i = topLeftCell.X; i <= bottomRightCell.X; i++)
                {
                    for (int j = topLeftCell.Y; j <= bottomRightCell.Y; j++)
                    {
                        mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                    }
                }
            }
        }

        /// <summary>
        /// Translate a CollisionUnit by the given amount.
        /// </summary>
        /// <param name="unit">The CollisionUnit to translate.</param>
        /// <param name="translation">The delta translation to perform on the CollisionUnit.</param>
        public void TranslateCollisionUnit(CollisionUnit unit, Vector2 translation)
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

            if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE || unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_BOX)
            {
                if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE)
                {
                    circleRadius = unit.GetCircleRadius();
                    oldCircleCenter = unit.GetCircleCenter();
                    newCircleCenter = oldCircleCenter + translation;

                    // calculate containing cells
                    oldTopLeftCell = CalculateCircleTopLeftCell(oldCircleCenter, circleRadius);
                    oldBottomRightCell = CalculateCircleBottomRightCell(oldCircleCenter, circleRadius);

                    newTopLeftCell = CalculateCircleTopLeftCell(newCircleCenter, circleRadius);
                    newBottomRightCell = CalculateCircleBottomRightCell(newCircleCenter, circleRadius);
                }
                else
                {
                    oldTopLeftCell = CalculateCellPosition(unit.GetUpperLeft());
                    oldBottomRightCell = CalculateCellPosition(unit.GetLowerRight());

                    newTopLeftCell = CalculateCellPosition(unit.GetUpperLeft() + translation);
                    newBottomRightCell = CalculateCellPosition(unit.GetLowerRight() + translation);
                }

                // remove from cells no longer within and add to new cells that unit is within
                if (translation.X > 0)
                {
                    // remove
                    for (int i = oldTopLeftCell.X; i < newTopLeftCell.X && i <= oldBottomRightCell.X; i++)
                    {
                        for (int j = oldTopLeftCell.Y; j <= oldBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    int minStart = oldBottomRightCell.X + 1;
                    if (minStart < newTopLeftCell.X)
                    {
                        minStart = newTopLeftCell.X;
                    }

                    for (int i = minStart; i <= newBottomRightCell.X; i++)
                    {
                        for (int j = newTopLeftCell.Y; j <= newBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                        }
                    }
                }
                else if (translation.X < 0)
                {
                    // remove
                    int minStart = newBottomRightCell.X + 1;
                    if (minStart < oldTopLeftCell.X)
                    {
                        minStart = oldTopLeftCell.X;
                    }

                    for (int i = minStart; i <= oldBottomRightCell.X; i++)
                    {
                        for (int j = oldTopLeftCell.Y; j <= oldBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    for (int i = newTopLeftCell.X; i < oldTopLeftCell.X && i <= newBottomRightCell.X; i++)
                    {
                        for (int j = newTopLeftCell.Y; j <= newBottomRightCell.Y; j++)
                        {
                            mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                        }
                    }
                }

                if (translation.Y > 0)
                {
                    // remove
                    for (int i = oldTopLeftCell.Y; i < newTopLeftCell.Y && i <= oldBottomRightCell.Y; i++)
                    {
                        for (int j = oldTopLeftCell.X; j <= oldBottomRightCell.X; j++)
                        {
                            mCollisionGrid.Cells[j, i].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    int minStart = oldBottomRightCell.Y + 1;
                    if (minStart < newTopLeftCell.Y)
                    {
                        minStart = newTopLeftCell.Y;
                    }

                    for (int i = minStart; i <= newBottomRightCell.Y; i++)
                    {
                        // avoid double adds
                        if (translation.X > 0)
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
                else if (translation.Y < 0)
                {
                    // remove
                    int minStart = newBottomRightCell.Y + 1;
                    if (minStart < oldTopLeftCell.Y)
                    {
                        minStart = oldTopLeftCell.Y;
                    }

                    for (int i = minStart; i <= oldBottomRightCell.Y; i++)
                    {
                        for (int j = oldTopLeftCell.X; j <= oldBottomRightCell.X; j++)
                        {
                            mCollisionGrid.Cells[j, i].RemoveCollisionUnit(unit);
                        }
                    }

                    // add
                    for (int i = newTopLeftCell.Y; i < oldTopLeftCell.Y && i <= newBottomRightCell.Y; i++)
                    {
                        // avoid double adds
                        if (translation.X > 0)
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
            else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_LINE)
            {
                oldLineStart = unit.GetLineStart();
                oldLineEnd = unit.GetLineEnd();

                // TODO: put in appropriate cells

            }
        }

        /// <summary>
        /// Rotate a CollisionUnit by the given amount.
        /// </summary>
        /// <param name="unit">The CollisionUnit to rotate.</param>
        /// <param name="rotation">The delta rotation to perform on the CollisionUnit (in radians).</param>
        public void RotateCollisionUnit(CollisionUnit unit, float rotation)
        {
            // TODO
        }

        /// <summary>
        /// Scale a CollisionUnit by the given amount.
        /// </summary>
        /// <param name="unit">The CollisionUnit to scale.</param>
        /// <param name="scale">The delta scale to perform on the CollisionUnit.</param>
        public void ScaleCollisionUnit(CollisionUnit unit, Vector2 scale)
        {
            int oldCircleRadius;
            int newCircleRadius;
            Vector2 circleCenter;
            Vector2 oldLineStart;
            Vector2 oldLineEnd;
            Point oldTopLeftCell;
            Point oldBottomRightCell;
            Point newTopLeftCell;
            Point newBottomRightCell;
            bool scaleFromCenter = false;

            if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE || unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_BOX)
            {
                if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE)
                {
                    oldCircleRadius = unit.GetCircleRadius();
                    //newCircleRadius = (int)(unit.GetCircleRadius() * ((scale.X + scale.Y) / 2));
                    //newCircleRadius = (int)(unit.GetCircleRadius() + unit.GetCircleRadius() * ((scale.X + scale.Y) / 2));
                    newCircleRadius = (int)(unit.GetCircleRadius() + ((scale.X + scale.Y) / 2));
                    circleCenter = unit.GetCircleCenter();

                    // calculate containing cells
                    oldTopLeftCell = CalculateCircleTopLeftCell(circleCenter, oldCircleRadius);
                    oldBottomRightCell = CalculateCircleBottomRightCell(circleCenter, oldCircleRadius);

                    newTopLeftCell = CalculateCircleTopLeftCell(circleCenter, newCircleRadius);
                    newBottomRightCell = CalculateCircleBottomRightCell(circleCenter, newCircleRadius);
                }
                else
                {
                    Vector2 point1 = unit.GetUpperLeft();
                    Vector2 point2 = unit.GetLowerRight();
                    int oldWidth = unit.GetWidth();
                    int oldHeight = unit.GetHeight();
                    //int xChange = (int)(((oldWidth * scale.X) - oldWidth) / 2);
                    //int yChange = (int)(((oldHeight * scale.Y) - oldHeight) / 2);
                    //int xChange = (int)(oldWidth * scale.X / 2);
                    //int yChange = (int)(oldHeight * scale.Y / 2);
                    int xChange = (int)scale.X;
                    int yChange = (int)scale.Y;

                    // scale from center
                    if (scaleFromCenter)
                    {
                        point1.X -= xChange;
                        point1.Y -= yChange;

                        point2.X += xChange;
                        point2.Y += yChange;
                    }
                    else
                    {
                        // scale with top left stationary
                        point2.X += xChange * 2;
                        point2.Y += yChange * 2;
                    }

                    oldTopLeftCell = CalculateCellPosition(unit.GetUpperLeft());
                    oldBottomRightCell = CalculateCellPosition(unit.GetLowerRight());

                    newTopLeftCell = CalculateCellPosition(point1);
                    newBottomRightCell = CalculateCellPosition(point2);
                }

                //if (scale.X < 1)
                if (scale.X < 0)
                {
                    for (int j = oldTopLeftCell.Y; j <= oldBottomRightCell.Y; j++)
                    {
                        // remove left
                        for (int i = oldTopLeftCell.X; i < newTopLeftCell.X; i++)
                        {
                            mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                        }

                        // remove right
                        for (int i = newBottomRightCell.X + 1; i <= oldBottomRightCell.X; i++)
                        {
                            mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                        }
                    }

                    //if (scale.Y < 1)
                    if (scale.Y < 0)
                    {
                        for (int i = newTopLeftCell.X; i <= newBottomRightCell.X; i++)
                        {
                            // remove top
                            for (int j = oldTopLeftCell.Y; j < newTopLeftCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                            }

                            // remove bottom
                            for (int j = newBottomRightCell.Y + 1; j <= oldBottomRightCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                            }
                        }
                    }
                    //else if (scale.Y > 1)
                    else if (scale.Y > 0)
                    {
                        for (int i = newTopLeftCell.X; i <= newBottomRightCell.X; i++)
                        {
                            // add top
                            for (int j = newTopLeftCell.Y; j < oldTopLeftCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                            }

                            // add bottom
                            for (int j = oldBottomRightCell.Y + 1; j <= newBottomRightCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                            }
                        }
                    }
                }
                //else if (scale.X > 1)
                else if (scale.X > 0)
                {
                    for (int j = newTopLeftCell.Y; j <= newBottomRightCell.Y; j++)
                    {
                        // add left
                        for (int i = newTopLeftCell.X; i < oldTopLeftCell.X; i++)
                        {
                            mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                        }

                        // add right
                        for (int i = oldBottomRightCell.X + 1; i <= newBottomRightCell.X; i++)
                        {
                            mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                        }
                    }

                    //if (scale.Y < 1)
                    if (scale.Y < 0)
                    {
                        for (int i = oldTopLeftCell.X; i <= oldBottomRightCell.X; i++)
                        {
                            // remove top
                            for (int j = oldTopLeftCell.Y; j < newTopLeftCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                            }

                            // remove bottom
                            for (int j = newBottomRightCell.Y + 1; j <= oldBottomRightCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].RemoveCollisionUnit(unit);
                            }
                        }
                    }
                    //else if (scale.Y > 1)
                    else if (scale.Y > 0)
                    {
                        for (int i = oldTopLeftCell.X; i <= oldBottomRightCell.X; i++)
                        {
                            // add top
                            for (int j = newTopLeftCell.Y; j < oldTopLeftCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                            }

                            // add bottom
                            for (int j = oldBottomRightCell.Y + 1; j <= newBottomRightCell.Y; j++)
                            {
                                mCollisionGrid.Cells[i, j].AddCollisionUnit(unit);
                            }
                        }
                    }
                }
            }
            else if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_LINE)
            {
                oldLineStart = unit.GetLineStart();
                oldLineEnd = unit.GetLineEnd();

                // TODO: put in appropriate cells

            }
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

        private Point CalculateCellPosition(Vector2 position)
        {
            Point cellPosition = new Point();

            cellPosition.X = (int)(position.X / mCellSize.X);
            cellPosition.Y = (int)(position.Y / mCellSize.Y);

            if (cellPosition.X < 0)
            {
                cellPosition.X = 0;
            }

            if (cellPosition.Y < 0)
            {
                cellPosition.Y = 0;
            }

            if (cellPosition.X >= mCollisionGrid.NumXCells)
            {
                cellPosition.X = mCollisionGrid.NumXCells - 1;
            }

            if (cellPosition.Y >= mCollisionGrid.NumYCells)
            {
                cellPosition.Y = mCollisionGrid.NumYCells - 1;
            }

            return cellPosition;
        }

        /// <summary>
        /// Helper function for debugging. Draws the collision units in the location of a collision cell on the collision grid.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used to draw the grid.</param>
        /// <param name="gridTexture">A texture representing one cell in the collision grid.</param>
        public void DrawCollisionGrid(SpriteBatch spriteBatch, Texture2D gridTexture)
        {
            Vector2 cellPosition = new Vector2();
            int count;
            Rectangle rect;

            for (int i = 0; i < mCollisionGrid.NumXCells; i++)
            {
                for (int j = 0; j < mCollisionGrid.NumYCells; j++)
                {
                    count = mCollisionGrid.Cells[i, j].collisionMembers.Count;
                    if (count > 0)
                    {
                        cellPosition.X = i * mCellSize.X;
                        cellPosition.Y = j * mCellSize.Y;

                        if (count > 1)
                        {
                            //spriteBatch.Draw(gridTexture, cellPosition, Color.Orange);
                            rect = new Rectangle((int)cellPosition.X, (int)cellPosition.Y, (int)mCellSize.X, (int)mCellSize.Y);
                            spriteBatch.Draw(gridTexture, rect, Color.Orange);
                        }
                        else
                        {
                            rect = new Rectangle((int)cellPosition.X, (int)cellPosition.Y, (int)mCellSize.X, (int)mCellSize.Y);
                            spriteBatch.Draw(gridTexture, rect, Color.White);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initialize the CollisionChief.
        /// </summary>
        /// <param name="worldSize">The size, in pixels, of the map. This defines the size of the collision grid.</param>
        /// <param name="cellSize">The size, in pixels, of a single cell in the collision grid.</param>
        public void Initialize(Vector2 worldSize, Vector2 cellSize)
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
            mEntitiesToNotify = new List<Entity>();
            mCurrTimeStamp = 0;
        }

        /// <summary>
        /// Get the CollisionChief singleton.
        /// </summary>
        /// <returns>The CollisionChief singleton.</returns>
        public static CollisionChief GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = new CollisionChief();
            }
            return mInstance;
        }
    }
}
