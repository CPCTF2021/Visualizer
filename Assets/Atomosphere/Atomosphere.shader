Shader "Custom/Atomosphere"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _SpectreTex ("Spectre", 2D) = "white" {}
    _F("F", Float) = 0.5
  }
  SubShader
  {
    // No culling or depth
    Cull Off ZWrite Off ZTest Always

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "AtomosphereFunc.cginc"

      struct appdata
      {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f
      {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
        float3 viewVector: TEXCOORD1;
      };

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        float3 viewVector = mul(unity_CameraInvProjection, float4(v.uv.xy * 2 - 1, 0, -1));
				o.viewVector = mul(unity_CameraToWorld, float4(viewVector,0));
        return o;
      }

      sampler2D _MainTex;
      sampler2D _CameraDepthTexture;
      sampler2D _Spectre;
      float3 _Center;
      float _AtomosphereRadius;
      float _PlanetRadius;
      float _F;

      static const float ATOMO_STEP = 50;
      static const float SUNLIGHT_STEP = 10;

      float densityMap(float3 pos)
      {
        return max(0.0, 1.0 - distance(pos, _Center) / _AtomosphereRadius);
      }

      float4 getSpectre(float value)
      {
        return tex2D(_Spectre, float2(saturate(value) * 0.9 + 0.05, 0.5));
      }

      float4 calculateEmittance(float3 origin, float3 sunDir)
      {
        float2 dPlanet = raySphere(_PlanetRadius, origin, sunDir);
        // if(dPlanet.y > 0) return float4(0.0, 0.0, 0.0, 1.0);
        float length = raySphere(_AtomosphereRadius, origin, sunDir).y;

        float delta = length / (float)(ATOMO_STEP);
        float3 pos = origin;
        float a = 0.0;

        for(int i=0;i<SUNLIGHT_STEP;i++)
        {
          pos += delta * sunDir;
          a += densityMap(pos) * delta / _AtomosphereRadius / 2.0;
        }
        
        return lerp(getSpectre(a * _F), float4(0.0, 0.0, 0.0, 1.0), saturate(dPlanet.y * 0.2));
      }

      float4 cloudColor(float3 origin, float3 dir, float length, float4 base)
      {
        float a = 0.0;
        float delta = length / (float)(ATOMO_STEP);
        float normalizeDelta = delta / _AtomosphereRadius / 2.0;
        float3 pos = origin;
        float3 sunDir = _WorldSpaceLightPos0.xyz;

        float4 originalLight = float4(1.0, 0.5, 0.8, 1.0);
        float4 light = float4(0.0, 0.0, 0.0, 0.0);

        for(int i=0;i<ATOMO_STEP;i++)
        {
          pos += delta * dir;
          a += densityMap(pos) * normalizeDelta;
          light += calculateEmittance(pos, sunDir) * exp(-(i) * normalizeDelta - 0.5);
        }
        return lerp(base, light, saturate(a * 0.4));
      }

      fixed4 frag (v2f i) : SV_Target
      {
        float3 rayDir = normalize(i.viewVector);
        float3 offset = _WorldSpaceCameraPos.xyz - _Center;
        float2 d = raySphere(_AtomosphereRadius * 2.0, offset, rayDir);
        float sceneDepthNonLinear = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
				float sceneDepth = LinearEyeDepth(sceneDepthNonLinear) * length(i.viewVector);
        float dist = min(d.x + d.y, sceneDepth) - d.x;

        float3 surfacePos = offset + rayDir * d.x;
        float4 baseColor = tex2D(_MainTex, i.uv);
        float4 color = cloudColor(surfacePos, rayDir, dist, baseColor);
        // float dist = d.y;
        // dist = exp(-dist);
        // float4 col = float4(dist, dist, dist, 1.0);
        return lerp(baseColor, color, step(0.00001, d.y));
        // return col;
        // return float4(surfacePos, 1.0);
      }
      ENDCG
    }
  }
}
