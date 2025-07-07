using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBaseStatsData", menuName = "ScriptableObject/UnitBaseStatsData", order = 1)]
public class UnitBaseStatsData : ScriptableObject
{
    public int Hp;
    public int Defendence;
    public int Damage;
    public int MoveSpeed;
    public float AttackRange;
    public float AttackSpeed;

    public ResourceCost cost;

    public GameObject unitPrefab;
}

[System.Serializable]
public class ResourceCost
{
    public int clay;
    public int gravel;
}
