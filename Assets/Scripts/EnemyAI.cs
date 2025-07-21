using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;

    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.GetIsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:

                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetTakeTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No more enemies have action -> end enemy turn
                        TurnSystem.Instance.NextTurn();
                    }
                }

                break;
            case State.Busy:
                break;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.GetIsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    // Delegate function
    private void SetTakeTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Taking Enemy action");

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryDoEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryDoEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }

        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            return false;
        }

        Debug.Log("Spin action");

        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);

        return true;
    }
}
