using UnityEngine;
using TMPro;

public class NameTag : MonoBehaviour
{
    [SerializeField] private TMP_Text textMesh;   // link auto-assigned
    [SerializeField] private string label;        // you edit this at the NameTag level

    private void Reset()
    {
        // auto find TMP_Text in children when script is added
        textMesh = GetComponentInChildren<TMP_Text>();
    }

    private void OnValidate()
    {
        // update text in editor when changed
        if (textMesh == null)
            textMesh = GetComponentInChildren<TMP_Text>();

        if (textMesh != null)
            textMesh.text = label;
    }

    // Optional: allow changing name via script
    public void SetLabel(string newLabel)
    {
        label = newLabel;
        if (textMesh != null)
            textMesh.text = newLabel;
    }
}
