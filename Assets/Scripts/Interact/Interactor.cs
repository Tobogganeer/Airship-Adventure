using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private static Interactor instance;
    private void Awake()
    {
        instance = this;
    }

    public float interactRange = 4;
    public LayerMask interactLayers;
    public Transform interactFrom;
    public static Transform InteractFrom => instance.interactFrom;
    public static float InteractRange => instance.interactRange;

    public static IInteractable CurrentInteractable { get; private set; }
    static IInteractable lookingAt;

    public static bool Interacting => CurrentInteractable != null && CurrentInteractable.IsInteracting;

    //public string die;

    private void Start()
    {
        CurrentInteractable = null;
        lookingAt = null;
    }

    private void Update()
    {
        FetchInteractables();

        if (PlayerInputs.Secondary && !Cursor.visible)
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
        if (PlayerInputs.Primary && !Cursor.visible)
        {
            if (CurrentInteractable != null && CurrentInteractable.IsInteracting)
            {
                if (CurrentInteractable.transform.TryGetComponent(out ILMBInteractable lmb))
                    lmb.OnLMBInteract();
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

    public static void OnDestroy(IInteractable i)
    {
        if (CurrentInteractable == i)
            CurrentInteractable = null;
    }

    private void OnDrawGizmos()
    {
        if (Physics.Raycast(interactFrom.position, interactFrom.forward, out RaycastHit hit, interactRange, interactLayers, QueryTriggerInteraction.Collide))
        {
            Gizmos.color = Color.red;

            if (hit.transform.TryGetComponent(out lookingAt))
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawSphere(hit.point, 0.1f);
            //die = hit.transform.name;
        }

        if (CurrentInteractable != null)
        {
            Gizmos.color = Color.green;
        }
        else if (lookingAt != null)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawLine(interactFrom.position, interactFrom.position + interactFrom.forward * interactRange);
    }
}
