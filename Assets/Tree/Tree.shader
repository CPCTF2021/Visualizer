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
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard vertex:vert fullforwardshadows
        #pragma instancing_options procedural:setup
        #pragma multi_compile_fog
        #pragma multi_compile_instancing
        #pragma target 2.0

        struct Input
        {
            float2 uv_MainTex;
        };

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        float4 _MainColor;

        UNITY_INSTANCING_BUFFER_START(Props)

            UNITY_DEFINE_INSTANCED_PROP(float, _Radius)
            UNITY_DEFINE_INSTANCED_PROP(float, _Progress)

        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v)
        {
            UNITY_SETUP_INSTANCE_ID(v);
            v.vertex.xyz = (v.vertex - float4(v.texcoord1.y, v.texcoord2.x, v.texcoord2.y, 0.0) * UNITY_ACCESS_INSTANCED_PROP(Props, _Radius) * (1.0 - max(min(v.texcoord1.x - (1.0 - UNITY_ACCESS_INSTANCED_PROP(Props, _Progress)), 1.0), 0.0))).xyz;
            // v.vertex = UnityObjectToClipPos(v.vertex);
            v.normal = UnityObjectToWorldNormal(v.normal);
        }

        void setup ()
        {
            
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
            o.Albedo = c.rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.0;
            o.Alpha = c.a;
        }
        ENDCG
    }
}
