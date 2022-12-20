using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeTransition : MonoBehaviour
{
    public Direction dir;
    [ReadOnly] public Biome currentBiome;
    public float tpOffset = 250;

    [Space]
    public GameObject grasslandsIcon;
    public GameObject desertIcon;
    public GameObject snowIcon;

    [ReadOnly] public bool active;

    private void OnTriggerEnter(Collider other)
    {
        if (other.HasTag("Player") && active)
        {
            Transition();
        }
    }

    void Transition()
    {
        float offset = dir == Direction.North ? tpOffset : -tpOffset;
        Airship.MoveAllObjects(Airship.Transform.position.WithZ(-Airship.Transform.position.z + offset));
        ProcGen.instance.currentBiome = currentBiome;
        ProcGen.instance.Gen();
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
        Gizmos.DrawWireSphere(transform.position + transform.forward * -tpOffset, 30f);
    }
}
