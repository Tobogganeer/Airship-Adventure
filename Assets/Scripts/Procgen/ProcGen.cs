using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGen : MonoBehaviour
{
    public static ProcGen instance;
    private void Awake()
    {
        instance = this;
        if (mainMenuSeed == 0)
            main.seed = Random.Range(1, 100000);
        else
            main.seed = mainMenuSeed;
        mainMenuSeed = 0;
    }

    public bool genOnStart = false;

    public Material mapMat;
    public Material bakeMat;
    public TerrainHeight terrain;
    public StructureGen structure;

    [Space]
    public NoiseSettings prec;
    public NoiseSettings temp;
    public MainSettings main;

    public static int mainMenuSeed;

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

    private void Start()
    {
        if (genOnStart)
            Gen();
    }

    public void Gen()
    {
        UploadValues();

        terrain.temp = temp;
        terrain.prec = prec;
        terrain.main = main;
        terrain.SetHeight();
        structure.Generate(main.seed);
    }

    void UploadValues()
    {
        prec.SetToMat(mapMat, "P");
        temp.SetToMat(mapMat, "T");
        main.SetToMat(mapMat);

        prec.SetToMat(bakeMat, "P");
        temp.SetToMat(bakeMat, "T");
        main.SetToMat(bakeMat);
    }
}
