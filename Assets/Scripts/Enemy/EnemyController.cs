using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyController : MonoBehaviour
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
        #endregion

        #region unit
        private GameObject targetUnit;
        #endregion

        private enum EnemyState { Idle, Attacking, Dead }
        private EnemyState currentState;
        private float attackTimer;

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
                    //죽음 처리
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
                    unit.TakeDamage(damage);
                }

                attackTimer = 0;
            }
   
        }


        // 피격시 체력 감소, 0보다 작거나 같으면 사망 처리
        public void TakeDamage(int damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                currentState = EnemyState.Dead;
                Destroy(gameObject);
                Debug.Log("적군이 사망");
            }
        }

    }    
}

