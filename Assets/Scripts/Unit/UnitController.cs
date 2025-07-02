using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private GameObject unitMarker;
    #region inspector
    public UnitBaseStatsData unitStats;
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos.Value, Time.deltaTime * moveSpeed);

            // 이동 방향에 따른 반전 효과
            // 이동 상태에 따른 애니메이션 제어를 원하면 else 구문까지 추가하여 제어
            if (transform.position.x > targetPos.Value.x)
            {
                Vector3 newScale = new Vector3(basicScale.x * -1, basicScale.y, basicScale.z);
                transform.localScale = newScale;
            }
            else if (transform.position.x < targetPos.Value.x)
            {
                transform.localScale = basicScale;
            }

            if (transform.position == targetPos)
            {
                targetPos = null;
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
    }

    //Unit이 이동할 수 있도록 targetPos를 업데이트
    public void MoveTo(Vector2 end)
    {
        targetPos = new Vector3(end.x, end.y, 0);
        
    }
}
