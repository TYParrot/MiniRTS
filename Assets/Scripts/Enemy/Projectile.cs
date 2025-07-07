using System.Collections;
using UnityEngine;
using Core.Common;

namespace Core.Attack
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] LayerMask targetLayer;
        private Vector3 targetPosition;
        private float attackSpeed;
        private int attackDamage;
        private CommonInterface target;

        public void Init(Vector3 targetPos, float speed, int damage, LayerMask targetLayer)
        {
            this.targetPosition = targetPos;
            this.attackSpeed = speed;
            this.attackDamage = damage;
            this.targetLayer = targetLayer;
        }

        void Update()
        {
            Vector3 dir = (targetPosition - transform.position).normalized;
            transform.position += dir * attackSpeed * Time.deltaTime;

            Debug.DrawLine(transform.position, targetPosition, Color.red);

            // 일정 거리 도달하면 Destroy
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// 충돌한 객체가 타겟으로 지정된 Layer이면 TakeDamage 메소드를 찾아 피해를 입력하기
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"[Projectile] 충돌 감지: {collision.gameObject.name} (Layer: {collision.gameObject.layer})");

            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                var hit = collision.GetComponent<CommonInterface>();
                if (hit != null)
                {
                    Debug.Log($"[Projectile] 타겟 {collision.gameObject.name}에게 {attackDamage} 데미지");
                    hit.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.Log($"[Projectile] CommonInterface 없음 - 무시");
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"[Projectile] LayerMask에 포함되지 않아 무시");
            }
        }
    }
}
