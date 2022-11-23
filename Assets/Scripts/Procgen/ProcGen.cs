using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProcGen : MonoBehaviour
{
    public Material terrainMat;
    public TerrainHeight terrain;

    [Space]
    public NoiseSettings prec;
    public NoiseSettings temp;
    public MainSettings main;

    [System.Serializable]
    public class MainSettings
    {
        public float scale = 750f;
        public Vector2 offset = Vector2.zero;
        public float frequency = 6;
        public float gain = 9;
        public float lacunarity = 10;
        public float warpMult = 100;

        public void SetToMat(Material mat)
        {
            mat.SetFloat("_Scale", scale);
            mat.SetVector("_Offset", offset);
            mat.SetFloat("_Frequency", frequency);
            mat.SetFloat("_Gain", gain);
            mat.SetFloat("_Lacunarity", lacunarity);
            mat.SetFloat("_Warp_Mult", warpMult);
        }
    }


    Material mat;
    private void Start()
    {
        //mat = GetComponent<Renderer>().material;
        mat = Instantiate(terrainMat);
        terrain.terrain.materialTemplate = mat;
        UploadValues();

        terrain.temp = temp.Get();
        terrain.prec = prec.Get();
        terrain.main = main;
        terrain.SetHeight();
    }
    private void OnDestroy()
    {
        Destroy(mat);
    }

    void UploadValues()
    {
        prec.SetToMat(mat, "P");
        temp.SetToMat(mat, "T");
        main.SetToMat(mat);
    }
}
