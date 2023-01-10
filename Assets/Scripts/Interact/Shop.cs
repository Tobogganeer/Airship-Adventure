using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject coins;
    public List<Transform> crates;

    private void OnTriggerEnter(Collider other)
    {
        if (other.HasTag("Crate"))
        {
            //Crate c = other.GetComponent<Crate>();
            if (!crates.Contains(other.transform))
            {
                crates.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.HasTag("Crate"))
        {
            Crate c = other.GetComponent<Crate>();
            if (crates.Contains(other.transform))
            {
                crates.Remove(other.transform);
            }
        }
    }


    // Called by lever
    public void Sell()
    {
        for (int i = 0; i < crates.Count; i++)
        {
            Instantiate(coins, crates[i].position,
                Quaternion.Euler(0, Random.Range(0, 360), 0));
            Destroy(crates[i].gameObject);
        }

        crates = new List<Transform>();
    }
}
