using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
    private float moveSpeed = 4f;
    private float rotateSpeed = 10f;

    [SerializeField]
    private int maxMoveDistance = 4;

    [SerializeField]
    private Animator unitAnimator;

    public MoveAction() { }

    protected override void Awake()
    {
        base.Awake();

        // Avoid the unit move back to (0, 0, 0)
        targetPosition = transform.position;
    }

    private void Update()
    {
        // Avoid multiple action happen at the same time
        if (!isActive)
        {
            return;
        }

        float stoppingDistance = .1f;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            // Add Walking animation to character
            unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            // Change back to Idle state
            unitAnimator.SetBool("isWalking", false);
            isActive = false;

            // Calling delegate to when action stop
            onActionComplete();
        }

        // Rotate the character to the target with smoothing ease out
        transform.forward = Vector3.Lerp(
            transform.forward,
            moveDirection,
            Time.deltaTime * rotateSpeed
        );
    }

    // public void Move(Vector3 targetPosition)
    // {
    //     this.targetPosition = targetPosition;
    // }

    // public void Move(GridPosition gridPosition, Action onActionComplete)
    // {
    //     isActive = true;
    //
    //     this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    //
    //     this.onActionComplete = onActionComplete;
    // }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isActive = true;

        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        this.onActionComplete = onActionComplete;
    }

    // public bool IsValidActionGridPosition(GridPosition gridPosition)
    // {
    //     List<GridPosition> validGridPositionList = GetValidActionGridPostionList();
    //
    //     return validGridPositionList.Contains(gridPosition);
    // }

    public override List<GridPosition> GetValidActionGridPostionList()
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
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
