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
    public int AttackSpeed;

    public ResourceCost cost;
}

[System.Serializable]
public class ResourceCost
{
    public int clay;
    public int gravel;
}
