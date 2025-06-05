using UnityEngine;
using System;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestroyed;
    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void Damage()
    {
        Destroy(gameObject);
        OnAnyCrateDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
