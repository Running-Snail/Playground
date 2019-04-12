Shader "WarOfColor/Fade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_NoiseStrength ("Noise Strength", Float) = 1
		_TintColor ("Tint Color", Color) = (0,0,0,1)
		_Range ("Range", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Tranparent" "Queue"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

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
				float2 noise : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 noise : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			sampler2D _NoiseTex;
			float _NoiseStrength;
			float _Range;
			float4 _TintColor;
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.noise = TRANSFORM_TEX(v.uv, _NoiseTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 noise = tex2D(_NoiseTex, i.noise);
				noise = tex2D(_NoiseTex, noise.xy) * _NoiseStrength;
				half2 uv = half2(i.uv.x + noise.x, i.uv.y + noise.y);
				fixed4 col = tex2D(_MainTex, uv);
				fixed4 mask = tex2D(_MaskTex, i.uv);
				col.a = mask.a;
				return col;
			}
			ENDCG
		}
	}
}
