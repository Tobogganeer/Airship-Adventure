using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BakeShader : MonoBehaviour
{
    public Material mat;

    //void Update()
    //{
    //    
    //}

    public float[,] Bake(int res)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(res, res);
        Graphics.Blit(null, renderTexture, mat);

        Texture2D texture = new Texture2D(res, res);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, res, res), 0, 0);

        float[,] heights = new float[res, res];

        for (int i = 0; i < res; i++)
        {
            for (int j = 0; j < res; j++)
            {
                heights[i, j] = texture.GetPixel(i, j).r;
            }
        }

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        DestroyImmediate(texture);

        return heights;
        //texture.Apply();
    }
}
