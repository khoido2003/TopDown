using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpintAmount = 0;

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
            totalSpintAmount = 0;
        }
    }

    public void Spin()
    {
        isActive = true;
    }
}
