// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/FogOfWar"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_Radius ("Radius", Float) = 1.0
		_Center1 ("Center1", Vector) = (0,0,0,1)
		_Thredhold ("Thredhold", Range(0.0, 1.0)) = 0.95
		_Alpha ("Alpha", Range(0.0, 1.0)) = 0.0
	}
	SubShader {
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
		}
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 wpos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;
			float _Radius;
			float4 _Center1;
			float _Thredhold;
			float _Alpha;

			float transition(float x) {
				if (x < _Thredhold) return _Alpha;
				else return (_Alpha - 1.0)*x/(_Thredhold - 1.0) + (_Thredhold - _Alpha)/(_Thredhold - 1.0);
			}

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.wpos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float dis = length(i.wpos.xy - _Center1.xy);
				col.rgb *= _TintColor.rgb;
				col.a *= _TintColor.a;
				col.a *= transition(dis/_Radius);
				return col;
			}
			ENDCG
		}
	}
}
