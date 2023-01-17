using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHeightTesting : MonoBehaviour
{
    Terrain terrain;

    int hmRes;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        hmRes = terrain.terrainData.heightmapResolution;
        SetHeight();
    }

    void SetHeight()
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

        return 0;
    }
}
