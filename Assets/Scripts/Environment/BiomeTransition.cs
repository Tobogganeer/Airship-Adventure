using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BiomeTransition : MonoBehaviour
{
    public Direction dir;
    [ReadOnly] public Biome currentBiome;
    public float tpDistance = 3000;

    [Space]
    public GameObject grasslandsIcon;
    public GameObject desertIcon;
    public GameObject snowIcon;

    [ReadOnly] public bool active;

    private void OnTriggerEnter(Collider other)
    {
        if (other.HasTag("Player") && active)
        {
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        HUD.SetLoading(true);

        for (int i = 0; i < 10; i++)
            yield return null;

        //float offset = dir == Direction.North ? tpOffset : -tpOffset;
        //Airship.MoveAllObjects(Airship.Transform.position.WithZ(-Airship.Transform.position.z + offset));
        Airship.MoveAllObjects(Airship.Transform.position.WithZ(transform.position.z + tpDistance));
        ProcGen.instance.currentBiome = currentBiome;
        ProcGen.instance.Gen();

        for (int i = 0; i < 10; i++)
            yield return null;

        HUD.SetLoading(false);
        //SetIcon();
    }

    public void SetBiome(Biome currentWorldBiome)
    {
        active = false;

        if (dir == Direction.North)
        {
            switch (currentWorldBiome)
            {
                case Biome.Grasslands:
                    currentBiome = Biome.Desert;
                    active = true;
                    break;
                case Biome.Snow:
                    currentBiome = Biome.Grasslands;
                    active = true;
                    break;
            }
        }
        else
        {
            switch (currentWorldBiome)
            {
                case Biome.Desert:
                    currentBiome = Biome.Grasslands;
                    active = true;
                    break;
                case Biome.Grasslands:
                    currentBiome = Biome.Snow;
                    active = true;
                    break;
            }
        }

        SetIcon();
    }

    public void SetIcon()
    {
        grasslandsIcon.SetActive(false);
        desertIcon.SetActive(false);
        snowIcon.SetActive(false);

        if (!active) return;

        (currentBiome switch
        {
            Biome.Grasslands => grasslandsIcon,
            Biome.Desert => desertIcon,
            Biome.Snow => snowIcon,
            _ => null,
        }).SetActive(true);
    }

    public enum Direction
    {
        North,
        South
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position + transform.forward * -tpOffset, 30f);
        //Gizmos.DrawWireSphere(transform.position.WithZ(-transform.position.z + tpOffset), 30f);
        Gizmos.DrawWireSphere(transform.position.WithZ(transform.position.z + tpDistance), 30f);
        Gizmos.DrawLine(transform.position, transform.position.WithZ(transform.position.z + tpDistance));
    }
}
