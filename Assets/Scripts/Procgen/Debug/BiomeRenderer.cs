using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeRenderer : MonoBehaviour
{
    public Color temperatureTint;
    public Color precipitationTint;

    public FlatRenderer tempRenderer;
    public FlatRenderer precRenderer;
    public FlatRenderer combinedRendererMult;
    public FlatRenderer combinedRendererAddition;
    public FlatRenderer combinedRendererMultHeightmap;
    public FlatRenderer combinedRendererAdditionHeightmap;
    public FlatRenderer biomeRenderer;
    public FlatRenderer blendRendererMult;
    public FlatRenderer blendRendererAddition;
    public Gradient tempGradient;
    public Gradient precGradient;
    public Gradient heightmap;

    private const int SIZE = 128;
    //private const int SIZE = 512;

    public float scale = 3;

    public float lower = -1;
    public float upper = 1;

    public bool bilinear = true;

    float[,] noise = new float[SIZE, SIZE];
    Color[,] colours = new Color[SIZE, SIZE];
    float[,] temps = new float[SIZE, SIZE];
    float[,] precs = new float[SIZE, SIZE];

    private void Start()
    {
        tempRenderer.high *= temperatureTint;
        precRenderer.high *= precipitationTint;

        Render();
    }

    //private void OnValidate()
    //{
    //    if (!Application.isPlaying) return;
    //
    //    Render();
    //}

    [ContextMenu("Render")]
    private void Render()
    {
        Biome.GenerateNoiseProfiles();

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                temps[x, y] = Remap.Float(Biome.GetTemperatureAt((int)(x * scale), (int)(y * scale)), lower, upper, 0, 1);
                precs[x, y] = Remap.Float(Biome.GetPrecipitationAt((int)(x * scale), (int)(y * scale)), lower, upper, 0, 1);
            }
        }

        tempRenderer.Draw(temps, bilinear);
        precRenderer.Draw(precs, bilinear);

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                noise[x, y] = temps[x, y] * precs[x, y];
                colours[x, y] = heightmap.Evaluate(noise[x, y]);
            }
        }

        combinedRendererMult.Draw(noise, bilinear);
        combinedRendererMultHeightmap.Draw(colours, bilinear);
        MeshGenerator.Create(SIZE, noise, colours);

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                noise[x, y] = (temps[x, y] + precs[x, y]) / 2;
                colours[x, y] = heightmap.Evaluate(noise[x, y]);
            }
        }

        combinedRendererAddition.Draw(noise, bilinear);
        combinedRendererAdditionHeightmap.Draw(colours, bilinear);
        //MeshGenerator.Create(SIZE, noise, colours);

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                Biome biome = Biome.GetBiomeFrom(temps[x, y], precs[x, y]);
                colours[x, y] = biome.mapColour;
            }
        }

        biomeRenderer.Draw(colours, bilinear);

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                colours[x, y] = tempGradient.Evaluate(temps[x, y]) * precGradient.Evaluate(precs[x, y]);
            }
        }

        blendRendererMult.Draw(colours, bilinear);

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                colours[x, y] = (tempGradient.Evaluate(temps[x, y]) + precGradient.Evaluate(precs[x, y])) / 2;
            }
        }

        blendRendererAddition.Draw(colours, bilinear);
    }
}
