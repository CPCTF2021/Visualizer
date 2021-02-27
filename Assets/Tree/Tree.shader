Shader "Unlit/Tree"
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
            #pragma multi_compile_instancing // 追加

            #if defined(UNITY_SUPPORT_INSTANCING) && defined(INSTANCING_ON)
                #define UNITY_INSTANCING_ENABLED
            #endif

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float3 normal: NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
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
            // float _Radius;
            // float _Progress;
            float4 _MainColor;

            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
                UNITY_DEFINE_INSTANCED_PROP(float, _Radius)
                UNITY_DEFINE_INSTANCED_PROP(float, _Progress)

            UNITY_INSTANCING_BUFFER_END(Props)
            
            #if defined(UNITY_INSTANCING_ENABLED)
                #define UNITY_GET_INSTANCE_ID(input) input.instanceID
                #define UNITY_TRANSFER_INSTANCE_ID(input, output) output.instanceID = UNITY_GET_INSTANCE_ID(input)
            #else
                #define UNITY_TRANSFER_INSTANCE_ID(input, output)
            #endif

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
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
