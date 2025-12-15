using UnityEngine;

public class DisableListedObjects : MonoBehaviour
{
    [Tooltip("Assign the objects you want to turn OFF.")]
    public GameObject[] objectsToDisable;

    // Call this from a button or another script
    public void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
