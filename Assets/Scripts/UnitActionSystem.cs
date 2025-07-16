using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public event EventHandler OnSelectedActionChanged;

    public event EventHandler<BusyActionArgs> OnBusyActionChanged;

    public class BusyActionArgs : EventArgs
    {
        public bool isBusy;
    }

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

        // Prevent handling input if clicking over UI
        // If mouse on UI element then not do any action
        if (EventSystem.current.IsPointerOverGameObject())
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
            if (selectedBaseAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetIsBusy();
                selectedBaseAction.TakeAction(mouseGridPosition, ClearIsBusy);
            }

            // // Withour generic code -> Use this
            // switch (selectedBaseAction)
            // {
            //     case MoveAction moveAction:
            //         // Check if the position is valid to move
            //         if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            //         {
            //             SetIsBusy();
            //             selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearIsBusy);
            //         }
            //         break;
            //
            //     case SpinAction spinAction:
            //
            //         SetIsBusy();
            //         spinAction.Spin(ClearIsBusy);
            //         break;
            // }
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
                    // Unit is already selected
                    // Add this so click on the unit does not select unit but do the action instead
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
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
        OnBusyActionChanged?.Invoke(this, new BusyActionArgs { isBusy = isBusy });
    }

    private void ClearIsBusy()
    {
        isBusy = false;

        OnBusyActionChanged?.Invoke(this, new BusyActionArgs { isBusy = isBusy });
    }

    private void SetSelectedUnit(Unit unit)
    {
        if (selectedUnit == unit)
        {
            SetSelectedAction(unit.GetMoveAction());

            return;
        }

        selectedUnit = unit;

        // Set default action to Move
        SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedBaseAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedBaseAction()
    {
        return selectedBaseAction;
    }
}
