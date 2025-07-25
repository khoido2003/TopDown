using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private int maxShootDistance = 5;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    public override string GetActionName()
    {
        return "Shoot";
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:

                float rotateSpeed = 10f;

                Vector3 aimDir = (targetUnit.GetWorldPosition() - transform.position).normalized;

                // Rotate the character to the target with smoothing ease out
                transform.forward = Vector3.Lerp(
                    transform.forward,
                    aimDir,
                    Time.deltaTime * rotateSpeed
                );

                break;

            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;

            case State.Cooloff:

                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(
            this,
            new OnShootEventArgs { targetUnit = targetUnit, shootingUnit = unit }
        );

        targetUnit.Damage(40);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:

                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;

            case State.Shooting:

                state = State.Cooloff;
                float cooloffStateTime = 0.5f;
                stateTimer = cooloffStateTime;
                break;

            case State.Cooloff:

                ActionComplete();
                break;
        }
    }

    public override List<GridPosition> GetValidActionGridPostionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPostionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPostionList(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPostion;

                // Check if the grid position is valid
                if (!LevelGrid.Instance.IsValidPosition(testGridPosition))
                {
                    continue;
                }

                // check if it is inside the range
                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                // Grid position list is empty, no Unit
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(testGridPosition);

                // Both unit on the same team
                if (targetUnit.GetIsEnemy() == unit.GetIsEnemy())
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyActionAI GetEnemyActionAI(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        return new EnemyActionAI
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPostionList(gridPosition).Count;
    }
}
