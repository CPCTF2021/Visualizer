Shader "Unlit/PixelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaxSegment("MaxSegmentPow(2^x)", Int) = 10
        _MinSegment("MinSegmentPow(2^x)", Int) = 5
        _Progress("Progress", Range(0, 1)) = 0
        _ColorStep("ColorStep", Int) = 10
        _EdgeThreshold("EdgeThreshold", Range(0, 0.05)) = 0.5
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _CameraDepthTexture;

            int _MaxSegment;
            int _MinSegment;
            int _ColorStep;
            float _Progress;
            float _EdgeThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int segment = pow(2.0, floor(lerp(_MinSegment, _MaxSegment, _Progress)));
                float2 uv = floor(i.uv * segment) / segment + float2(1.0, 1.0) / segment * 0.5;
                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                float colorStep = lerp(_ColorStep, 16.0, _Progress);
                float lumin = 0.298912 * col.r + 0.586611 * col.g + 0.114478 * col.b;
                float luminStepped = floor(lumin * colorStep) / (colorStep - 1);
                // luminStepped = luminStepped * 0.8 + 0.2;
                col = col / lumin * luminStepped;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
