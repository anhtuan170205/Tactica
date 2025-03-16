using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform bulletShootPointTransform;
    private void  Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMove += MoveAction_OnStartMove;
            moveAction.OnCompleteMove += MoveAction_OnCompleteMove;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }
    private void MoveAction_OnStartMove(object sender, EventArgs e)
    {
        animator.SetBool("isRunning", true);
    }
    private void MoveAction_OnCompleteMove(object sender, EventArgs e)
    {
        animator.SetBool("isRunning", false);
    }
    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, bulletShootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = bulletShootPointTransform.position.y;
        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }
}
