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
        
        CGPROGRAM
        #pragma surface surf Standard vertex:vert fullforwardshadows
        #pragma instancing_options procedural:setup
        #pragma multi_compile_fog
        #pragma multi_compile_instancing
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float3 normal;
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
            // v.vertex = v.vertex;
            v.normal = UnityObjectToWorldNormal(v.normal);
        }

        void setup ()
        {
            
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
            o.Normal = IN.normal;
            o.Albedo = c.rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.0;
            o.Alpha = c.a;
        }
        ENDCG
    }
}
