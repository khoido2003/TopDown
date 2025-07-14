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

    private BaseAction selectedBaseAction;

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

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(
                MouseWorld.GetPosition()
            );
            switch (selectedBaseAction)
            {
                case MoveAction moveAction:
                    // Check if the position is valid to move
                    if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                    {
                        SetIsBusy();
                        selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearIsBusy);
                    }
                    break;

                case SpinAction spinAction:

                    SetIsBusy();
                    spinAction.Spin(ClearIsBusy);
                    break;
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
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
        }
        return false;
    }

    private void SetIsBusy()
    {
        isBusy = true;
    }

    private void ClearIsBusy()
    {
        isBusy = false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedBaseAction = baseAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
