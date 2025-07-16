using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }
}
