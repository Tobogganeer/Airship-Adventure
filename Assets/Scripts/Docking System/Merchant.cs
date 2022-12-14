using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public Transform InteractFrom { get; set; }

    bool IInteractable.FixedPosition => true;
    public bool IsInteracting { get; set; }

    public MerchantDock dock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
    }
}
