using System.Collections;
using UnityEngine;
using Oculus.Interaction; // optional, keeps inspector typed if you have the SDK

public class ReturnToSnapRegion : MonoBehaviour
{
    [System.Serializable]
    public class SnapPair
    {
        // Keep typed references so you can drag the correct components in inspector
        public SnapInteractor interactor;
        public SnapInteractable interactable;

        // Optional: if your interactable has a dedicated child "SnappingRegion" use that instead of root
        public string snappingRegionChildName = "SnappingRegion";
    }

    [Tooltip("List all interactor -> interactable pairs here.")]
    public SnapPair[] bones;

    [Tooltip("If > 0, parts will smoothly move back over this duration (seconds). If 0, instant snap.")]
    public float smoothReturnDuration = 0.25f;

    [Tooltip("If true, re-enable physics (isKinematic = false) after returning.")]
    public bool restorePhysics = true;

    public void ReturnAllToSnap()
    {
        foreach (var bone in bones)
        {
            if (bone == null || bone.interactor == null || bone.interactable == null)
            {
                Debug.LogWarning("[ReturnToSnapRegion] Missing pair or component in bones list.");
                continue;
            }

            // Figure out which GameObject to move: usually the interactor's root GameObject
            GameObject objectToMove = bone.interactor.gameObject;

            // Determine the target transform: prefer a SnappingRegion child if present
            Transform target = GetSnappingTarget(bone.interactable.transform, bone.snappingRegionChildName);

            if (target == null)
            {
                Debug.LogWarning($"[ReturnToSnapRegion] No snapping target found for {bone.interactable.name}. Using its root transform instead.");
                target = bone.interactable.transform;
            }

            // Start coroutine to move it (instant if duration==0)
            StartCoroutine(MoveObjectToTarget(objectToMove, target, smoothReturnDuration, restorePhysics));
        }
    }

    private Transform GetSnappingTarget(Transform interactableRoot, string childName)
    {
        if (!string.IsNullOrEmpty(childName))
        {
            var child = interactableRoot.Find(childName);
            if (child != null) return child;
        }
        return interactableRoot;
    }

    private IEnumerator MoveObjectToTarget(GameObject obj, Transform target, float duration, bool restorePhysics)
    {
        if (obj == null || target == null) yield break;

        // Get the rigidbody on the object (if any) and make it kinematic so physics doesn't fight us
        Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
        bool hadRigidbody = rb != null;
        bool prevKinematic = false;
        if (rb != null)
        {
            prevKinematic = rb.isKinematic;
            rb.isKinematic = true;
        }

        Transform t = obj.transform;
        Vector3 startPos = t.position;
        Quaternion startRot = t.rotation;
        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        if (duration <= 0f)
        {
            t.position = endPos;
            t.rotation = endRot;
        }
        else
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsed / duration);
                // smooth step easing
                float eased = Mathf.SmoothStep(0f, 1f, alpha);
                t.position = Vector3.Lerp(startPos, endPos, eased);
                t.rotation = Quaternion.Slerp(startRot, endRot, eased);
                yield return null;
            }
            t.position = endPos;
            t.rotation = endRot;
        }

        // Optionally re-enable physics
        if (rb != null && restorePhysics)
        {
            rb.isKinematic = prevKinematic; // restore previous state (usually false)
        }
    }
}
