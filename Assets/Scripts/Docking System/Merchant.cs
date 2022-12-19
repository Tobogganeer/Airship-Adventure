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

    private CursorController curcon;

    // Start is called before the first frame update
    void Start()
    {
        curcon = GameObject.FindObjectOfType<CursorController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
        dock.saleActive = IsInteracting;

        curcon.IntToggle();
    }
}
