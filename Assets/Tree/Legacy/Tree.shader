﻿Shader "Unlit/Tree"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // _Radius ("Radius", Float) = 0.1
        // _Progress ("Progress", Range(0, 1)) = 0
        _MainColor("MainColor", Color) = (0.0, 0.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;

            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
                UNITY_DEFINE_INSTANCED_PROP(float, _Radius)
                UNITY_DEFINE_INSTANCED_PROP(float, _Progress)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex - float3(v.uv2.y, v.uv3.x, v.uv3.y) * UNITY_ACCESS_INSTANCED_PROP(Props, _Radius) * (1.0 - max(min(v.uv2.x - (1 - UNITY_ACCESS_INSTANCED_PROP(Props, _Progress)), 1.0), 0.0)));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _MainColor * tex2D(_MainTex, i.uv) * dot(i.normal, float3( 0.7071, 0.7071, 0.0));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
