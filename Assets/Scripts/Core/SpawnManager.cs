using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Unit
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject spawnPos;
        [SerializeField]
        private GameObject unitParent;

        private List<UnitController> unitList = new List<UnitController>();
        // 외부 참조용
        public IReadOnlyList<UnitController> units => unitList;


        /// <summary>
        /// GameMapUIManager에서 요구하는 unitPrefab을 생성
        /// clone을 하지 못한 경우나, unit을 찾을 수 없는 경우에는 false 반환
        /// </summary>
        /// <param name="unitPrefab"></param>
        public bool SpawnUnit(GameObject unitPrefab)
        {
            GameObject clone = Instantiate(unitPrefab, spawnPos.transform.position, Quaternion.identity);
            clone.transform.SetParent(unitParent.transform, worldPositionStays: true);

            if (clone == null) return false;

            UnitController unit = clone.GetComponent<UnitController>();
            // 사망시 처리 이벤트 등록
            unit.OnDead += HandleUnitDead;

            if (unit == null) return false;

            unitList.Add(unit);
            return true;
        }

        private void HandleUnitDead(UnitController unit)
        {
            unitList.Remove(unit);
        }
            
    }
}


