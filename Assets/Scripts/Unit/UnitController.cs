using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Path;
using Core.Enemy;

public class UnitController : MonoBehaviour
{
    private GameObject unitMarker;
    public event System.Action<UnitController> OnDead;

    #region enemy
    [SerializeField] LayerMask enemyLayer;
    private GameObject currentTarget;
    private float attackTimer;
    #endregion


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
    private float attackSpeed;
    private float attackRange;
    #endregion

    #region visual
    private Vector3 basicScale;
    private Vector3? targetPos;
    #endregion

    private enum UnitState
    {
        Idle, Moving, Attacking, Dead
    }
    private UnitState currentState;

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
        switch (currentState)
        {
            case UnitState.Idle:
                LookForEnemy();
                break;

            case UnitState.Moving:
                LookForEnemy();
                break;

            case UnitState.Attacking:
                AttackTarget();
                break;

            case UnitState.Dead:
                //죽음 처리
                //리스트에서 삭제
                break;
        }

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
        attackRange = unitStats.AttackRange;
        attackSpeed = unitStats.AttackSpeed;

        basicScale = transform.localScale;

        path = GameObject.Find("===Manager===").GetComponent<PathFinding>();
        if (path == null)
        {
            Debug.Log("찾지 못함");
        }

        // 대기 상태로 세팅
        currentState = UnitState.Idle;
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
                //이동 상태 시작
                currentState = UnitState.Moving;

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


    /// <summary>
    /// 주변의 적을 인식하고, 공격 상태로 전환
    /// 이동은 중지되므로, 목적지는 초기화
    /// 길목에 적군이 있다면, 처치하지 않고서는 지나갈 수 없는 상태가 됨.
    /// </summary>
    void LookForEnemy()
    {
        Collider2D targetEnemy = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);

        if (targetEnemy != null)
        {
            currentTarget = targetEnemy.gameObject;
            currentState = UnitState.Attacking;

            // 이동 중이면 이동을 중지하고, 목적지를 초기화
            targetPos = null;
            myWay = null;
            StopCoroutine("move");

        }

    }

    /// <summary>
    /// 적을 실제로 공격
    /// </summary>
    void AttackTarget()
    {
        // 적이 사망하면 Idle 상태로 전환 필요
        if (currentTarget == null)
        {
            currentState = UnitState.Idle;
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackSpeed)
        {
            var enemy = currentTarget.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Debug.Log($"적군 {currentTarget.name}을 공격중!");
            attackTimer = 0;
        }
    }

    /// <summary>
    /// 피격시 체력 감소
    /// 0보다 작거나 같으면 사망 처리
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            currentState = UnitState.Dead;

            OnDead?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
