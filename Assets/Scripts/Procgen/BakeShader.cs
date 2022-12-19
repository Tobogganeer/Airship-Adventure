using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeShader : MonoBehaviour
{
    public Renderer mapMaterialObject;
    Material setTexTo;
    Texture2D tex;

    //public Material mat;

    //void Update()
    //{
    //    
    //}

    public float[,] Bake(int res, Material mat)
    {
        if (setTexTo == null && Application.isPlaying)
            setTexTo = mapMaterialObject.material;

        RenderTexture renderTexture = RenderTexture.GetTemporary(res, res);
        Graphics.Blit(null, renderTexture, mat);

        if (tex == null)
            tex = new Texture2D(res, res, TextureFormat.RGBA32, 0, true);// { anisoLevel = 9 };
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, res, res), 0, 0);

        float[,] heights = new float[res, res];
        //int logs = 100;

        for (int i = 0; i < res; i++)
        {
            for (int j = 0; j < res; j++)
            {
                heights[i, j] = tex.GetPixel(res - i, j).g;
                //if (heights[i, j] > 0.1f && logs-- > 0)
                //{
                //    Debug.Log($"[{i}, {j}] > 0.1f");
                //}
            }
        }

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        tex.Apply();
        if (Application.isPlaying)
            setTexTo.SetTexture("_MainTex", tex);
        //setTexTo.u
            //setTexTo.mainTexture = texture;
        //DestroyImmediate(texture);

        return heights;
        //texture.Apply();
    }

    private void OnDestroy()
    {
        if (setTexTo != null)
            Destroy(setTexTo);
    }
}
