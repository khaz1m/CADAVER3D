using UnityEngine;
using Oculus.Interaction;

public class ObjectFilter : MonoBehaviour, IGameObjectFilter
{
    public GameObject allowedObject;

    public bool Filter(GameObject gameObject)
    {
        return gameObject == allowedObject;
    }
}
