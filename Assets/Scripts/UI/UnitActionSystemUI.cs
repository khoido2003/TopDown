using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField]
    private Transform actionButtonPrefab;

    [SerializeField]
    private Transform actionButtonContainerTransform;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        // Clear old button from previous unit
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(
                actionButtonPrefab,
                actionButtonContainerTransform
            );

            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();

            actionButtonUI.SetBaseAction(baseAction);
        }
    }
}
