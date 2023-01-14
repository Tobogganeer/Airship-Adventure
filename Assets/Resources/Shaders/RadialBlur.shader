Shader "Hidden/RadialBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurWidth("Blur Width", Range(0,1)) = 0.85
        _Intensity("Intensity", Range(0,1)) = 1
        _Center("Center", Vector) = (0.5,0.5,0,0)
        //_FalloffPow("Falloff Power", Range(0.5, 4)) = 2
        //_FalloffMult("Falloff Mult", Range(0.5, 4)) = 1
    }
    SubShader
    {
        // No culling or depth
        Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            #define NUM_SAMPLES 100

            float _BlurWidth;
            float _Intensity;
            float4 _Center;
            //float _FalloffPow;
            //float _FalloffMult;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = fixed4(0.0f, 0.0f, 0.0f, 1.0f);

                float2 ray = i.uv - _Center.xy;

                float dist = distance(i.uv, _Center.xy);
                dist = clamp(dist, 0.0f, 1.0f);

                //float val = dist;
                //float val = _Dot;
                //float val = clamp(_Center.z, 0.0f, 1.0f);
                float val = max(clamp(_Center.z * 2.0f, 0.0f, 1.0f), dist);
                val = 1.0f - val;
                val = clamp(pow(val, 2.0f), 0.0f, 1.0f);
                //val = clamp(pow(val * _FalloffMult, _FalloffPow), 0.0f, 1.0f);
                //val = _Dot;
                //return lerp(fixed4(0.0f, 1.0f, 0.0f, 1.0f), fixed4(1.0f, 0.0f, 0.0f, 1.0f), val);
                //return _Dot;
                //return fixed4(val, val, val, 1.0f);

                for (int i = 0; i < NUM_SAMPLES; i++)
                {
                    float scale = 1.0f - _BlurWidth * (float(i) / float(NUM_SAMPLES - 1));
                    //color.xyz += _Dot * tex2D(_MainTex, (ray * scale) + _Center.xy).xyz / float(NUM_SAMPLES);
                    color.xyz += val * tex2D(_MainTex, (ray * scale) + _Center.xy).xyz / float(NUM_SAMPLES);
                }
                
                return color * _Intensity;
            }
            ENDCG
        }
    }
}
