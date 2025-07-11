using System.Collections;
using System.Collections.Generic;
using Core.Common;
using UnityEngine;
using System;
using Core.Attack;
using Core.Effect;

public class BaseController : MonoBehaviour, CommonInterface
{

    #region stats
    private int hp;
    private int defendence;
    private int damage;
    private float attackSpeed;
    private float attackRange;
    [SerializeField] public UnitBaseStatsData baseStats;    
    [SerializeField] LayerMask unitLayer;
    [SerializeField] private int type;
    [SerializeField] private GameObject projectilePrefab;
    #endregion

    #region unit
    private GameObject targetUnit;
    #endregion

    //기지가 파괴되면 게임은 종료됨.
    //GameUIController에서 구독하도록 할것!
    public static event Action OnEndGame;

    private enum BaseState { Idle, Attacking, Destry }
    private BaseState currentState;
    private float attackTimer;

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (currentState)
        {
            case BaseState.Idle:
                LookForUnit();
                break;
            case BaseState.Attacking:
                AttackUnit();
                break;
            case BaseState.Destry:
                break;
        }
    }

    void Init()
    {
        hp = baseStats.Hp;
        defendence = baseStats.Defendence;
        damage = baseStats.Damage;
        attackSpeed = baseStats.AttackSpeed;
        attackRange = baseStats.AttackRange;        
    }

    //유닛을 탐지하고, 발견하면 공격상태로 전환
    void LookForUnit()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, attackRange, unitLayer);

        if (target != null)
        {
            targetUnit = target.gameObject;
            currentState = BaseState.Attacking;
        }
    }

    
        //타겟 유닛이 없으면 Idle로 전환, 아니면 공격
        void AttackUnit()
        {
            if (targetUnit == null)
            {
                currentState = BaseState.Idle;
                return;
            }

            attackTimer += Time.deltaTime;

            if (attackTimer >= attackSpeed)
            {
                var unit = targetUnit.GetComponent<UnitController>();

                if (unit != null)
                {
                    SpawnProjectile(unit);
                }

                attackTimer = 0;
            }
   
        }

    /// <summary>
    /// 발사체 생성 로직
    /// </summary>
    /// <param name="enemy"></param>
    public void SpawnProjectile(UnitController unit)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab이 지정되지 않았습니다!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        var projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Init(
                unit.transform.position,
                attackSpeed,
                damage,
                unitLayer
            );
        }

        if (type == 2)
        {
            projectile.transform.localScale = new Vector3(2, 2, 2);
        }

    }


    public void TakeDamage(int damage)
    {
        if (currentState == BaseState.Destry)
        {
            return;
        }

        hp = hp - (damage - defendence);

        if (hp <= 0)
        {
            currentState = BaseState.Destry;

            //Destroy 이펙트 재생
            EffectManager.Instance.PlayDestryEffect(gameObject);

            OnEndGame?.Invoke();
            Destroy(gameObject);
        }
    }
}
