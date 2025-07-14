using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    private bool isBusy;

    [SerializeField]
    private Unit selectedUnit;

    [SerializeField]
    private LayerMask unitLayerMask;

    public event EventHandler OnSelectedUnitChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one UnitActionSystem! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection())
            {
                return;
            }

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(
                MouseWorld.GetPosition()
            );

            // Check if the position is valid to move
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetIsBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearIsBusy);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetIsBusy();
            selectedUnit.GetSpinAction().Spin(ClearIsBusy);
        }
    }

    private void SetIsBusy()
    {
        isBusy = true;
    }

    private void ClearIsBusy()
    {
        isBusy = false;
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, float.MaxValue, unitLayerMask))
        {
            Unit unit = raycastHit.transform.GetComponent<Unit>();

            if (unit != null)
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
