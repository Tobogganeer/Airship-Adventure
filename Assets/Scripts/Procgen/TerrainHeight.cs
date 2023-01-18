using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHeight : MonoBehaviour
{
    public float height = 80f;
    public AnimationCurve heightCurve;

    public BiomeHeightSettings grasslands;
    public BiomeHeightSettings desert;
    public BiomeHeightSettings snow;
    //public float scale = 5f;

    public BakeShader baker;

    [HideInInspector] public Terrain terrain;

    [HideInInspector] public NoiseSettings prec;
    [HideInInspector] public NoiseSettings temp;
    [HideInInspector] public ProcGen.MainSettings main;

    int hmRes;

    public void SetHeight(Biome biome, Material mat)
    {
        terrain = GetComponent<Terrain>();
        hmRes = terrain.terrainData.heightmapResolution;

        /*
        float[,] heights = new float[hmRes, hmRes];
        for (int i = 0; i < hmRes; i++)
        {
            for (int j = 0; j < hmRes; j++)
            {
                float height = GetHeight(i, j);
                heights[i, j] = Remap.Float(height, 0, 1,
                    0, this.height / terrain.terrainData.size.y);
                //heights[i, j] = height * this.height;
                debugTex.SetPixel(i, j, new Color(height, height, height));
            }
        }
        */

        float[,] heights = baker.Bake(hmRes, mat);

        //float total = 0;

        BiomeHeightSettings settings = Get(biome);

        for (int i = 0; i < hmRes; i++)
        {
            for (int j = 0; j < hmRes; j++)
            {
                // Terrain height indexed as [y,x] actually..?
                //heights[j, i] = Remap.Float(heightCurve.Evaluate(heights[j, i]), 0, 1,
                heights[i, j] = Remap.Float(settings.heightCurve.Evaluate(heights[i, j]), 0, 1,
                    0, this.height / terrain.terrainData.size.y);
                //total += heights[i, j];
            }
        }

        //Debug.Log("Total height: " + total);

        terrain.terrainData.SetHeights(0, 0, heights);
        terrain.Flush();
        //debugTex.Apply();
    }

    BiomeHeightSettings Get(Biome biome) => biome switch
    {
        Biome.Grasslands => grasslands,
        Biome.Desert => desert,
        Biome.Snow => snow,
        _ => throw new System.NotImplementedException()
    };

    /*
    float GetHeight(int x, int y)
    {
        // 0-1
        //Vector3 pos = transform.position;
        //pos.x /= transform.localScale.x;
        //pos.z /= transform.localScale.z;

        //Vector2 uv = new Vector2(pos.x, pos.z);
        //Vector2 uv = new Vector2(x / terrain.terrainData.size.x * hmRes,
        //    y / terrain.terrainData.size.z * hmRes);
        Vector2 uv = new Vector2(x, y);

        uv += main.offset;
        uv *= scale;

        float precNoise = GetNoise(prec, uv);
        float tempNoise = GetNoise(temp, uv);

        float comb = precNoise + tempNoise;
        comb /= 2f;

        return comb;
    }

    Vector2 Warp(FastNoiseLite noise, Vector2 uv)
    {
        float qX = uv.x;
        float qY = uv.y;

        noise.DomainWarp(ref qX, ref qY);

        float rX = uv.x + 4 * qX + 1.7f;
        float rY = uv.y + 4 * qY + 9.2f;

        noise.DomainWarp(ref rX, ref rY);

        uv.x += 4 * rX;
        uv.y += 4 * rY;
        return uv;
    }

    float GetNoise(NoiseSettings noise, Vector2 uv)
    {
        uv += noise.offset;
        uv *= main.scale;
        uv *= noise.scale;


        //Warp(state, x, y);
        //value = (fnlGetNoise2D(state, x, y) + 1.0f) / 2.0f;

        FastNoiseLite n = noise.Get(main);
        uv = Warp(n, uv);
        return (n.GetNoise(uv.x, uv.y) + 1f) / 2f;
    }
    */

    [System.Serializable]
    public class BiomeHeightSettings
    {
        public AnimationCurve heightCurve;
    }
}
