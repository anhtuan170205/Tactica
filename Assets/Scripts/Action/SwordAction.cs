using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted; 
    private enum State
    {
        SwingBeforeHit,
        SwingAfterHit,
    }
    private int swordRange = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.SwingBeforeHit:
                Aim();
                break;
            case State.SwingAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingBeforeHit:
                state = State.SwingAfterHit;
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                Slash(targetUnit);
                break;
            case State.SwingAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    private void Aim()
    {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection);
        float rotateSpeed = 10f;
        unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, aimRotation, rotateSpeed * Time.deltaTime);
    }

    private void Slash(Unit targetUnit)
    {
        targetUnit.Damage(100);
        OnAnySwordHit?.Invoke(this, EventArgs.Empty);
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override void ExecuteAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -swordRange; x <= swordRange; x++)
        {
            for (int z = -swordRange; z <= swordRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!LevelGrid.Instance.HasUnitAtGridPosition(testGridPosition))
                {
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemyUnit() == unit.IsEnemyUnit())
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
            actionValue = 200
        };
    }

    public int GetSwordRange()
    {
        return swordRange;
    }
}
