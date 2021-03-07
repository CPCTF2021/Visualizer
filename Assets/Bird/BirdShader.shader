Shader "Custom/BirdShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
        #pragma instancing_options procedural:setup

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        struct Bird {
            float3 position;
            float3 velocity;
        };

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            StructuredBuffer<Bird> _BirdDataBuffer;
        #endif

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float3 _BirdScale;

        float4x4 eulerAnglesToRotationMatrix(float3 angles)
		{
			float ch = cos(angles.y); float sh = sin(angles.y); // heading
			float ca = cos(angles.z); float sa = sin(angles.z); // attitude
			float cb = cos(angles.x); float sb = sin(angles.x); // bank

			// Ry-Rx-Rz (Yaw Pitch Roll)
			return float4x4(
				ch * ca + sh * sb * sa, -ch * sa + sh * sb * ca, sh * cb, 0,
				cb * sa, cb * ca, -sb, 0,
				-sh * ca + ch * sb * sa, sh * sa + ch * sb * ca, ch * cb, 0,
				0, 0, 0, 1
			);
		}

        void vert(inout appdata_full v) {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                Bird bird = _BirdDataBuffer[unity_InstanceID];
                float3 pos = bird.position;
                float3 scale = _BirdScale;
                float3 vel = bird.velocity;

                float4x4 object2worldMat = (float4x4)0.0;
                object2worldMat._11_22_33_44 = float4(scale, 1.0);

                float rotY = atan2(vel.x, vel.z);
                float rotX = -asin(vel.y / (length(vel) + 1e-8));
                float4x4 rotMatrix = eulerAnglesToRotationMatrix(float3(rotX, rotY, 0));

                object2worldMat = mul(rotMatrix, object2worldMat);
                object2worldMat._14_24_34 += pos;


                v.vertex = mul(object2worldMat, v.vertex);
                v.normal = normalize(mul(object2worldMat, v.normal));
            #endif
        }

        void setup() {
            
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
