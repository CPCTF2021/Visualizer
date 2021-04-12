Shader "Custom/VertexColor" {
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#pragma target 3.0

		struct Input {
			float4 vertColor;
			fixed4 normal;
		};

		void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertColor = v.color;
			o.normal = v.normal;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.vertColor.rgb;
			o.Normal = IN.normal;
			o.Emission = fixed3(0.0, 0.0, 0.0);
			o.Specular = 0.0;
			o.Gloss = 0.0;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}