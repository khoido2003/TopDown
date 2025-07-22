using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField]
    private Transform gridDebugObjectPrefab;

    private GridSystem<GridObject> gridSystem;

    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMoveGridPosition;

    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem<GridObject>(
            10,
            10,
            2f,
            (GridSystem<GridObject> g, GridPosition gridPosition) =>
            {
                return new GridObject(g, gridPosition);
            }
        );
        gridSystem.CreateDebugObject(gridDebugObjectPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUniListtAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public void UnitMoveGridPosition(
        Unit unit,
        GridPosition fromGridPosition,
        GridPosition toGridPosition
    )
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public bool IsValidPosition(GridPosition gridPosition)
    {
        return gridSystem.IsValidPosition(gridPosition);
    }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public int GetWidth()
    {
        return gridSystem.GetWidth();
    }

    public int GetHeight()
    {
        return gridSystem.GetHeight();
    }
}
