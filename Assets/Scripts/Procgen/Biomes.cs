using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biomes
{
    public static readonly Biome Tundra = new DefaultBiome(MapColours.Tundra);
    public static readonly Biome Grassland = new DefaultBiome(MapColours.Grasslands);
    public static readonly Biome Desert = new DefaultBiome(MapColours.Desert);

    public static readonly Biome BareForest = new DefaultBiome(MapColours.BareForest);
    public static readonly Biome Drylands = new DefaultBiome(MapColours.Drylands);
    public static readonly Biome Savanna = new DefaultBiome(MapColours.Savanna);

    public static readonly Biome ModerateForest = new DefaultBiome(MapColours.ModerateForest);
    public static readonly Biome LushForest = new DefaultBiome(MapColours.LushForest);
    public static readonly Biome Jungle = new DefaultBiome(MapColours.Jungle);
}

public class DefaultBiome : Biome
{
    public DefaultBiome(Color mapColour) : base(mapColour) { }

    public override float Sample(int x, int y)
    {
        throw new System.NotImplementedException();
    }
}
