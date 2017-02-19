Shader "PointCloud/PointDialate"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_samples("number of samples", Range(0, 10)) = 0
		_sampleSizeX("size of the sample", Range(0, 1)) = 0
		_sampleSizeY("size of the sample", Range(0, 1)) = 0
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float _samples;
			float _sampleSizeX;
			float _sampleSizeY;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_CameraDepthTexture, i.uv);
				// just invert the colors
				
				float4 sumColor;
				int count = 0;
				for (int k = -_samples/2; k < _samples/2; k++) {
					for (int j = -_samples / 2; j < _samples / 2; j++) {
						float4 sampColor = tex2D(_MainTex, i.uv + float2(_sampleSizeX * k, _sampleSizeY * j));
						if (sampColor.a > 0.0) {
							sumColor += sampColor;
							count++;
						}
					}
				}

				sumColor /= count;

				float4 depth = Linear01Depth(tex2D(_CameraDepthTexture, i.uv).r);
           
				return sumColor;
			}
			ENDCG
		}
	}
			
}
