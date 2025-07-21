using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<Unit> unitList;
    private List<Unit> friendlyList;
    private List<Unit> enemyList;

    public static UnitManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        unitList = new();
        friendlyList = new();
        enemyList = new();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.GetIsEnemy())
        {
            enemyList.Remove(unit);
        }
        else
        {
            friendlyList.Remove(unit);
        }
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.GetIsEnemy())
        {
            enemyList.Add(unit);
        }
        else
        {
            friendlyList.Add(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyList;
    }
}
