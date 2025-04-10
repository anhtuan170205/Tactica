using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootUnit;
    }
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    private int shootRange = 7;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;
    [SerializeField] private int damage = 40;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Aim();
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    canShootBullet = false;
                    Shoot();
                }
                break;
            case State.Cooloff:
                break;
        }
        
        if (stateTimer <= 0)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                if (stateTimer <= 0)
                {
                    state = State.Shooting;
                    float shootDuration = 0.1f;
                    stateTimer = shootDuration;
                }
                break;
            case State.Shooting:
                if (stateTimer <= 0)
                {
                    state = State.Cooloff;
                    float cooloffDuration = 0.5f;
                    stateTimer = cooloffDuration;
                }
                break;
            case State.Cooloff:
                if (stateTimer <= 0)
                {
                    ActionComplete();
                }
                break;
        }
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        for (int x = -shootRange; x <= shootRange; x++)
        {
            for (int z = -shootRange; z <= shootRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > shootRange)
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
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.5f;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, aimDirection, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstaclesLayerMask))
                {
                    continue;
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }
    
    public override void ExecuteAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.Aiming;
        float aimDuration = 0.5f;
        stateTimer = aimDuration;
        canShootBullet = true;
        ActionStart(onActionComplete);
    }
    private void Shoot()
    {
        targetUnit.Damage(damage);
        OnShoot?.Invoke(this, new OnShootEventArgs { targetUnit = targetUnit, shootUnit = unit });
        OnAnyShoot?.Invoke(this, new OnShootEventArgs { targetUnit = targetUnit, shootUnit = unit });
    }
    private void Aim()
    {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection);
        float rotateSpeed = 10f;
        unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, aimRotation, rotateSpeed * Time.deltaTime);
    }
    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        int actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f);
        
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = actionValue,
        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
