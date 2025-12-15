using UnityEngine;

public class SimpleReset : MonoBehaviour
{
    public void ResetObject()
    {
        ResetController.Instance.ResetTarget(gameObject);
    }
}
