using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHeight : MonoBehaviour
{
    [HideInInspector] public Terrain terrain;

    [HideInInspector] public FastNoiseLite prec;
    [HideInInspector] public FastNoiseLite temp;
    [HideInInspector] public ProcGen.MainSettings main;

    int hmRes;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        hmRes = terrain.terrainData.heightmapResolution;
        SetHeight();
    }

    public void SetHeight()
    {
        float[,] heights = new float[hmRes, hmRes];
        for (int i = 0; i < hmRes; i++)
            for (int j = 0; j < hmRes; j++)
                heights[i, j] = GetHeight(i, j);

        terrain.terrainData.SetHeights(0, 0, heights);

    }

    float GetHeight(int x, int y)
    {
        // 0-1
        //Vector3 pos = transform.position;
        //pos.x /= transform.localScale.x;
        //pos.z /= transform.localScale.z;

        //Vector2 uv = new Vector2(pos.x, pos.z);
        Vector2 uv = new Vector2(x / terrain.terrainData.size.x * hmRes,
            y / terrain.terrainData.size.z * hmRes);

        uv += main.offset;

        Vector2 precUV = uv;
        //precUV += 

        Vector2 tempUV = uv;

        return 0;
    }
}
