using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractAction : BaseAction
{
    private int interactRange = 1;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }
    public override string GetActionName()
    {
        return "Interact";
    }

    public override void ExecuteAction(GridPosition gridPosition, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -interactRange; x <= interactRange; x++)
        {
            for (int z = -interactRange; z <= interactRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if (door == null)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}
