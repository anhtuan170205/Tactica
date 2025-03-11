using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnButton;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnNumberText();
    }

    public void UpdateTurnNumberText()
    {
        turnNumberText.text = "Turn: " + TurnSystem.Instance.GetTurnNumber();
    }
    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateTurnNumberText();
    }

}
