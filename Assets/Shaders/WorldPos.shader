// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "PointCloud/WorldPos" {
	SubShader{

		Pass{

		CGPROGRAM
		

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

	struct v2f {
		float4 pos : SV_POSITION;
		fixed4 color : COLOR0;
		float2 depth : TEXCOORD0;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = mul(unity_ObjectToWorld, v.vertex);
		COMPUTE_EYEDEPTH(o.depth);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		return float4(i.color.xyz, 1);
	}
		ENDCG

	}
	}

}