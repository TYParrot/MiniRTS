using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Path
{
    public class Node
    {
        public bool canWalk;
        public Vector3 myPos;
        public int myX;
        public int myY;
        public int gCost;
        public int hCost;

        public Node parent;
        public Node(bool walk, Vector3 pos, int X, int Y)
        {
            canWalk = walk;
            myPos = pos;
            myX = X;
            myY = Y;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }
    }
}

