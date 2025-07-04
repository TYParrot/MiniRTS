using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Unit
{
    public class RTSUnitController : MonoBehaviour
    {
        private List<UnitController> selectedUnitList;  //플레이어가 클릭 or 드래그로 선택한 유닛

        void Start()
        {
            selectedUnitList = new List<UnitController>();
        }


        /// <summary>
        /// 마우스 클릭으로 단일 Unit을 선택할 때 호출
        /// </summary>
        /// <param name="newUnit"></param>
        public void ClickSelectUnit(UnitController newUnit)
        {
            DeselectAll();

            SelectUnit(newUnit);
        }

        /// <summary>
        /// Shift + 마우스 클릭으로 Unit을 추가 선택할 때 호출
        /// </summary>
        /// <param name="newUnit"></param>
        public void ShiftClickSelectUnit(UnitController newUnit)
        {
            if (selectedUnitList.Contains(newUnit))
            {
                DeslsectUnit(newUnit);
            }
            else
            {
                SelectUnit(newUnit);
            }
        }

        /// <summary>
        /// 모든 Unit의 선택을 해제할 때 호출
        /// </summary>
        public void DeselectAll()
        {
            foreach (var unit in selectedUnitList)
            {
                unit.DeselectUnit();
            }

            selectedUnitList.Clear();
        }

        /// <summary>
        /// Unit의 Marker 활성화 및 선택 유닛 리스트 업데이트
        /// </summary>
        /// <param name="newUnit"></param>
        public void SelectUnit(UnitController newUnit)
        {
            newUnit.SelectUnit();
            selectedUnitList.Add(newUnit);
        }


        /// <summary>
        /// Unit의 Marker 비활성화 및 비선택 유닛 리스트 업데이트
        /// </summary>
        /// <param name="newUnit"></param>
        private void DeslsectUnit(UnitController newUnit)
        {
            newUnit.DeselectUnit();
            selectedUnitList.Remove(newUnit);
        }

        /// <summary>
        /// Selected Unit들을 모두 목적지까지 이동
        /// </summary>
        /// <param name="end"></param>
        public void MoveTo(Vector2 end)
        {
            foreach (var unit in selectedUnitList)
            {
                unit.MoveTo(end);
            }
        }
        
        
    }
}

