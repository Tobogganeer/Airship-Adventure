using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange = 4;
    public LayerMask interactLayers;
    public Transform interactFrom;

    public static IInteractable CurrentInteractable { get; private set; }
    static IInteractable lookingAt;

    public static bool Interacting => CurrentInteractable != null && CurrentInteractable.IsInteracting;
    
    private void Update()
    {
        FetchInteractables();

        if (PlayerInputs.Secondary)
        {
            if (CurrentInteractable != null && CurrentInteractable.IsInteracting)
            {
                CurrentInteractable.OnInteract();
                if (CurrentInteractable.IsInteracting == false)
                {
                    CurrentInteractable = null;
                }

                return;
            }
            else
            {
                CurrentInteractable = null;
            }

            if (lookingAt != null)
            {
                lookingAt.OnInteract();
                if (lookingAt.IsInteracting)
                {
                    CurrentInteractable = lookingAt;
                }
                else
                {
                    CurrentInteractable = null;
                }
            }
        }
    }

    private void FetchInteractables()
    {
        if (Physics.Raycast(interactFrom.position, interactFrom.forward, out RaycastHit hit, interactRange, interactLayers, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.TryGetComponent(out lookingAt))
            {
                HUD.SetInteract(!lookingAt.IsInteracting);
                return;
            }
        }

        HUD.SetInteract(false);
        lookingAt = null;
    }
}
