Shader "Custom/FogOfWarOverlay"
{
    Properties
    {
        _TintColor ("Tint Color (RGB) + Base Alpha (A)", Color) = (0,0,0,0.92)
        _LightSoftness ("Edge Softness", Float) = 3.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+10" }
        LOD 100

        Pass
        {
            Name "FOR_OVERLAY"
            Tags { "LightMode"="UniversalForward" }

            ZWrite Off
            ZTest Always
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // === ПАРАМЕТРЫ ===
            CBUFFER_START(UnityPerMaterial)
                float4 _TintColor;
                float  _LightSoftness;
                int    _LightCount;
            CBUFFER_END

            // Максимум источников света (можешь уменьшить для мобилок)
            #define MAX_LIGHTS 16
            float4 _LightPositions[MAX_LIGHTS];   // xyz = world pos (XZ важны)
            float  _LightRadii[MAX_LIGHTS];       // радиус каждого света

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0; // мировая позиция фрагмента на плоскости
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                float4 pos_ws = float4(TransformObjectToWorld(IN.positionOS), 1.0);
                OUT.positionWS = pos_ws.xyz;
                OUT.positionHCS = TransformWorldToHClip(pos_ws.xyz);
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                // Мировая позиция пикселя на «простыне»
                float2 p = float2(IN.positionWS.x, IN.positionWS.z);

                // Расчёт видимости как max влияния всех источников
                // visibility = 0 (полная тьма) ... 1 (полная «дырка»)
                float visibility = 0.0;

                [unroll]
                for (int i = 0; i < _LightCount && i < MAX_LIGHTS; i++)
                {
                    float2 l = float2(_LightPositions[i].x, _LightPositions[i].z);
                    float  r = _LightRadii[i];
                    float  s = _LightSoftness; // ширина мягкой кромки

                    // расстояние по XZ
                    float d = distance(p, l);

                    // smoothstep от внешнего к внутреннему радиусу:
                    // при d >= r -> 0; при d <= (r - s) -> 1
                    float inner = max(r - s, 0.0001);
                    float v = saturate(smoothstep(r, inner, d));
                    visibility = max(visibility, v);
                }

                // Итог: тёмный цвет с альфой, «протравленной» видимостью
                float alpha = _TintColor.a * (1.0 - visibility);
                return float4(_TintColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
