using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 4f;
    private float rotateSpeed = 10f;

    [SerializeField]
    private int maxMoveDistance = 4;

    [SerializeField]
    private Animator unitAnimator;

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        // Avoid the unit move back to (0, 0, 0)
        targetPosition = transform.position;
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
    }

    // public void Move(Vector3 targetPosition)
    // {
    //     this.targetPosition = targetPosition;
    // }

    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPostionList();

        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPostionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPostion;

                // Check if the grid position is valid
                if (!LevelGrid.Instance.IsValidPosition(testGridPosition))
                {
                    continue;
                }

                // Ignore the position that the current unit is inside
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }

                // Grid Position already has other unit on it
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
                Debug.Log(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
