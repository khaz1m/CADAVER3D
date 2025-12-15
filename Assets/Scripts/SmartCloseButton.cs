using UnityEngine;
using UnityEngine.UI;

public class SmartCloseButton : MonoBehaviour
{
    // OPTIONAL: If the auto-detect fails, you can drag a specific panel here manually.
    // If you leave this empty, the script will guess automatically.
    public GameObject manualOverridePanel;

    void Start()
    {
        // 1. Automatically find the button component on THIS object
        Button myButton = GetComponent<Button>();

        // 2. Automatically add the click listener
        if (myButton != null)
        {
            myButton.onClick.AddListener(CloseLogic);
        }
        else
        {
            Debug.LogError("SmartCloseButton script must be attached to a Button object!");
        }
    }

    void CloseLogic()
    {
        // Option A: Use the manual slot if you filled it in
        if (manualOverridePanel != null)
        {
            manualOverridePanel.SetActive(false);
        }
        // Option B: Auto-Detect (The "Lazy" Mode)
        else
        {
            // Hierarchy assumes: 
            // 1. Main Panel (Grandparent) -> 2. Button Container (Parent) -> 3. X Button (This)
            
            if (transform.parent != null && transform.parent.parent != null)
            {
                transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                // Backup: If there is no grandparent, close the immediate parent
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}