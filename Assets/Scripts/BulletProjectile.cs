using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;

    [SerializeField]
    private TrailRenderer trailRenderer;

    [SerializeField]
    private Transform bulletHitVFX;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 100f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        // If the bullet fly through target then destroy it
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            // Detach trail from parent so it can vanish itself
            trailRenderer.transform.parent = null;
            Destroy(gameObject);

            // Create targetPosition
            Instantiate(bulletHitVFX, targetPosition, Quaternion.identity);
        }
    }
}
