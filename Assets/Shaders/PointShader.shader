Shader "PointCloud/pointMesh" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Texture2("color", 2D) = "white" {}
	}
	SubShader{
		Pass{

		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#pragma glsl
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		float4 _MainTex_ST;

		sampler2D _Texture2;
		float4 _Texture2_ST;
		struct v2f {
			float4 pos : SV_POSITION;
			float2 texcoord : TEXCOORD0;
			fixed3 color : COLOR0;
			float size : PSIZE;
			float2 depth : TEXCOORD1;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			float4 tex = tex2Dlod(_MainTex, float4(v.texcoord.xy, 0, 0));
			if (tex.a < 0.01) {
				tex = float4(-9999, -9999, -9999, 1);
			}
			o.pos = UnityObjectToClipPos(tex);
			float4 quadPos = mul(UNITY_MATRIX_P, v.vertex);
			o.pos += quadPos;
			o.size = 10;
			o.color = float3(1, 1, 1);
			o.texcoord = v.texcoord;
			COMPUTE_EYEDEPTH(o.depth);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			//UNITY_OUTPUT_DEPTH(i.depth);
			return fixed4(tex2D(_Texture2, i.texcoord));
		}
			ENDCG

		}
	}
		FallBack "Diffuse"
}