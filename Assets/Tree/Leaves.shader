Shader "Custom/Leaves"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass {
            Tags { "LightMode"="ShadowCaster" "RenderType"="Opaque" "Queue"="Geometry" }
  		    ZWrite On
  		    ColorMask 0

            // CGPROGRAM

            // #pragma vertex vert
            // #pragma fragment frag

            // struct appdata 
            // {
            //     float4 vertex : POSITION;
            // };

            // struct v2f
            // {
            //     float4 vertex : SV_POSITION;
            // };

            // #include "UnityCG.cginc"

            // UNITY_INSTANCING_BUFFER_START(Props)

            //     UNITY_DEFINE_INSTANCED_PROP(float, _Radius)
            //     UNITY_DEFINE_INSTANCED_PROP(float, _Progress)

            // UNITY_INSTANCING_BUFFER_END(Props)

            // v2f vert(appdata v) {
            //     v2f o;
            //     o.vertex = UnityObjectToClipPos(v.vertex);
            //     return o;
            // }

            // fixed4 frag(v2f o):COLOR
            // {
            //     return float4(1.0, 1.0, 1.0, 1.0);
            // }

            // ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 shade : TEXCOORD0;
            };

            float4 _MainColor;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v)
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.shade = (saturate(dot(UnityObjectToWorldNormal(v.normal).xyz, _WorldSpaceLightPos0.xyz)) * 0.6 + 0.4) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);

                return o;
            };

            fixed4 frag (v2f v): SV_Target
            {
                return v.shade;
            };
            ENDCG
        }
    }
    FallBack "Diffuse"
}
