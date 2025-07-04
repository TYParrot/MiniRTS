using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Path;

public class Grid : MonoBehaviour
{
    [SerializeField] private float mapMinX;
    [SerializeField] private float mapMaxX;
    [SerializeField] private float mapMinY;
    [SerializeField] private float mapMaxY;

    public float nodeSize;
    [SerializeField] Node[,] myNode;
    int nodeCountX;
    int nodeCountY;
    //장애물인지 아닌지 구분해 줄 LayerMask
    [SerializeField] LayerMask obstacle;
    //예상 경로
    public List<Node> path;

    private void Start()
    {
        //노드가 커지면 개수가 작고, 정확도는 낮지만 계산이 빨라지고
        //노드가 작아지면 개수가 많아져서 정확도는 올라가지만 계산속도가 느려짐.
        nodeCountX = Mathf.CeilToInt((mapMaxX - mapMinX) / nodeSize);
        nodeCountY = Mathf.CeilToInt((mapMaxY - mapMinY) / nodeSize);

        myNode = new Node[nodeCountX, nodeCountY];

        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountY; j++)
            {
                //해당 노드의 좌표
                Vector3 pos = new Vector3(mapMinX + i * nodeSize, mapMinY + j * nodeSize);

                //여러 겹의 타일이 겹쳐있어도, 장애물의 여부를 확인해주는 구문
                //nodeSize/2로 검사하는 것보다 전체의 길이를 검사하는 것이 더 정확
                Collider2D hit = Physics2D.OverlapBox(pos, new Vector2(nodeSize, nodeSize), 0, obstacle);

                bool noHit = false;
                if (hit == null) noHit = true;
                myNode[i, j] = new Node(noHit, pos, i, j);
            }
        }
    }

    public List<Node> SearchNeighborNode(Node node)
    {
        List<Node> nodeList = new List<Node>();

        // 3x3 격자내에서 이동이 가능
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                // 자기 자신은 생략
                if (i == 0 && j == 0) continue;

                int newX = node.myX + i;
                int newY = node.myY + j;

                if (newX >= 0 && newY >= 0 && newX < nodeCountX && newY < nodeCountY)
                {
                    if(myNode[newX, newY].canWalk)
                    nodeList.Add(myNode[newX, newY]);
                }
                    
            }
        }

        return nodeList;
    }

    public Node GetNodeFromVector(Vector3 vector)
    {
        int posX = Mathf.RoundToInt((vector.x - mapMinX )/ nodeSize);
        int posY = Mathf.RoundToInt((vector.y - mapMinY) / nodeSize);

        posX = Mathf.Clamp(posX, 0, nodeCountX - 1);
        posY = Mathf.Clamp(posY, 0, nodeCountY - 1);

        return myNode[posX, posY];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3((mapMaxX + mapMinX) / 2, (mapMaxY + mapMinY) / 2, 1));
        if (myNode != null)
        {
            foreach (Node no in myNode)
            {
                Gizmos.color = (no.canWalk) ? Color.white : Color.red;
                Gizmos.DrawCube(no.myPos, Vector3.one * (nodeSize / 2)); ;
            }
        }
    }

}
