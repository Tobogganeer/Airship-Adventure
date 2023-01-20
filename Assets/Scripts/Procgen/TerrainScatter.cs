using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScatter : MonoBehaviour
{
    public float diameter = 3500;
    public LayerMask sampleLayers;

    [Space]
    public BiomeScatter grasslandsScatter;
    public BiomeScatter desertScatter;
    public BiomeScatter snowScatter;

    //[Space]
    //public bool heightGizmos = false;

    Transform holder;

    [Space]
    public bool perlinGizmos;
    public int numGizmos = 100;
    public float perlinScale = 1.1f;
    public Vector2 range = Vector2.right;


    [ContextMenu("Test")]
    public void Test()
    {
        Generate(10);
    }

    public void Generate(int seed)
    {
        if (transform.childCount > 0)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(0).gameObject);
            else
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        holder = new GameObject("Scatter Holder").transform;
        holder.SetParent(transform);
        holder.localPosition = Vector3.zero;
        holder.localRotation = Quaternion.identity;
        holder.localScale = Vector3.one;

        using (SeededRNG.Block(seed))
        {
            BiomeScatter currentBiomeScatter = Current();

            for (int i = 0; i < currentBiomeScatter.scatter.Length; i++)
            {
                GenerateScatterLayer(currentBiomeScatter.scatter[i], i);
            }
        }
    }

    void GenerateScatterLayer(ScatterLayer layer, int index)
    {
        Transform layerHolder = new GameObject("Layer " + index).transform;
        layerHolder.SetParent(holder);

        Transform[] subLayers = new Transform[layer.objects.Length];

        for (int i = 0; i < subLayers.Length; i++)
        {
            Transform subLayerHolder = new GameObject("SubLayer " + index + "-" + i).transform;
            subLayerHolder.SetParent(layerHolder);
            subLayers[i] = subLayerHolder;
        }

        List<Vector3> points = GetPoints(layer);
        foreach (Vector3 position in points)
        {
            int sub = Random.Range(0, layer.objects.Length);
            Instantiate(layer.objects[sub], position, Quaternion.Euler(RandomVec(layer.spawnRotRandomization)), subLayers[sub]);
        }

        for (int i = 0; i < subLayers.Length; i++)
        {
            StaticBatchingUtility.Combine(subLayers[i].gameObject);
        }
    }

    Vector3 RandomVec(Vector3 range)
    {
        return new Vector3(
            Random.Range(-range.x, range.x),
            Random.Range(-range.y, range.y),
            Random.Range(-range.z, range.z)
        );
    }

    public List<Vector3> GetPoints(ScatterLayer layer)
    {
        List<Vector3> points = new List<Vector3>();

        float radius = diameter / 2f;
        float rndOffset = Random.value * 1000f;

        for (int x = 0; x < layer.sampleGridSize; x++)
        {
            for (int y = 0; y < layer.sampleGridSize; y++)
            {
                if (Random.value > layer.spawnChance) continue;

                Vector3 pt = new Vector3((float)x / layer.sampleGridSize * diameter, layer.maxHeight, (float)y / layer.sampleGridSize * diameter);
                pt.x -= radius;
                pt.z -= radius;
                //Debug.Log(pt);
                pt.x += Random.Range(-layer.sampleRandomOffset, layer.sampleRandomOffset);
                pt.z += Random.Range(-layer.sampleRandomOffset, layer.sampleRandomOffset);

                float val = Mathf.PerlinNoise(rndOffset + pt.x * layer.perlinScale, rndOffset + pt.z * layer.perlinScale);
                if (val < layer.perlinThreshold) continue;

                if (Physics.Raycast(new Ray(pt, Vector3.down), out RaycastHit hit, layer.maxHeight - layer.minHeight, sampleLayers))
                {
                    if (Vector3.Angle(Vector3.up, hit.normal) > layer.maxAngle) continue;
                    points.Add(hit.point);
                }
            }
        }

        return points;
    }

    private void OnDrawGizmos()
    {
        if (!perlinGizmos) return;

        for (int x = 0; x < numGizmos; x++)
        {
            for (int y = 0; y < numGizmos; y++)
            {
                Vector3 pos = new Vector3(x, 100, y);
                float val = Mathf.PerlinNoise(pos.x * perlinScale, pos.z * perlinScale);
                Color col = Color.Lerp(Color.red, Color.green, Remap.Float01(val, range.x, range.y));
                Gizmos.color = col;
                Gizmos.DrawLine(pos, pos + Vector3.up);
            }
        }
    }



    BiomeScatter Current()
    {
        if (ProcGen.instance == null) return grasslandsScatter;

        switch (ProcGen.instance.currentBiome)
        {
            case Biome.Grasslands:
                return grasslandsScatter;
            case Biome.Desert:
                return desertScatter;
            case Biome.Snow:
                return snowScatter;
        }

        throw new System.Exception();
    }

    [System.Serializable]
    public class BiomeScatter
    {
        public ScatterLayer[] scatter;
        //public float[] chances;
    }

    [System.Serializable]
    public class ScatterLayer
    {
        public GameObject[] objects;
        public float minHeight = 20f;
        public float maxHeight = 70f;
        public float maxAngle = 30f;
        public int sampleGridSize = 400;
        public float sampleRandomOffset = 4f;
        [Range(0f, 1f)]
        public float spawnChance = 0.8f;
        public float perlinScale;
        public float perlinThreshold;
        public Vector3 spawnRotRandomization;
    }
}
