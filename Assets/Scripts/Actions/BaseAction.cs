using System;
using System.Collections.Generic;
using UnityEngine;

// Add abstract to avoid init this class
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStart;
    public static event EventHandler OnAnyActionComplete;

    protected Unit unit;
    protected bool isActive;

    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPostionList();

        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPostionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStart?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionComplete?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyActionAI GetBestEnemyAIAction()
    {
        List<EnemyActionAI> enemyActionAIList = new List<EnemyActionAI>();

        List<GridPosition> validGridGridPositionList = GetValidActionGridPostionList();

        foreach (GridPosition gridPosition in validGridGridPositionList)
        {
            EnemyActionAI enemyActionAI = GetEnemyActionAI(gridPosition);

            enemyActionAIList.Add(enemyActionAI);
        }

        if (enemyActionAIList.Count > 0)
        {
            enemyActionAIList.Sort(
                (EnemyActionAI a, EnemyActionAI b) =>
                {
                    return b.actionValue - a.actionValue;
                }
            );
            return enemyActionAIList[0];
        }
        else
        {
            // No possible Enemy AI Action
            return null;
        }
    }

    public abstract EnemyActionAI GetEnemyActionAI(GridPosition gridPosition);
}
