using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    private Vector3 targetPosition;

    private void Update()
    {
        float stopDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetMouseWorldPosition());
        }
    }
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
