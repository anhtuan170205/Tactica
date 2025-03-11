using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
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
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }
}
