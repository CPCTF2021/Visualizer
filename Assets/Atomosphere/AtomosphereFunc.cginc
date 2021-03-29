#include "UnityCG.cginc"
static const float MAX_FLOAT = 3.402823466e+38;


float2 raySphere(float radius, float3 camPos, float3 dir)
{
  float3 offset = camPos;
  
  float a = 1.0;
  float b = 2.0 * dot(offset, dir);
  float c = dot(offset, offset) - radius * radius;
  float d = b * b - 4 * a * c;

  if(d > 0)
  {
    float s = sqrt(d);
    float dstToSphereNear = max(0, (-b - s) / (2 * a));
    float dstToSphereFar = (-b + s) / (2 * a);

    if (dstToSphereFar >= 0) {
      return float2(dstToSphereNear, dstToSphereFar - dstToSphereNear);
    }
  }

  return float2(MAX_FLOAT, 0);
}
