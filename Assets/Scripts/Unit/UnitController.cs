using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Path;
using System.IO;
using Unity.VisualScripting;

public class UnitController : MonoBehaviour
{
    private GameObject unitMarker;
    #region inspector
    public UnitBaseStatsData unitStats;
    PathFinding path;
    private List<Node> myWay;
    private Node targetNode;
    #endregion

    #region stats
    private int hp;
    private int defendence;
    private int damage;
    private int moveSpeed;
    private int attackSpeed;
    #endregion

    private Vector3 basicScale;
    private Vector3? targetPos;

    private void Awake()
    {
        unitMarker = transform.Find("Marker").gameObject;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        // Unit의 이동 제어
        if (targetPos.HasValue)
        {
            // 이동 방향에 따른 반전 효과
            // 이동 상태에 따른 애니메이션 제어를 원하면 else 구문까지 추가하여 제어
            if (transform.position.x > targetPos.Value.x)
            {
                Vector3 newScale = new Vector3(basicScale.x * -1, basicScale.y, basicScale.z);
                transform.localScale = newScale;
            }
            else
            {
                transform.localScale = basicScale;
            }
            
        }
    }

    //선택-Marker 활성화
    public void SelectUnit()
    {
        unitMarker.SetActive(true);
    }

    //선택 해제-Marker 비활성화
    public void DeselectUnit()
    {
        unitMarker.SetActive(false);
    }

    /// <summary>
    /// Unit의 Status & 관련 변수 초기화
    /// </summary>
    void Init()
    {
        hp = unitStats.Hp;
        defendence = unitStats.Defendence;
        damage = unitStats.Damage;
        moveSpeed = unitStats.MoveSpeed;
        attackSpeed = unitStats.AttackSpeed;

        basicScale = transform.localScale;

        path = GameObject.Find("===Manager===").GetComponent<PathFinding>();
        if (path == null)
        {
            Debug.Log("찾지 못함");
        }
    }

    //Unit이 이동할 수 있도록 targetPos를 업데이트
    public void MoveTo(Vector2 end)
    {
        targetPos = new Vector3(end.x, end.y, 0);


        if (path != null)
        {
            List<Node> newWay = path.PathFind(transform.position, targetPos.Value);
            if (newWay != null)
            {
                StopCoroutine("move");
                myWay = newWay;
                StartCoroutine("move");
            }
        }
        else
        {
            Debug.LogWarning("경로 탐색 실패");
            targetPos = null;
        }

    }

    IEnumerator move()
    {
        if (myWay == null || myWay.Count == 0)
        {
            yield break;
        }

        int idx = 0;
        targetNode = myWay[0];

        while (idx < myWay.Count)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            Vector2 target2D = new Vector2(targetNode.myPos.x, targetNode.myPos.y);

            if (Vector2.Distance(pos2D, target2D) < 0.05f)
            {
                idx++;
                if (idx >= myWay.Count)
                {
                    transform.position = new Vector3(targetNode.myPos.x, targetNode.myPos.y, transform.position.z);
                    targetPos = null;   // 목적지 해제
                    yield break;
                }

                targetNode = myWay[idx];
            }

            transform.position = Vector2.MoveTowards(pos2D, target2D, Time.deltaTime * moveSpeed);
            yield return null;
        }
    
    }
}
