using UnityEngine;
using UnityEngine.UI;

public class FavoriteToggle : MonoBehaviour
{
    [Header("Panel A (System Page)")]
    public Button systemButton; // The button you click to Add

    [Header("Panel B (Favorites Page)")]
    public GameObject favoriteObject; // The entire object to Show/Hide
    public Button favoriteButton;     // The button inside the favorite object to Remove

    void Start()
    {
        // 1. Ensure the Favorite object starts hidden
        if (favoriteObject != null)
        {
            favoriteObject.SetActive(false);
        }

        // 2. Listen for clicks on the System Page button (Button A)
        if (systemButton != null)
        {
            systemButton.onClick.AddListener(ToggleState);
        }

        // 3. Listen for clicks on the Favorites Page button (Button B)
        if (favoriteButton != null)
        {
            favoriteButton.onClick.AddListener(ToggleState);
        }
    }

    // This function simply looks at Object B. 
    // If it's On, turn it Off. If it's Off, turn it On.
    void ToggleState()
    {
        if (favoriteObject != null)
        {
            bool currentState = favoriteObject.activeSelf;
            favoriteObject.SetActive(!currentState);
        }
    }
}