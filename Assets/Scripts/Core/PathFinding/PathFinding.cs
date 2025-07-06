using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Path
{
    public class PathFinding : MonoBehaviour
    {
        Grid grid;
        private void Start()
        {
            grid = GetComponent<Grid>();
        }

        public List<Node> PathFind(Vector3 startPos, Vector3 endPos)
        {
            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();

            Node startNode = grid.GetNodeFromVector(startPos);
            Node endNode = grid.GetNodeFromVector(endPos);

            // 시작/끝 노드가 이동 불가능한 경우 처리
            if (!startNode.canWalk || !endNode.canWalk)
            {
                Debug.LogWarning("시작점 또는 도착점이 이동 불가능한 지역입니다.");
                return null;
            }

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node curNode = openList[0];

                // 최적의 노드 선택 (fCost 기준, 같으면 hCost 기준)
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < curNode.fCost ||
                        (openList[i].fCost == curNode.fCost && openList[i].hCost < curNode.hCost))
                    {
                        curNode = openList[i];
                    }
                }

                openList.Remove(curNode);
                closedList.Add(curNode);

                // 목적지 도달
                if (curNode == endNode)
                {
                    return Retrace(startNode, curNode);
                }

                foreach (Node neighborNode in grid.SearchNeighborNode(curNode))
                {
                    // 이미 처리된 노드는 건너뛰기
                    if (closedList.Contains(neighborNode))
                        continue;

                    int newCost = curNode.gCost + GetDistance(neighborNode, curNode);

                    if (newCost < neighborNode.gCost || !openList.Contains(neighborNode))
                    {
                        neighborNode.gCost = newCost;
                        neighborNode.hCost = GetDistance(neighborNode, endNode);
                        neighborNode.parent = curNode;

                        if (!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                        }
                    }
                }
            }

            return null;
        }


        List<Node> Retrace(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node curNode = end;
            while (curNode != start)
            {
                path.Add(curNode);
                curNode = curNode.parent;
            }

            path.Reverse();

            // 완성된 리스트를 역정렬해주면 출발점 - 도착점까지가 됨
            grid.path = path;
            return path;
        }

        /// <summary>
        /// 두 노드가 주어지면 거리 가중치를 계산하는 함수
        /// </summary>
        /// <param name="aNode"></param>
        /// <param name="bNode"></param>
        /// <returns>절대값으로 계산, 절대값이 더 작은 만큼은 대각선 이동 반환, 초과치는 직선 이동 반환</returns>
        int GetDistance(Node aNode, Node bNode)
        {
            int x = Mathf.Abs(aNode.myX - bNode.myX);
            int y = Mathf.Abs(aNode.myY - bNode.myY);

            if (x > y)
            {
                return 14 * y + 10 * (x - y);
            }
            return 14 * x + 10 * (y - x);
        }
    }
}
