using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BakeShader : MonoBehaviour
{
    public int res = 513;
    public Material mat;

    [Space]
    public Material showMat;

    Texture2D texture;

    private void Start()
    {
        texture = new Texture2D(res, res);
        showMat.mainTexture = texture;
    }

    //void Update()
    //{
    //    
    //}

    [ContextMenu("Bake")]
    void Bake()
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(res, res);
        Graphics.Blit(null, renderTexture, mat);

        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, res, res), 0, 0);

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        //DestroyImmediate(texture);
        texture.Apply();
    }
}
