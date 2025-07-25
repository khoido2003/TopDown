using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform bulletProjectilePrefab;

    [SerializeField]
    private Transform shootPointTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        Transform bulletProjectileTransform = Instantiate(
            bulletProjectilePrefab,
            shootPointTransform.position,
            Quaternion.identity
        );

        BulletProjectile bulletProjectile =
            bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootPosition = e.targetUnit.GetWorldPosition();

        // Make the bullet fly horizontally
        targetUnitShootPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootPosition);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", true);
    }
}
