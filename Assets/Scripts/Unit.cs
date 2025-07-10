using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private GridPosition gridPosition;

    private float moveSpeed = 4f;
    private float rotateSpeed = 10f;

    [SerializeField]
    private Animator unitAnimator;

    private void Awake()
    {
        // Avoid the unit move back to (0, 0, 0)
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        float stoppingDistance = .1f;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            // Rotate the character to the target with smoothing ease out
            transform.forward = Vector3.Lerp(
                transform.forward,
                moveDirection,
                Time.deltaTime * rotateSpeed
            );

            // Add Walking animation to character
            unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            // Change back to Idle state
            unitAnimator.SetBool("isWalking", false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            // Unit change grid position
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
