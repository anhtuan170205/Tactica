using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINT_MAX = 3;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDied;
    [SerializeField] private bool isEnemyUnit;
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] actionArray;
    private int actionPoints = ACTION_POINT_MAX;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        actionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDie += HealthSystem_OnDie;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);

        }
    }
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in actionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public BaseAction[] GetActionArray()
    {
        return actionArray;
    }

    public bool TrySpendActionPointToExecuteAction(BaseAction baseAction)
    {
        if (CanSpendActionPointToExecuteAction(baseAction))
        {
            SpendActionPoint(baseAction.GetActionPointCost());
            return true;
        }
        return false;
    }

    public bool CanSpendActionPointToExecuteAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointCost();
    }

    private void SpendActionPoint(int actionPointCost)
    {
        actionPoints -= actionPointCost;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }
    public float GetActionPointsNormalized()
    {
        return (float)actionPoints / ACTION_POINT_MAX;
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemyUnit() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemyUnit() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINT_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemyUnit()
    {
        return isEnemyUnit;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.TakeDamage(damageAmount);
    }

    private void HealthSystem_OnDie(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        OnAnyUnitDied?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
