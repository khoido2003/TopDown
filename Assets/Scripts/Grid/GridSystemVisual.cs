using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField]
    private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    public static GridSystemVisual Instance { get; private set; }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft,
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    [SerializeField]
    private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridSystemVisualSingleTransform = Instantiate(
                    gridSystemVisualSinglePrefab,
                    LevelGrid.Instance.GetWorldPosition(gridPosition),
                    Quaternion.identity
                );

                gridSystemVisualSingleArray[x, z] =
                    gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged +=
            UnitActionSystem_OnSelectedActionChanged;

        LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;

        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    // private void Update()
    // {
    //     UpdateGridVisual();
    // }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(
        List<GridPosition> gridPositionList,
        GridVisualType gridVisualType
    )
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z]
                .Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void ShowGridPositionRange(
        GridPosition gridPosition,
        int range,
        GridVisualType gridVisualType
    )
    {
        List<GridPosition> gridPositionList = new();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void UpdateGridVisual()
    {
        HideAllGridPosition();

        if (UnitActionSystem.Instance == null)
        {
            Debug.LogError("UnitActionSystem.Instance is NULL!");
            return;
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedBaseAction();

        if (selectedAction == null)
        {
            Debug.LogError("selectedBaseAction is NULL!");
            return;
        }

        List<GridPosition> validList = selectedAction.GetValidActionGridPostionList();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(
                    selectedUnit.GetGridPosition(),
                    shootAction.GetMaxShootDistance(),
                    GridVisualType.RedSoft
                );
                break;
        }
        ShowGridPositionList(validList, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.Log("Could not find Material for GridVisualType: " + gridVisualType);
        return null;
    }
}
