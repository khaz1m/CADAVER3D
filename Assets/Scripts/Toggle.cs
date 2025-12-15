using UnityEngine;

public class ToggleInfoPanel : MonoBehaviour
{
    public GameObject infoPanel; // assign your Skull Info Panel in Inspector
    private bool isVisible = false;

    public void TogglePanel()
    {
        isVisible = !isVisible;
        infoPanel.SetActive(isVisible);
    }
}
