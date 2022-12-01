using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FastNoiseLite;

[CreateAssetMenu(menuName = "Scriptable Objects/Noise Settings")]
public class NoiseSettings : ScriptableObject
{
    [Space]
    public float scale = 1;
    public float seed = 1258;
    //public Vector2 offset = Vector2.zero;
    //public float frequency = 1.4f;
    //NoiseType mNoiseType = NoiseType.OpenSimplex2;
    //RotationType3D mRotationType3D = RotationType3D.None;

    //[Space]
    //FractalType mFractalType = FractalType.None;
    //public float octaves = 7;
    //public float lacunarity = 3.85f;
    //public float gain = -0.48f;
    //public float warpAmplitute = 27f;
    //float mWeightedStrength = 0.0f;
    //float mPingPongStrength = 2.0f;

    //[Space]
    //CellularDistanceFunction mCellularDistanceFunction = CellularDistanceFunction.EuclideanSq;
    //CellularReturnType mCellularReturnType = CellularReturnType.Distance;
    //float mCellularJitterModifier = 1.0f;

    //[Space]
    //DomainWarpType mDomainWarpType = DomainWarpType.OpenSimplex2;

    /*
    public FastNoiseLite Get(ProcGen.MainSettings settings)
    {
        FastNoiseLite noise = new FastNoiseLite((int)seed);

        noise.SetFrequency(frequency * (settings.frequency / 10f));
        noise.SetNoiseType(mNoiseType);
        noise.SetRotationType3D(mRotationType3D);

        noise.SetFractalType(mFractalType);
        noise.SetFractalOctaves((int)octaves);
        noise.SetFractalLacunarity(lacunarity * (settings.lacunarity / 10f));
        noise.SetFractalGain(gain * (settings.gain / 10f));
        noise.SetFractalWeightedStrength(mWeightedStrength);
        noise.SetFractalPingPongStrength(mPingPongStrength);

        noise.SetCellularDistanceFunction(mCellularDistanceFunction);
        noise.SetCellularReturnType(mCellularReturnType);
        noise.SetCellularJitter(mCellularJitterModifier);

        noise.SetDomainWarpType(mDomainWarpType);
        noise.SetDomainWarpAmp(warpAmplitute * settings.warpMult);

        return noise;
    }
    */

    public void SetToMat(Material mat, string prefix)
    {
        string Get(string val) => $"_{prefix}{val}";

        mat.SetFloat(Get("Scale"), scale);
        mat.SetFloat(Get("Seed"), seed);
        //mat.SetVector(Get("Offset"), offset);
        //mat.SetFloat(Get("Frequency"), frequency);
        //mat.SetFloat(Get("Octaves"), octaves);
        //mat.SetFloat(Get("Lacunarity"), lacunarity);
        //mat.SetFloat(Get("Gain"), gain);
        //mat.SetFloat(Get("Warp_Amplitude"), warpAmplitute);
    }
}
