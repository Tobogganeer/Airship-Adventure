using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatRenderer : MonoBehaviour
{
    public Renderer textureRenderer;
    public Color low = Color.black;
    public Color high = Color.white;
    private Texture2D tex;

    private void DrawTexture()
    {
        textureRenderer.sharedMaterial.mainTexture = tex;
        //textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    /// <summary>
    /// Heights should be in range 0-1
    /// </summary>
    /// <param name="heights"></param>
    public void Draw(float[,] heights, bool bilinear)
    {
        int width = heights.GetLength(0);
        int height = heights.GetLength(1);
        if (tex == null)
        {
            tex = new Texture2D(width, height);
            DrawTexture();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, Color.Lerp(low, high, heights[x, y]));
            }
        }

        tex.filterMode = !bilinear ? FilterMode.Point : FilterMode.Bilinear;

        tex.Apply();
        //DrawTexture(tex);
    }

    public void Draw(Color[,] heights, bool bilinear)
    {
        int width = heights.GetLength(0);
        int height = heights.GetLength(1);
        if (tex == null)
        {
            tex = new Texture2D(width, height);
            DrawTexture();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, heights[x, y]);
            }
        }

        tex.filterMode = !bilinear ? FilterMode.Point : FilterMode.Bilinear;

        tex.Apply();
    }
}
