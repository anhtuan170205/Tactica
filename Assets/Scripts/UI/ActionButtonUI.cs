using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private Button actionButton;
    [SerializeField] private GameObject selectedGameObjec;
    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        actionNameText.text = baseAction.GetActionName().ToUpper();
        actionButton.onClick.AddListener(() => 
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObjec.SetActive(baseAction == selectedBaseAction);
    }
}
