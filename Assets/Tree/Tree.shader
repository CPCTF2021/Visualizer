Shader "Unlit/Tree"
{
    Properties
    {
        // _Radius ("Radius", Float) = 0.1
        // _Progress ("Progress", Range(0, 1)) = 0
        _MainColor("MainColor", Color) = (0.0, 0.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass {
            Tags { "LightMode"="ShadowCaster" "RenderType"="Opaque" "Queue"="Geometry" }
  		    ZWrite On
  		    ColorMask 0

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct appdata 
            {
                float4 vertex : POSITION;
                float4 texcoord0: TEXCOORD0;
                float4 texcoord1: TEXCOORD1;
                float4 texcoord2: TEXCOORD2;
                float4 texcoord3: TEXCOORD3;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            #include "UnityCG.cginc"

            UNITY_INSTANCING_BUFFER_START(Props)

                UNITY_DEFINE_INSTANCED_PROP(float, _Radius)
                UNITY_DEFINE_INSTANCED_PROP(float, _Progress)

            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(float4((v.vertex - float4(v.texcoord1.y, v.texcoord2.x, v.texcoord2.y, 0.0) * UNITY_ACCESS_INSTANCED_PROP(Props, _Radius) * (1.0 - max(min(v.texcoord1.x - (1.0 - UNITY_ACCESS_INSTANCED_PROP(Props, _Progress)), 1.0), 0.0))).xyz, 1.0));
                return o;
            }

            fixed4 frag(v2f o):COLOR
            {
                return float4(1.0, 1.0, 1.0, 1.0);
            }

            ENDCG
		}
        
        Pass {
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
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 shade : TEXCOORD0;
            };


            float4 _MainColor;

            UNITY_INSTANCING_BUFFER_START(Props)

                UNITY_DEFINE_INSTANCED_PROP(float, _Radius)
                UNITY_DEFINE_INSTANCED_PROP(float, _Progress)

            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v)
                o.vertex = (v.vertex - float4(v.texcoord1.y, v.texcoord2.x, v.texcoord2.y, 0.0) * UNITY_ACCESS_INSTANCED_PROP(Props, _Radius) * (1.0 - max(min(v.texcoord1.x - (1.0 - UNITY_ACCESS_INSTANCED_PROP(Props, _Progress)), 1.0), 0.0)));
                o.vertex = UnityObjectToClipPos(float4(o.vertex.x, o.vertex.y, o.vertex.z, 1.0));
                // v.vertex = UnityObjectToClipPos(v.vertex);
                // v.vertex = v.vertex;
                o.shade = (saturate(dot(UnityObjectToWorldNormal(v.normal).xyz, _WorldSpaceLightPos0.xyz)) * 0.6 + 0.4) * _MainColor;

                return o;
            };

            fixed4 frag (v2f v): SV_Target
            {
                return v.shade;
            };
            ENDCG
        }
    }
}
