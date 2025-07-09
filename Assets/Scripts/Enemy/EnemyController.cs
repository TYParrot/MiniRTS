using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Common;
using Core.Attack;
using System;

namespace Core.Enemy
{
    public class EnemyController : MonoBehaviour, CommonInterface
    {
        #region stats
        private int hp;
        private int defendence;
        private int damage;
        private int moveSpeed;
        private float attackSpeed;
        private float attackRange;
        #endregion
        
        #region inspector
        public UnitBaseStatsData enemyStats;
        [SerializeField] LayerMask unitLayer;
        [SerializeField] private int type;
        [SerializeField] private GameObject projectilePrefab;
        #endregion

        #region unit
        private GameObject targetUnit;
        #endregion

        private enum EnemyState { Idle, Attacking, Dead }
        private EnemyState currentState;
        private float attackTimer;
        
        //static event이므로, 클래스 전체에 공유되는 이벤트
        //구독도 단 한번이면 된다.
        public static event Action OnEnemyKilled;

        void Start() {
            Init();
        }

        void Update()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    LookForUnit();
                    break;
                case EnemyState.Attacking:
                    AttackUnit();
                    break;
                case EnemyState.Dead:
                    //죽음 처리는 자동화됨.
                    break;
            }
        }

        //관련 변수 및 상태 초기화
        void Init() {
            currentState = EnemyState.Idle;

            hp = enemyStats.Hp;
            defendence = enemyStats.Defendence;
            damage = enemyStats.Damage;
            moveSpeed = enemyStats.MoveSpeed;
            attackSpeed = enemyStats.AttackSpeed;
            attackRange = enemyStats.AttackRange;

        }


        //유닛을 탐지하고, 발견하면 공격상태로 전환
        void LookForUnit()
        {
            Collider2D target = Physics2D.OverlapCircle(transform.position, attackRange, unitLayer);

            if (target != null)
            {
                targetUnit = target.gameObject;
                currentState = EnemyState.Attacking;
            }

        }

        //타겟 유닛이 없으면 Idle로 전환, 아니면 공격
        void AttackUnit()
        {
            if (targetUnit == null)
            {
                currentState = EnemyState.Idle;
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

        // 피격시 체력 감소, 0보다 작거나 같으면 사망 처리
        // 중복 호출 방지를 위해 currentState를 플래그 처리
        public void TakeDamage(int damage)
        {
            if (currentState == EnemyState.Dead)
            {
                return;
            }
            
            hp = hp - (damage - defendence);

            if (hp <= 0)
            {
                currentState = EnemyState.Dead;

                OnEnemyKilled?.Invoke();

                Destroy(gameObject);
            }
        }

    }    
}

