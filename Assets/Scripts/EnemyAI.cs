using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        timer = 2f;
    }

    private void Update()
    {
        if (TurnSystem.Instance.GetIsPlayerTurn())
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}
