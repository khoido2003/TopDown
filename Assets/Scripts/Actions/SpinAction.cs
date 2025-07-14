using System;
using UnityEngine;

public class SpinAction : BaseAction
{
    public delegate void SpinCompletedDelegate();
    private float totalSpintAmount = 0;

    // Custom delegate
    // private SpinCompletedDelegate onSpinComplete;

    // private Action onSpinComplete;
    // Built in delegate: Action, Func


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

    public void Spin(Action onActionComplete)
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
