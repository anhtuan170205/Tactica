using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;
    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        float stopDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, direction, rotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("isRunning", true);
        }
        else
        {
            unitAnimator.SetBool("isRunning", false);
        }
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
