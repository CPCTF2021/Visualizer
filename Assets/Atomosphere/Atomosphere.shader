Shader "Custom/Atomosphere"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _BlueNoise ("BlueNoise", 2D) = "white" {}
    densityFalloff("densityFalloff", Float) = 10.0
    ditherScale("ditherScale", Float) = 0.5
    ditherStrength("ditherStrength", Float) = 0.1
    scatteringCoefficients("scatteringCoefficients", Vector) = (0.1, 0.1, 0.1, 1.0)
    brightnessAdaptionStrength("brightnessAdaptionStrength", Float) = 0.15
    reflectedLightOutScatterStrength("reflectedLightOutScatterStrength", Float) = 3.0
    intensity("intensity", Float) = 0.1 
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
      sampler2D _BlueNoise;
      float3 _Center;
      float _AtomosphereRadius;
      float _PlanetRadius;
      float _F;

      float densityFalloff;
      float ditherScale;
      float ditherStrength;
      float4 scatteringCoefficients;
      float brightnessAdaptionStrength = 0.15;
      float reflectedLightOutScatterStrength = 3;
      float intensity;


      static const float ATOMO_STEP = 20;
      static const float SUNLIGHT_STEP = 10;

      float2 squareUV(float2 uv) {
				float width = _ScreenParams.x;
				float height =_ScreenParams.y;
				//float minDim = min(width, height);
				float scale = 1000;
				float x = uv.x * width;
				float y = uv.y * height;
				return float2 (x/scale, y/scale);
			}

      float densityAtPoint(float3 densitySamplePoint)
      {
				float heightAboveSurface = length(densitySamplePoint) - _PlanetRadius;
				float height01 = heightAboveSurface / (_AtomosphereRadius - _PlanetRadius);
				float localDensity = exp(-height01 * densityFalloff) * (1 - height01);
				return localDensity;
			}

      float opticalDepth(float3 rayOrigin, float3 rayDir)
      {
        float rayLength = raySphere(_AtomosphereRadius, rayOrigin, rayDir).y;
				float3 densitySamplePoint = rayOrigin;
				float stepSize = rayLength / (SUNLIGHT_STEP - 1.0);
				float opticalDepth = 0;

				for (int i = 0; i < SUNLIGHT_STEP; i ++) {
					float localDensity = densityAtPoint(densitySamplePoint);
					opticalDepth += localDensity * stepSize;
					densitySamplePoint += rayDir * stepSize;
				}
				return opticalDepth;
			}

      float opticalDepthBaked2(float3 rayOrigin, float3 rayDir, float rayLength) {
				float3 endPoint = rayOrigin + rayDir * rayLength;
				float d = dot(rayDir, normalize(rayOrigin));
				float opticalDepthValue = 0;

				const float blendStrength = 1.5;
				float w = saturate(d * blendStrength + .5);
				
				float d1 = opticalDepth(rayOrigin, rayDir) - opticalDepth(endPoint, rayDir);
				float d2 = opticalDepth(endPoint, -rayDir) - opticalDepth(rayOrigin, -rayDir);

				opticalDepthValue = lerp(d2, d1, w);
				return opticalDepthValue;
			}

      float3 calculateLight(float3 rayOrigin, float3 rayDir, float rayLength, float3 originalCol, float2 uv)
      {
        

        float blueNoise = tex2Dlod(_BlueNoise, float4(squareUV(uv) * ditherScale, 0.0, 0.0));
        blueNoise = (blueNoise - 0.5) * ditherStrength;

        float3 dirToSun = _WorldSpaceLightPos0;
        float3 inScatterPoint = rayOrigin;
        float stepSize = rayLength / (ATOMO_STEP - 1);
        float3 inScatteredLight = 0;
        float viewRayOpticalDepth = 0;

        for(int i=0;i<ATOMO_STEP;i++)
        {
          float sunRayLength = raySphere(_AtomosphereRadius, inScatterPoint, dirToSun).y;
          float sunRayOpticalDepth = opticalDepth(inScatterPoint + dirToSun * ditherStrength, dirToSun);
          float localDensity = densityAtPoint(inScatterPoint);
					viewRayOpticalDepth = opticalDepthBaked2(rayOrigin, rayDir, stepSize * i);
					float3 transmittance = exp(-(sunRayOpticalDepth + viewRayOpticalDepth) * scatteringCoefficients);
          
          inScatterPoint += localDensity * transmittance;
          inScatterPoint += rayDir * stepSize;
        }
				inScatteredLight *= scatteringCoefficients * intensity * stepSize / _PlanetRadius;
				inScatteredLight += blueNoise * 0.01;

				float brightnessAdaption = dot (inScatteredLight,1) * brightnessAdaptionStrength;
				float brightnessSum = viewRayOpticalDepth * intensity * reflectedLightOutScatterStrength + brightnessAdaption;
				float reflectedLightStrength = exp(-brightnessSum);
				float hdrStrength = saturate(dot(originalCol,1)/3-1);
				reflectedLightStrength = lerp(reflectedLightStrength, 1, hdrStrength);
				float3 reflectedLight = originalCol * reflectedLightStrength;

				float3 finalCol = reflectedLight + inScatteredLight;
				
				return finalCol;
      }

      fixed4 frag (v2f i) : SV_Target
      {
        float4 originalCol = tex2D(_MainTex, i.uv);
        float sceneDepthNonLinear = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
				float sceneDepth = LinearEyeDepth(sceneDepthNonLinear) * length(i.viewVector);
        
        float3 rayOrigin = _WorldSpaceCameraPos;
        float3 rayDir = normalize(i.viewVector);
        float3 offset = rayOrigin - _Center;

        float dstToPlanet = raySphere(_PlanetRadius, offset, rayDir).x;
        float dstToSurface = min(sceneDepth, dstToPlanet);

        float2 hitInfo = raySphere(_AtomosphereRadius, offset, rayDir);
        float dstToAtomosphere = hitInfo.x;
        float dstThroughAtomosphere = min(hitInfo.y, dstToSurface - dstToAtomosphere);

        if(dstThroughAtomosphere > 0)
        {
          const float epsilon = 0.0001;
          float3 pointInAtomosphere = offset + rayDir * (dstToAtomosphere + epsilon);
          float3 light = calculateLight(pointInAtomosphere, rayDir, dstThroughAtomosphere - epsilon * 2.0, originalCol, i.uv);
          return float4(light, 1.0);
        }
        return originalCol;
      }
      ENDCG
    }
  }
}
