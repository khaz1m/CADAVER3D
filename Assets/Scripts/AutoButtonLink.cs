using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AutoButtonLink : MonoBehaviour
{
    [Header("Assign the button manually (recommended for prefabs)")]
    public Button targetButton;

    [Header("What should the button do?")]
    public UnityEvent onClickAction;

    private void Reset()
    {
        // auto-fill only when adding the component
        targetButton = GetComponentInChildren<Button>();
    }

    private void Awake()
    {
        if (targetButton != null)
        {
            targetButton.onClick.RemoveAllListeners();
            targetButton.onClick.AddListener(() => onClickAction.Invoke());
        }
    }
}
