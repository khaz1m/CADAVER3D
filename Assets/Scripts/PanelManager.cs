using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Tooltip("Leave empty: it will auto-find all direct child panels.")]
    public GameObject[] panels;

    void Awake()
    {
        // Auto-fill panels: takes all direct children under this object
        if (panels == null || panels.Length == 0)
        {
            int count = transform.childCount;
            panels = new GameObject[count];

            for (int i = 0; i < count; i++)
                panels[i] = transform.GetChild(i).gameObject;
        }
    }

    public void OpenPanel(string panelName)
    {
        foreach (var p in panels)
        {
            // Enable only the target panel
            p.SetActive(p.name == panelName);
        }
    }
}
