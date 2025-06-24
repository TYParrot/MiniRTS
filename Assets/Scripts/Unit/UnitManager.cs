using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitManager : MonoBehaviour
    {
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

        // Start is called before the first frame update
        void Start()
        {
            StatsInit();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Unit의 Status를 초기화한다.
        /// </summary>
        void StatsInit()
        {
            hp = unitStats.Hp;
            defendence = unitStats.Defendence;
            damage = unitStats.Damage;
            moveSpeed = unitStats.MoveSpeed;
            attackSpeed = unitStats.AttackSpeed;
        }
    }

}