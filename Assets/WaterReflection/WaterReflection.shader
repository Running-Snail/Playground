Shader "Custom/WaterReflection"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_NoiseTex ("Noise Texture", 2D) = "bump" {}
		_ReflectionTex ("Reflection Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Range(0, 1)) = 0.02
		_Speed ("Speed", Range(0, 10)) = 0.15
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uvnoise : TEXCOORD0;
				float2 uvrefl : TEXCOORD1;
			};

			struct v2f
			{
				float2 uvnoise : TEXCOORD0;
				float2 uvrefl : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _ReflectionTex;
			float4 _ReflectionTex_ST;

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;

			fixed4 _TintColor;
			fixed _Magnitude;
			fixed _Speed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvnoise = TRANSFORM_TEX(v.uvnoise, _NoiseTex);
				o.uvrefl = TRANSFORM_TEX(v.uvrefl, _ReflectionTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// perspective correction
				// i.uvnoise += fixed2(2.0f * (0.5 - i.uvnoise.x) * i.uvnoise.y, 0.0f);
				fixed4 noise = tex2D(_NoiseTex, i.uvnoise + fixed2(_Speed * _Time.y, 0));
 				fixed2 distortion = UnpackNormal(noise).rg;

				// reverse uvrefl
				fixed2 p = fixed2(i.uvrefl.x, 1 - i.uvrefl.y) + distortion * _Magnitude;
				fixed4 refl = tex2D(_ReflectionTex, p);
				return refl * _TintColor;
			}
			ENDCG
		}
	}
}
