using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;
    private void Awake()
    {
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDied += Unit_OnAnyUnitDied;
    }
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        Debug.Log("Unit spawned: " + unit.name);
        unitList.Add(unit);
        if (unit.IsEnemyUnit())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }
    private void Unit_OnAnyUnitDied(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        Debug.Log("Unit died: " + unit.name);
        unitList.Remove(unit);
        if (unit.IsEnemyUnit())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}
