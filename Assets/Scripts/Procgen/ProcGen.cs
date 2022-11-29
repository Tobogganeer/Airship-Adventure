using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProcGen : MonoBehaviour
{
    public Material terrainMat;
    public Material mapMat;
    public Material bakeMat;
    public TerrainHeight terrain;

    [Space]
    public NoiseSettings prec;
    public NoiseSettings temp;
    public MainSettings main;

    [System.Serializable]
    public class MainSettings
    {
        public float scale = 750f;
        public float terrainScale = 5f;
        public Vector2 offset = Vector2.zero;
        public float frequency = 6;
        public float gain = 9;
        public float lacunarity = 10;
        public float warpMult = 100;

        public void SetToMat(Material mat, bool useTerrainScale = false)
        {
            mat.SetFloat("_Scale", useTerrainScale ? terrainScale : scale);
            mat.SetVector("_Offset", offset);
            mat.SetFloat("_Frequency", frequency);
            mat.SetFloat("_Gain", gain);
            mat.SetFloat("_Lacunarity", lacunarity);
            mat.SetFloat("_Warp_Mult", warpMult);
        }
    }


    //Material c_mat;
    private void Start()
    {
        //mat = GetComponent<Renderer>().material;
        //c_mat = Instantiate(terrainMat);
        //terrain.terrain.materialTemplate = c_mat;
        UploadValues();

        terrain.temp = temp;
        terrain.prec = prec;
        //terrain.tempNoise = temp.Get();
        //terrain.precNoise = prec.Get();
        terrain.main = main;
        terrain.SetHeight();
    }
    private void OnDestroy()
    {
        //Destroy(c_mat);
    }

    public void Gen()
    {
        UploadValues();

        terrain.temp = temp;
        terrain.prec = prec;
        //terrain.tempNoise = temp.Get();
        //terrain.precNoise = prec.Get();
        terrain.main = main;
        terrain.SetHeight();
    }

    void UploadValues()
    {
        prec.SetToMat(terrainMat, "P");
        temp.SetToMat(terrainMat, "T");
        main.SetToMat(terrainMat, true);

        prec.SetToMat(mapMat, "P");
        temp.SetToMat(mapMat, "T");
        main.SetToMat(mapMat);

        prec.SetToMat(bakeMat, "P");
        temp.SetToMat(bakeMat, "T");
        main.SetToMat(bakeMat);
    }
}
