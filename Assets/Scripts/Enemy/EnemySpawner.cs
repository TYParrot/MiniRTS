using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Path;
using Core.Enemy;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Core.Path.Grid grid;
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private int enemyCount = 15;

    // Grid에서 생성한 노드 중 walkable만 저장하기 위함.
    private List<Vector3> walkableNode = new List<Vector3>();

    // 생성한 적들을 리스트로 저장
    private List<EnemyController> enemyList = new List<EnemyController>();

    void Start()
    {
        Init();
    }

    /// <summary>
    /// Grid에서 생성된 Node들 중 walkable한 노드만 따로 뽑아서 저장.
    /// 무작위 노드 추출 후 해당 위치에 Enemy를 Instantiate.
    /// </summary>
    void Init()
    {
        Node[,] worldNode = grid.MyNode;

        foreach (var n in worldNode)
        {
            if (n.canWalk)
            {
                walkableNode.Add(n.myPos);
            }
        }

        // 무작위 섞기
        Shuffle();

        // 적들의 랜덤한 위치에 스폰.
        // enemyController를 리스트에 저장.
        for (int i = 0; i < enemyCount && i < walkableNode.Count; i++)
        {
            Vector3 spawnPos = walkableNode[i];

            GameObject clone;

            // enemyPrefab은 2종류
            if (i < enemyCount/3*2)
            {
                clone = Instantiate(enemyPrefab1, spawnPos, Quaternion.identity);
            }
            else
            {
                clone = Instantiate(enemyPrefab2, spawnPos, Quaternion.identity);
            }
            
            clone.transform.SetParent(enemyParent.transform, worldPositionStays: true);

            if (clone == null) continue;

            EnemyController enemy = clone.GetComponent<EnemyController>();

            if (enemy == null) continue;

            enemyList.Add(enemy);
        }
    }

    /// <summary>
    /// 랜덤한 순서로 walkableNode를 셔플 (Fisher–Yates)
    /// </summary>
    void Shuffle()
    {
        for (int i = walkableNode.Count - 1; i > 0; i--)
        {
            int k = Random.Range(0, i + 1);
            (walkableNode[i], walkableNode[k]) = (walkableNode[k], walkableNode[i]);
        }
    }
}
