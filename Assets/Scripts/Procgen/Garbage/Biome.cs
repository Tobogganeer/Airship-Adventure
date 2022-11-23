using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Biome
{
    private static bool initialized = false;

    public readonly Color mapColour;


    #region Biome Map Settings

    private static FastNoiseLite tempNoise;
    private static FastNoiseLite precNoise;

    private static Biome[,] biomeLookups;
    private const int BIOME_LOOKUP_DEPTH = 5;

    #endregion

    public Biome(Color mapColour)
    {
        this.mapColour = mapColour;
    }


    public abstract float Sample(int x, int y);


    private static void InitializeBiomes()
    {
        if (initialized) return;

        initialized = true;
        GenerateNoiseProfiles();

        // Add biomes to dict here

        GenerateBiomeMap();
    }

    private static void GenerateBiomeMap()
    {
        // biomeLookups[temp, prec]

        biomeLookups = new Biome[BIOME_LOOKUP_DEPTH, BIOME_LOOKUP_DEPTH]
        {
            // Wet/Cold                                         Wet/Hot
            { Biomes.ModerateForest, Biomes.ModerateForest, Biomes.LushForest, Biomes.LushForest, Biomes.Jungle },
            { Biomes.BareForest, Biomes.ModerateForest, Biomes.ModerateForest, Biomes.LushForest, Biomes.Savanna },
            { Biomes.BareForest, Biomes.ModerateForest, Biomes.ModerateForest, Biomes.Drylands, Biomes.Savanna },
            { Biomes.Tundra, Biomes.BareForest, Biomes.Grassland, Biomes.Drylands, Biomes.Savanna },
            { Biomes.Tundra, Biomes.Grassland, Biomes.Grassland, Biomes.Desert, Biomes.Desert },
            // Dry/Cold                                         Dry/Hot

            // VERTICAL: Wet
            // HORIZONTAL: Heat
        };
    }


    public static void GenerateNoiseProfiles()
    {
        tempNoise = Noises.Temperature.Get();
        precNoise = Noises.Precipitation.Get();
    }

    public static Biome GetBiomeAt(int x, int y)
    {
        if (!initialized) InitializeBiomes();

        float temp = GetTemperatureAt(x, y);
        float prec = GetPrecipitationAt(x, y);

        float temp01 = Remap.Float01(temp, -1, 1);
        float prec01 = Remap.Float01(prec, -1, 1);

        return GetBiomeFrom(temp01, prec01);
    }

    public static Biome GetBiomeFrom(float temp01, float prec01)
    {
        int tempIndex = Mathf.RoundToInt(temp01 * (BIOME_LOOKUP_DEPTH - 1));
        int precIndex = Mathf.RoundToInt(prec01 * (BIOME_LOOKUP_DEPTH - 1));

        tempIndex = Mathf.Clamp(tempIndex, 0, (BIOME_LOOKUP_DEPTH - 1));
        precIndex = Mathf.Clamp(precIndex, 0, (BIOME_LOOKUP_DEPTH - 1));

        return biomeLookups[tempIndex, precIndex];
    }

    public static float GetTemperatureAt(int x, int y)
    {
        return GetNoiseAt(tempNoise, x, y);
    }

    public static float GetPrecipitationAt(int x, int y)
    {
        return GetNoiseAt(precNoise, x, y);
    }

    private static float GetNoiseAt(FastNoiseLite noise, int x, int y)
    {
        if (!initialized) InitializeBiomes();

        float fX = x, fY = y;
        //noise.DomainWarp(ref fX, ref fY);

        //DomainWarp.Warp(noise, ref fX, ref fY, Noises.WarpMode);
        DomainWarp.Warp(noise, ref fX, ref fY, DomainWarp.WarpMode.Double);

        return noise.GetNoise(fX, fY);
    }
}
