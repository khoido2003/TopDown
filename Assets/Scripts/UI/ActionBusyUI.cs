using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        Hide();

        UnitActionSystem.Instance.OnBusyActionChanged += UnitActionSystem_OnBusyActionChanged;
    }

    private void UnitActionSystem_OnBusyActionChanged(
        object sender,
        UnitActionSystem.BusyActionArgs e
    )
    {
        if (e.isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
