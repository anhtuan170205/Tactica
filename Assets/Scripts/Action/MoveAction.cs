using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMove;
    public event EventHandler OnCompleteMove; 
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private int moveRange = 5;
    private Vector3 targetPosition;

    protected override void Awake() 
    {
        base.Awake();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        Vector3 direction = (targetPosition - transform.position).normalized;
        float stopDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            OnCompleteMove?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public override void ExecuteAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        OnStartMove?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -moveRange; x <= moveRange; x++)
        {
            for (int z = -moveRange; z <= moveRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (testGridPosition == unitGridPosition)
                {
                    continue;
                }
                if (LevelGrid.Instance.HasUnitAtGridPosition(testGridPosition))
                {
                    continue;
                }
                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }
    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        int actionValue = targetCountAtGridPosition * 10;
        Debug.Log($"[MoveAction] Unit: {unit.name}, Target Count: {targetCountAtGridPosition}, Action Value: {actionValue}");

        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue =  actionValue,
        };
    }

}
