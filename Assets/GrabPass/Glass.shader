Shader "Custom/Glass"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (0,0,0,1)
		_BumpMap ("Noise", 2D) = "bump" {}
		_Magnitude ("Magnitude", Range(0,1)) = 0.05
	}
	SubShader
	{
		Tags {
			"RenderType"="Opaque"
			"Queue"="Transparent"
			"IgnoreProjector"="True"
		}
		LOD 100

		GrabPass {}

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 uvgrab : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _BumpMap;
			sampler2D _GrabTexture;
			float4 _MainTex_ST;
			float4 _TintColor;
			float _Magnitude;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				half4 mainColor = tex2D(_MainTex, i.uv);
				half4 bump = tex2D(_BumpMap, i.uv);
				half2 distortion = UnpackNormal(bump).rg;
				i.uvgrab.xy += distortion * _Magnitude;
				fixed4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				return col * mainColor * _TintColor;
			}
			ENDCG
		}
	}
}
