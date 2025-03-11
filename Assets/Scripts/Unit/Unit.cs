using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINT_MAX = 2;
    public static event EventHandler OnAnyActionPointsChanged;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] actionArray;
    private int actionPoints = 2;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        actionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
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
            Debug.Log("Action Points of " + gameObject.name + " is now " + actionPoints);
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

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        actionPoints = ACTION_POINT_MAX;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
}
