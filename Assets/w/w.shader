// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'
// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.

Shader "Unlit/BillBoardXZ"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("MainColor", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #define PI 3.1415926535

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float random (in float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise (in float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);

                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, u.x)
                        + (c - a) * u.y * (1.0 - u.x)
                        + (d - b) * u.x * u.y;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                float4 origin = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));

                float3 norm = UnityObjectToWorldNormal(v.normal);
                float3 blendWeight = pow(abs(norm), 3.0);
                blendWeight /= dot(blendWeight, 1);

                float value = 0.0;
                value += noise(norm.yz + float2(10.0, _Time.y) * sign(norm.x)) * blendWeight.x;
                value += noise(norm.zx + float2(20.0, _Time.y) * sign(norm.y)) * blendWeight.y;
                value += noise(norm.xy + float2(30.0, _Time.y) * sign(norm.z)) * blendWeight.z;

                o.vertex = UnityObjectToClipPos(v.vertex + float4(cos(value * PI * 2.0), sin(value * PI * 2.0), 0.0, 0.0) * v.vertex.z * v.vertex.z * 10.0);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex) * 0.99;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
