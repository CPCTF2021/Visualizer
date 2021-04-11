// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'
// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.

Shader "Unlit/BillBoard"
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

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;
            
            UNITY_INSTANCING_BUFFER_START (MyProperties)
                UNITY_DEFINE_INSTANCED_PROP (float4, _Color)
            UNITY_INSTANCING_BUFFER_END(MyProperties)

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID (v);
                UNITY_TRANSFER_INSTANCE_ID (v, o);

                float3 vpos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
				float4 worldCoord = float4(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23, 1);
				float4 viewPos = mul(UNITY_MATRIX_V, worldCoord) + float4(vpos, 0);
				float4 outPos = mul(UNITY_MATRIX_P, viewPos);

				o.vertex = outPos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID (i);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor * UNITY_ACCESS_INSTANCED_PROP (MyProperties, _Color);
                clip(col.a - 0.999999);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
