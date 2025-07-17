using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private bool isPlayerTurn = true;

    private void Awake()
    {
        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool GetIsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
