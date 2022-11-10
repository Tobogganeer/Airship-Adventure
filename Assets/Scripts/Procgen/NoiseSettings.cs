using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FastNoiseLite;

[System.Serializable]
public class NoiseSettings
{
    [Space]
    public int mSeed = 1337;
    public float mFrequency = 0.01f;
    public NoiseType mNoiseType = NoiseType.OpenSimplex2;
    public RotationType3D mRotationType3D = RotationType3D.None;

    [Space]
    public FractalType mFractalType = FractalType.None;
    public int mOctaves = 3;
    public float mLacunarity = 2.0f;
    public float mGain = 0.5f;
    public float mWeightedStrength = 0.0f;
    public float mPingPongStrength = 2.0f;

    [Space]
    public CellularDistanceFunction mCellularDistanceFunction = CellularDistanceFunction.EuclideanSq;
    public CellularReturnType mCellularReturnType = CellularReturnType.Distance;
    public float mCellularJitterModifier = 1.0f;

    [Space]
    public DomainWarpType mDomainWarpType = DomainWarpType.OpenSimplex2;
    public float mDomainWarpAmp = 1.0f;

    public FastNoiseLite Get()
    {
        FastNoiseLite noise = new FastNoiseLite(mSeed);

        noise.SetFrequency(mFrequency);
        noise.SetNoiseType(mNoiseType);
        noise.SetRotationType3D(mRotationType3D);

        noise.SetFractalType(mFractalType);
        noise.SetFractalOctaves(mOctaves);
        noise.SetFractalLacunarity(mLacunarity);
        noise.SetFractalGain(mGain);
        noise.SetFractalWeightedStrength(mWeightedStrength);
        noise.SetFractalPingPongStrength(mPingPongStrength);

        noise.SetCellularDistanceFunction(mCellularDistanceFunction);
        noise.SetCellularReturnType(mCellularReturnType);
        noise.SetCellularJitter(mCellularJitterModifier);

        noise.SetDomainWarpType(mDomainWarpType);
        noise.SetDomainWarpAmp(mDomainWarpAmp);

        return noise;
    }
}
