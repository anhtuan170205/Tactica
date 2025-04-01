using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeAction : BaseAction
{
    [SerializeField] private GameObject grenadeProjectilePrefab;
    private int throwRange = 5;
    private LayerMask obstaclesLayerMask;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }
    public override string GetActionName()
    {
        return "Nade";
    }
    public override void ExecuteAction(GridPosition gridPosition, Action onActionComplete)
    {
        GameObject grenadeProjectileGameObject = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileGameObject.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviorCompete);
        ActionStart(onActionComplete);
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -throwRange; x <= throwRange; x++)
        {
            for (int z = -throwRange; z <= throwRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > throwRange)
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
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
    private void OnGrenadeBehaviorCompete()
    {
        ActionComplete();
    }

}
