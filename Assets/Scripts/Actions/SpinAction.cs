using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    public delegate void SpinCompletedDelegate();
    private float totalSpintAmount = 0;

    // Custom delegate
    // private SpinCompletedDelegate onSpinComplete;

    // private Action onSpinComplete;
    // Built in delegate: Action, Func

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpintAmount += spinAddAmount;
        if (totalSpintAmount >= 360)
        {
            isActive = false;

            // Call the delegate function
            this.onActionComplete();
        }
    }

    // Normal way
    public void Spin(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpintAmount = 0f;
    }

    public override List<GridPosition> GetValidActionGridPostionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }

    // Generic way
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpintAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
