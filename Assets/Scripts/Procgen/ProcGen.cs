using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGen : MonoBehaviour
{
    public static ProcGen instance;
    private void Awake()
    {
        instance = this;

        if (mainMenuSeed >= 0)
        {
            if (mainMenuSeed == 0)
                seed = Random.Range(1, 100000);
            else
                seed = mainMenuSeed;
        }
        mainMenuSeed = 0;
    }

    public bool genOnStart = false;

    public Material grasslandsBakeMat;
    public Material desertBakeMat;
    public Material snowBakeMat;
    public Material mapMat;
    public TerrainHeight terrain;
    public StructureGen structure;

    Material bakeMat;

    [Space]
    public NoiseSettings prec;
    public NoiseSettings temp;
    public MainSettings main;
    int seed = 1130;
    [ReadOnly, Rename("Seed")] public int inspectorSeed;

    public static int mainMenuSeed = -1;
    public static Biome mainMenuBiome;
    public static bool useMainMenuBiome;

    public Biome currentBiome = Biome.Grasslands;
    public Material grasslands;
    public Material desert;
    public Material snow;

    [Space]
    public BiomeTransition north;
    public BiomeTransition south;

    //public BiomeSettings grasslands;
    //public BiomeSettings desert;
    //public BiomeSettings snow;

    [System.Serializable]
    public class MainSettings
    {
        public int seed = 0;
        public float scale = 750f;
        public Vector2 offset = Vector2.zero;

        public void SetToMat(Material mat)
        {
            mat.SetFloat("_Seed", seed);
            mat.SetFloat("_Scale", scale);
            mat.SetVector("_Offset", offset);
        }
    }

    /*
    [System.Serializable]
    public class BiomeSettings
    {
        [System.Serializable]
        public class BiomeColourSet
        {
            public Color low;
            public Color mid;
            public Color high;
        }


        public float hillPower = 0.7f;



        public BiomeColourSet normal;
        public BiomeColourSet hill;

        public void SetToMat(Material mat)
        {
            mat.SetFloat("_Hill_Power", hillPower);

            mat.SetColor("_LowColour", normal.low);
            mat.SetColor("_MidColour", normal.mid);
            mat.SetColor("_HighColour", normal.high);
            mat.SetColor("_LowColourHill", hill.low);
            mat.SetColor("_MidColourHill", hill.mid);
            mat.SetColor("_HighColourHill", hill.high);
        }
    }
    */

    private void Start()
    {
        if (genOnStart)
            Gen();
    }

    public void Gen()
    {
        if (useMainMenuBiome)
            currentBiome = mainMenuBiome;

        main.seed = seed + (int)currentBiome;
        inspectorSeed = seed;

        UploadValues();

        terrain.temp = temp;
        terrain.prec = prec;
        terrain.main = main;
        terrain.SetHeight(currentBiome, bakeMat);
        structure.Generate(main.seed);

        north.SetBiome(currentBiome);
        south.SetBiome(currentBiome);
    }

    void UploadValues()
    {
        prec.SetToMat(mapMat, "P");
        temp.SetToMat(mapMat, "T");
        main.SetToMat(mapMat);

        bakeMat = GetBakeMaterial(currentBiome);
        prec.SetToMat(bakeMat, "P");
        temp.SetToMat(bakeMat, "T");
        main.SetToMat(bakeMat);

        terrain.terrain = terrain.GetComponent<Terrain>();
        terrain.terrain.materialTemplate = GetColourMaterial(currentBiome);
        //GetColours(currentBiome).SetToMat(colourMat);
    }

    Material GetColourMaterial(Biome biome) => biome switch
    {
        Biome.Grasslands => grasslands,
        Biome.Desert => desert,
        Biome.Snow => snow,
        _ => throw new System.NotImplementedException()
    };

    Material GetBakeMaterial(Biome biome) => biome switch
    {
        Biome.Grasslands => grasslandsBakeMat,
        Biome.Desert => desertBakeMat,
        Biome.Snow => snowBakeMat,
        _ => throw new System.NotImplementedException()
    };
}
