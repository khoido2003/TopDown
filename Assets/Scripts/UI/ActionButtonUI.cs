using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    [SerializeField]
    private Button button;

    [SerializeField]
    private GameObject selectedGameObject;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.SetText(baseAction.GetActionName().ToUpper());

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedBaseAction();

        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }
}
