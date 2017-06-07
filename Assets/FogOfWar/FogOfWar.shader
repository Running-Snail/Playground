Shader "Custom/FogOfWar" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_FogRadius ("Fog Radius", Float) = 4.0
		_Center1 ("Center1", Vector) = (0,0,0,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc";

		sampler2D _MainTex;
		fixed4 _Color;
		float _FogRadius;
		float4 _Center1;

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

		sampler2D _MainTex;
		float4 _MainTex_ST;
		
		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}
		
		fixed4 frag (v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_MainTex, i.uv);
			return col;
		}

		float transition(float x) {
			if (x < 0.95) return 0;
			else return x-0.95;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
