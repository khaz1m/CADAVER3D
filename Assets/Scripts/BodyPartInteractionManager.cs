using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class BodyPartInteractionManager : MonoBehaviour
{
    [Header("High-level parts (Root: e.g., SPINE, Arm, Leg)")]
    public List<GameObject> majorParts;

    [Header("Detailed parts (e.g., Radius, Ulna, Tibia, etc.)")]
    public List<GameObject> detailedParts;

    void Start()
    {
        // Default mode: Major parts only
        SetInteractableForHierarchy(majorParts, true);
        SetInteractableForHierarchy(detailedParts, false);
    }

    // --- Enable/Disable entire hierarchy of interactables & colliders ---
    private void SetInteractableForHierarchy(List<GameObject> roots, bool enable)
    {
        foreach (GameObject root in roots)
        {
            if (root == null) continue;

            // Get ALL components in this object AND its children
            RayInteractable[] rayInteracts = root.GetComponentsInChildren<RayInteractable>(true);
            GrabInteractable[] grabInteracts = root.GetComponentsInChildren<GrabInteractable>(true);
            Oculus.Interaction.HandGrab.HandGrabInteractable[] handGrabs = root.GetComponentsInChildren<Oculus.Interaction.HandGrab.HandGrabInteractable>(true);
            Collider[] colliders = root.GetComponentsInChildren<Collider>(true);

            // Disable/Enable Ray Interactables
            foreach (var r in rayInteracts) r.enabled = enable;

            // Disable/Enable Grab Interactables
            foreach (var g in grabInteracts) g.enabled = enable;

            // Disable/Enable HandGrab Interactables
            foreach (var h in handGrabs) h.enabled = enable;

            // Disable/Enable Colliders (important!)
            foreach (var c in colliders) c.enabled = enable;
        }
    }

    // UI BUTTON: Enter Detail Mode
    public void EnterDetailMode()
    {
        SetInteractableForHierarchy(majorParts, false);
        SetInteractableForHierarchy(detailedParts, true);
    }

    // UI BUTTON: Exit Detail Mode
    public void ExitDetailMode()
    {
        SetInteractableForHierarchy(majorParts, true);
        SetInteractableForHierarchy(detailedParts, false);
    }
}
