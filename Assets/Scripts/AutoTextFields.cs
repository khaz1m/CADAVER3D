using UnityEngine;
using TMPro;

public class AutoTextFields : MonoBehaviour
{
    [Header("Text Content")]
    public string titleText;
    [TextArea] public string descriptionText;

    [Header("Auto-Assigned")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;

    private void Reset()
    {
        // Auto-assign if in the expected hierarchy
        title = transform.Find("Title")?.GetComponent<TextMeshProUGUI>();
        desc = transform.Find("Desc")?.GetComponent<TextMeshProUGUI>();
    }

    private void OnValidate()
    {
        // Update texts in editor instantly
        if (title != null)
            title.text = titleText;

        if (desc != null)
            desc.text = descriptionText;
    }
}
