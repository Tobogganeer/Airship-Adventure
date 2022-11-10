Shader "Custom/Terrain"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        [Header(Terrain Settings)] _Scale("Scale", Float) = 50
        _Offset("Offset", Vector) = (0, 0, 0, 0)
        _LowColour("Low Colour", Color) = (0, 0, 0, 1)
        _HighColour("High Colour", Color) = (1, 1, 1, 1)

        [Header(Both)] _Freq("Freq", Float) = 10
        _Gain("Gain", Float) = 10
        _Laq("Laq", Float) = 10

        [Header(Precipitation)] _PSeed("PSeed", Integer) = 1248
        _POffset("POffset", Vector) = (0, 0, 0, 0)
        _PFreq("PFreq", Float) = 1.4
        _POctaves("POctaves", Integer) = 6
        _PLaq("PLaq", Float) = 3.4
        _PGain("PGain", Float) = 0.48
        //_PStr("PStr", Float) = 0.6
        _PWarpAmp("PWarpAmp", Float) = 3700

        [Header(Temperature)] _TSeed("TSeed", Integer) = 5052
        _TOffset("TOffset", Vector) = (0, 0, 0, 0)
        _TFreq("TFreq", Float) = 1.8
        _TOctaves("TOctaves", Integer) = 7
        _TLaq("TLaq", Float) = 2.2
        _TGain("TGain", Float) = 0.58
        //_TStr("TStr", Float) = 0.2
        _TWarpAmp("TWarpAmp", Float) = 5000
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "FastNoiseLite.hlsl"

            float _Scale;
            float4 _Offset;
            half4 _LowColour;
            half4 _HighColour;

            float _Freq;
            float _Gain;
            float _Laq;

            int _PSeed;
            float4 _POffset;
            float _PFreq;
            int _POctaves;
            float _PLaq;
            float _PGain;
            //float _PStr;
            float _PWarpAmp;

            int _TSeed;
            float4 _TOffset;
            float _TFreq;
            int _TOctaves;
            float _TLaq;
            float _TGain;
            //float _TStr;
            float _TWarpAmp;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            void Warp(fnl_state noise, inout float x, inout float y)
            {
                float qX = x;
                float qY = y;

                fnlDomainWarp2D(noise, qX, qY);
                //float idkNotNeeded = fnlGetNoise2D(noise, x, y);

                float rX = x + 4 * qX + 1.7f;
                float rY = y + 4 * qY + 9.2f;

                fnlDomainWarp2D(noise, rX, rY);

                x += 4 * rX;
                y += 4 * rY;

                /*
                float qX = x;
                float qY = y;

                noise.DomainWarp(ref qX, ref qY);


                float rX = x + 4 * qX + 1.7f;
                float rY = y + 4 * qY + 9.2f;

                noise.DomainWarp(ref rX, ref rY);

                x += 4 * rX;
                y += 4 * rY;
                */
            }

            //fnl_state temp = fnlCreateState();


            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
         
            half4 frag(Varyings IN) : SV_Target
            {
                // Prec
                fnl_state prec = fnlCreateState();
                prec.seed = _PSeed;
                prec.frequency = (_PFreq * (_Freq / 10)) / 10000;
                prec.noise_type = FNL_NOISE_OPENSIMPLEX2;
                prec.rotation_type_3d = FNL_ROTATION_NONE;

                prec.fractal_type = FNL_FRACTAL_DOMAIN_WARP_INDEPENDENT;
                prec.octaves = _POctaves;
                prec.lacunarity = _PLaq * (_Laq / 10);
                prec.gain = _PGain * (_Gain / 10);
                //prec.weighted_strength = _PStr;

                prec.cellular_distance_func = FNL_CELLULAR_DISTANCE_HYBRID;
                prec.cellular_return_type = FNL_CELLULAR_RETURN_TYPE_DISTANCE;

                prec.domain_warp_amp = _PWarpAmp;

                // Temp
                fnl_state temp = fnlCreateState();
                temp.seed = _TSeed;
                temp.frequency = (_TFreq * (_Freq / 10)) / 10000;
                temp.noise_type = FNL_NOISE_OPENSIMPLEX2;
                temp.rotation_type_3d = FNL_ROTATION_NONE;

                temp.fractal_type = FNL_FRACTAL_DOMAIN_WARP_INDEPENDENT;
                temp.octaves = _TOctaves;
                temp.lacunarity = _TLaq * (_Laq / 10);
                temp.gain = _TGain * (_Gain / 10);
                //temp.weighted_strength = _TStr;

                temp.cellular_distance_func = FNL_CELLULAR_DISTANCE_HYBRID;
                temp.cellular_return_type = FNL_CELLULAR_RETURN_TYPE_DISTANCE;

                temp.domain_warp_amp = _TWarpAmp;

                float precX = (IN.uv.x + _Offset.x + _POffset.x) * _Scale;
                float precY = (IN.uv.y + _Offset.y + _POffset.y) * _Scale;
                float tempX = (IN.uv.x + _Offset.x + _TOffset.x) * _Scale;
                float tempY = (IN.uv.y + _Offset.y + _TOffset.y) * _Scale;

                Warp(prec, precX, precY);
                Warp(temp, tempX, tempY);

                float precNoise = (fnlGetNoise2D(prec, precX, precY) + 1) / 2;
                float tempNoise = (fnlGetNoise2D(temp, tempX, tempY) + 1) / 2;

                half4 col;
                col = lerp(_LowColour, _HighColour, (precNoise + tempNoise) / 2);
                //customColor = half4(0.5, 0, 0, 1);
                return col;
            }
            ENDHLSL
        }
    }
}
