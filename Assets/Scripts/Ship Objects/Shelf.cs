using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public List<ShelfObject> shelfList = new List<ShelfObject>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ShelfObject obj) && !shelfList.Contains(obj))
        {
            shelfList.Add(obj);
            obj.onShelf = true;

            //other.gameObject.transform.localScale *= 4f;
            //Destroy(other.gameObject);
            
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ShelfObject obj) && shelfList.Contains(obj))
        {
            shelfList.Remove(obj);
            obj.onShelf = false;

            //other.gameObject.transform.localScale *= 0.25f;
        }
    }
}
