
#ifndef INCLUDE_TERRAIN_TOBO
#define INCLUDE_TERRAIN_TOBO

#include "FastNoiseLite.hlsl"

void Warp(fnl_state noise, inout float x, inout float y)
{
    float qX = x;
    float qY = y;

    fnlDomainWarp2D(noise, qX, qY);

    float rX = x + 4 * qX + 1.7f;
    float rY = y + 4 * qY + 9.2f;

    fnlDomainWarp2D(noise, rX, rY);

    x += 4 * rX;
    y += 4 * rY;
}

void TerrainShading_float(float2 uv, int seed, float freq, int octaves, float laq, float gain, float warmAmp, out float value)
{
    fnl_state state = fnlCreateState(seed);
    //prec.frequency = (_PFreq * (_Freq / 10)) / 10000;
    state.frequency = freq / 10000.0f;
    state.noise_type = FNL_NOISE_OPENSIMPLEX2;
    state.rotation_type_3d = FNL_ROTATION_NONE;

    state.fractal_type = FNL_FRACTAL_DOMAIN_WARP_INDEPENDENT;
    state.octaves = octaves;
    //state.lacunarity = _PLaq * (_Laq / 10);
    state.lacunarity = laq;
    //state.gain = _PGain * (_Gain / 10);
    state.gain = gain;

    state.cellular_distance_func = FNL_CELLULAR_DISTANCE_HYBRID;
    state.cellular_return_type = FNL_CELLULAR_RETURN_TYPE_DISTANCE;

    state.domain_warp_amp = warmAmp;
    
    float x = uv.x;
    float y = uv.y;

    Warp(state, x, y);

    value = (fnlGetNoise2D(state, x, y) + 1.0f) / 2.0f;
}

#endif