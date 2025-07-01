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

    private void Awake()
    {
        unitMarker = transform.Find("Marker").gameObject;
    }

    void Start()
    {
        StatsInit();
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

    //Unit이 이동(일단은 순간이동임..)
    public void MoveTo(Vector2 end)
    {
        transform.position = new Vector3(end.x, end.y, 0);
    }
}
