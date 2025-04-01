using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    public static event EventHandler OnAnyGrenadeExploded;
    private Vector3 targetPosition;
    private Action onGrenadeBehaviorComplete;
    private float totalDistance;
    private Vector3 positionXZ;
    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float reachedDistance = 0.1f;
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 3f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (Vector3.Distance(positionXZ, targetPosition) < reachedDistance)
        {
            float explosionRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, explosionRadius);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviorComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0f;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
