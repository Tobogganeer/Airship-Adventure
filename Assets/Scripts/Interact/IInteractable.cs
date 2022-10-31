using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool FixedPosition { get; }
    Transform transform { get; }
    bool IsInteracting { get; }
    Transform InteractFrom { get; }
    void OnInteract();
}
