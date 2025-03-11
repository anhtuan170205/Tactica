using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public event EventHandler OnTurnChanged;

    private int turnNumber = 1;
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
