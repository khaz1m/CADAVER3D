using UnityEngine;
using UnityEngine.UI;

public class AutoClosePanel : MonoBehaviour
{
    // If you leave this empty, the script will hunt for a parent named "Root"
    public GameObject manualPanel; 

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(CloseThePanel);
        }
    }

    void CloseThePanel()
    {
        // 1. If you assigned a panel manually, close that.
        if (manualPanel != null)
        {
            manualPanel.SetActive(false);
            return;
        }

        // 2. "Smart" Search: Look for a parent with "Root" in the name
        Transform currentParent = transform.parent;

        // Loop while there are still parents to check
        while (currentParent != null)
        {
            if (currentParent.name.Contains("Root"))
            {
                currentParent.gameObject.SetActive(false);
                return; // Found it! Stop looking.
            }
            
            // Keep climbing up to the next parent
            currentParent = currentParent.parent;
        }

        // 3. Backup: If we found nothing, just log a warning
        Debug.LogWarning("AutoClosePanel could not find a parent with 'Root' in the name!");
    }
}