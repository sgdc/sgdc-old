using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MyPolarBear.Pathfinding
{
    public class AGrid
    {
        private static AGrid mInstance;

        public ANode[,] mGrid;

        private AGrid()
        {
            mGrid = new ANode[82, 82];

            for (int i = 0; i < 82; i++)
            {
                for (int j = 0; j < 82; j++)
                {
                    mGrid[i, j] = new ANode(new Vector2(i, j));
                }
            }
        }

        public static AGrid GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = new AGrid();
            }

            return mInstance;
        }

        public void setLevel(List<LevelElement> level)
        {
            foreach (ANode node in mGrid)
            {
                node.bWalkable = true;
                node.Type = ANode.NOT_SPECIAL;
            }

            //int i;
            //int j;
            //int type;

            foreach (LevelElement ele in level)
            {
                addResource(ele);
                //if (ele.Type.Equals("Bush"))
                //{
                //    type = ANode.SEED_SOURCE;
                //}
                //else if (ele.Type.Equals("SoftGround"))
                //{
                //    type = ANode.PLANT_AREA;
                //}
                //else
                //{
                //    type = ANode.NOT_SPECIAL;
                //}

                //i = (int)((ele.CollisionRect.Left + 2048) / 50);
                //if (i > 81)
                //{
                //    i = 81;
                //}
                //if (i < 0)
                //{
                //    i = 0;
                //}

                //j = (int)((ele.CollisionRect.Top + 2048) / 50);
                //if (j > 81)
                //{
                //    j = 81;
                //}
                //if (j < 0)
                //{
                //    j = 0;
                //}

                //// top left
                //mGrid[i, j].bWalkable = false;
                //mGrid[i, j].Type = type;

                //i = (int)((ele.CollisionRect.Right + 2028) / 50);
                //if (i > 81)
                //{
                //    i = 81;
                //}
                //if (i < 0)
                //{
                //    i = 0;
                //}

                //// top right
                //mGrid[i, j].bWalkable = false;
                //mGrid[i, j].Type = type;

                //j = (int)((ele.CollisionRect.Bottom + 2048) / 50);
                //if (j > 81)
                //{
                //    j = 81;
                //}
                //if (j < 0)
                //{
                //    j = 0;
                //}

                //// bottom right
                //mGrid[i, j].bWalkable = false;
                //mGrid[i, j].Type = type;

                //i = (int)(ele.CollisionRect.Left);
                //if (i > 81)
                //{
                //    i = 81;
                //}
                //if (i < 0)
                //{
                //    i = 0;
                //}

                //// bottom left
                //mGrid[i, j].bWalkable = false;
                //mGrid[i, j].Type = type;
            }
        }

        public void addResource(LevelElement ele)
        {
            int i;
            int j;
            int type;
            bool walkable;

            if (ele.Type.Equals("Bush"))
            {
                type = ANode.SEED_SOURCE;
                walkable = false;
            }
            else if (ele.Type.Equals("SoftGround"))
            {
                type = ANode.PLANT_AREA;
                walkable = true;
            }
            else if (ele.Type.Equals("BabyPlant"))
            {
                type = ANode.NOT_SPECIAL;
                walkable = true;
            }
            else
            {
                type = ANode.NOT_SPECIAL;
                walkable = false;
            }

            i = (int)((ele.CollisionRect.Left + 2048) / 50);
            if (i > 81)
            {
                i = 81;
            }
            if (i < 0)
            {
                i = 0;
            }

            j = (int)((ele.CollisionRect.Top + 2048) / 50);
            if (j > 81)
            {
                j = 81;
            }
            if (j < 0)
            {
                j = 0;
            }

            // top left
            mGrid[i, j].bWalkable = walkable;
            mGrid[i, j].Type = type;

            i = (int)((ele.CollisionRect.Right + 2028) / 50);
            if (i > 81)
            {
                i = 81;
            }
            if (i < 0)
            {
                i = 0;
            }

            // top right
            mGrid[i, j].bWalkable = walkable;
            mGrid[i, j].Type = type;

            j = (int)((ele.CollisionRect.Bottom + 2048) / 50);
            if (j > 81)
            {
                j = 81;
            }
            if (j < 0)
            {
                j = 0;
            }

            // bottom right
            mGrid[i, j].bWalkable = walkable;
            mGrid[i, j].Type = type;

            i = (int)(ele.CollisionRect.Left);
            if (i > 81)
            {
                i = 81;
            }
            if (i < 0)
            {
                i = 0;
            }

            // bottom left
            mGrid[i, j].bWalkable = walkable;
            mGrid[i, j].Type = type;
        }

        // Perform A* to get shortest path from start to end
        public List<Vector2> getPath(Vector2 start, Vector2 end)
        {
            List<Vector2> path = new List<Vector2>();
            List<ANode> openList = new List<ANode>();
            List<ANode> closedList = new List<ANode>();
            int startX;
            int startY;
            int endX;
            int endY;
            bool bFound = false;
            int start_i;
            int start_j;
            int end_i;
            int end_j;
            int gCost;

            startX = (int)((start.X + 2048) / 50);
            startY = (int)((start.Y + 2048) / 50);
            if (startX < 0)
            {
                startX = 0;
            }
            if (startX > 81)
            {
                startX = 81;
            }
            if (startY < 0)
            {
                startY = 0;
            }
            if (startY > 81)
            {
                startY = 81;
            }
            endX = (int)((end.X + 2048) / 50);
            endY = (int)((end.Y + 2048) / 50);

            ANode currNode = mGrid[startX, startY];
            if (currNode.bWalkable == true)
            {
                openList.Add(currNode);
                currNode.bOpen = true;
            }

            while (openList.Count > 0 && !bFound)
            {
                currNode = openList[0];
                openList.RemoveAt(0);
                currNode.bOpen = false;

                closedList.Add(currNode);
                currNode.bClosed = true;

                // 8 surrounding blocks positions
                start_i = (int)currNode.Location.X;
                end_i = (int)currNode.Location.X;
                start_j = (int)currNode.Location.Y;
                end_j = (int) currNode.Location.Y;
                if (start_i > 0)
                {
                    start_i--;
                }
                if (end_i < 81)
                {
                    end_i++;
                }
                if (start_j > 0)
                {
                    start_j--;
                }
                if (end_j < 81)
                {
                    end_j++;
                }

                // consider 8 surrounding blocks for next move
                for (int i = start_i; i <= end_i && !bFound; i++)
                {
                    for (int j = start_j; j <= end_j; j++)
                    {
                        if (i == endX && j == endY)
                        {
                            bFound = true;
                            if ((int)(currNode.Location.X) != i && (int)(currNode.Location.Y) != j)
                            {
                                mGrid[i, j].Parent = currNode;
                            }
                            currNode = mGrid[i, j];
                            break;
                        }

                        if (mGrid[i, j].bWalkable && !mGrid[i, j].bClosed)
                        {
                            if (i != (int)currNode.Location.X && j != (int)currNode.Location.Y)
                            {
                                // diagonal
                                gCost = currNode.GCost + 3;
                            }
                            else
                            {
                                // horizontal or vertical
                                gCost = currNode.GCost + 2;
                            }

                            if (!mGrid[i, j].bOpen)
                            {
                                // hasn't been checked yet
                                if ((int)(currNode.Location.X) != i && (int)(currNode.Location.Y) != j)
                                {
                                    mGrid[i, j].Parent = currNode;
                                }
                                
                                mGrid[i, j].GCost = gCost;
                                mGrid[i, j].HCost = (Math.Abs(endX - i) + Math.Abs(endY - j)) * 2;
                                mGrid[i, j].FCost = mGrid[i, j].GCost + mGrid[i, j].HCost;

                                openList.Add(mGrid[i, j]);
                                mGrid[i, j].bOpen = true;
                            }
                            else
                            {
                                // already on open list
                                if (mGrid[i, j].GCost > gCost)
                                {
                                    if ((int)(currNode.Location.X) != i && (int)(currNode.Location.Y) != j)
                                    {
                                        mGrid[i, j].Parent = currNode;
                                    }

                                    mGrid[i, j].GCost = gCost;
                                    mGrid[i, j].FCost = mGrid[i, j].GCost + mGrid[i, j].HCost;
                                }
                            }
                        }
                    }
                }

                // sort open list by fcost
                openList.Sort();
            }

            // reset touched nodes
            foreach (ANode node in openList)
            {
                node.bOpen = false;
                node.Parent = null;
                node.GCost = 0;
                node.HCost = 0;
                node.FCost = 0;
            }

            foreach (ANode node in closedList)
            {
                node.bClosed = false;
                node.Parent = null;
                node.GCost = 0;
                node.HCost = 0;
                node.FCost = 0;
            }

            if (!bFound)
            {
                return null;
            }

            while (currNode != null)
            {
                if (path.Count > 1000)  // maybe cycle
                {
                    path = null;
                    break;
                }
                path.Insert(0, new Vector2(currNode.Location.X * 50 - 2048, currNode.Location.Y * 50 - 2048));
                currNode = currNode.Parent;
            }

            return path;
        }

        // Perform Dijkstra's to get shortest path from start to closest node of type nodeType
        public List<Vector2> getPath(Vector2 start, int nodeType)
        {
            List<Vector2> path = new List<Vector2>();
            List<ANode> openList = new List<ANode>();
            List<ANode> closedList = new List<ANode>();
            int startX;
            int startY;
            //int endX;
            //int endY;
            bool bFound = false;
            int start_i;
            int start_j;
            int end_i;
            int end_j;
            int gCost;

            startX = (int)((start.X + 2048) / 50);
            startY = (int)((start.Y + 2048) / 50);
            if (startX < 0)
            {
                startX = 0;
            }
            if (startX > 81)
            {
                startX = 81;
            }
            if (startY < 0)
            {
                startY = 0;
            }
            if (startY > 81)
            {
                startY = 81;
            }
            //endX = (int)((end.X + 2048) / 50);
            //endY = (int)((end.Y + 2048) / 50);

            ANode currNode = mGrid[startX, startY];
            if (currNode.bWalkable == true)
            {
                openList.Add(currNode);
                currNode.bOpen = true;
            }

            while (openList.Count > 0 && !bFound)
            {
                currNode = openList[0];
                openList.RemoveAt(0);
                currNode.bOpen = false;

                closedList.Add(currNode);
                currNode.bClosed = true;

                // 8 surrounding blocks positions
                start_i = (int)currNode.Location.X;
                end_i = (int)currNode.Location.X;
                start_j = (int)currNode.Location.Y;
                end_j = (int)currNode.Location.Y;
                if (start_i > 0)
                {
                    start_i--;
                }
                if (end_i < 81)
                {
                    end_i++;
                }
                if (start_j > 0)
                {
                    start_j--;
                }
                if (end_j < 81)
                {
                    end_j++;
                }

                // consider 8 surrounding blocks for next move
                for (int i = start_i; i <= end_i && !bFound; i++)
                {
                    for (int j = start_j; j <= end_j; j++)
                    {
                        if (mGrid[i, j].Type == nodeType)
                        {
                            bFound = true;
                            if ((int)(currNode.Location.X) != i && (int)(currNode.Location.Y) != j)
                            {
                                mGrid[i, j].Parent = currNode;
                            }

                            currNode = mGrid[i, j];
                            break;
                        }

                        if (mGrid[i, j].bWalkable && !mGrid[i, j].bClosed)
                        {
                            if (i != (int)currNode.Location.X && j != (int)currNode.Location.Y)
                            {
                                // diagonal
                                gCost = currNode.GCost + 14;
                            }
                            else
                            {
                                // horizontal or vertical
                                gCost = currNode.GCost + 10;
                            }

                            if (!mGrid[i, j].bOpen)
                            {
                                // hasn't been checked yet
                                if ((int)(currNode.Location.X) != i && (int)(currNode.Location.Y) != j)
                                {
                                    mGrid[i, j].Parent = currNode;
                                }

                                mGrid[i, j].GCost = gCost;
                                mGrid[i, j].HCost = 0;      // Dijkstra's
                                mGrid[i, j].FCost = mGrid[i, j].GCost + mGrid[i, j].HCost;

                                openList.Add(mGrid[i, j]);
                                mGrid[i, j].bOpen = true;
                            }
                            else
                            {
                                // already on open list
                                if (mGrid[i, j].GCost > gCost)
                                {
                                    if ((int)(currNode.Location.X) != i && (int)(currNode.Location.Y) != j)
                                    {
                                        mGrid[i, j].Parent = currNode;
                                    }

                                    mGrid[i, j].GCost = gCost;
                                    mGrid[i, j].FCost = mGrid[i, j].GCost + mGrid[i, j].HCost;
                                }
                            }
                        }
                    }
                }

                // sort open list by fcost
                openList.Sort();
            }

            // reset touched nodes
            foreach (ANode node in openList)
            {
                node.bOpen = false;
                node.Parent = null;
                node.GCost = 0;
                node.HCost = 0;
                node.FCost = 0;
            }

            foreach (ANode node in closedList)
            {
                node.bClosed = false;
                node.Parent = null;
                node.GCost = 0;
                node.HCost = 0;
                node.FCost = 0;
            }

            if (!bFound)
            {
                return null;
            }

            while (currNode != null)
            {
                if (path.Count > 1000) // maybe cycle
                {
                    path = null;
                    break;
                }
                path.Insert(0, new Vector2(currNode.Location.X * 50 - 2048, currNode.Location.Y * 50 - 2048));
                currNode = currNode.Parent;
            }

            return path;
        }

        //// return - 0 : equal, positive : x is greater, negative : y is greater
        //private static int CompareANodes(ANode x, ANode y)
        //{
        //    if (x == null && y == null)
        //    {
        //        return 0;
        //    }

        //    if (x == null && y != null)
        //    {
        //        return -1;
        //    }

        //    if (x != null && y == null)
        //    {
        //        return 1;
        //    }

        //    return x.FCost - y.FCost;
        //}
    }
}
