using UnityEngine;

public class ResetController : MonoBehaviour
{
    public static ResetController Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ResetTarget(GameObject target)
    {
        StartCoroutine(ResetRoutine(target));
    }

    private System.Collections.IEnumerator ResetRoutine(GameObject target)
    {
        target.SetActive(false);
        yield return null;  // wait 1 frame
        target.SetActive(true);
    }
}
