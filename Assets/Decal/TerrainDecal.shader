Shader "Unlit/TerrainDecal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DecalCenter ("Decal Center", Vector) = (0, 0, 0, 0)
		_DecalSize ("Decal Size", Float) = 1
		_Color ("Color", Color) = (1, 1, 1, 1)
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
			
			#include "Lighting.cginc"
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 wpos : TEXCOORD1;
				float3 wnormal : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _DecalCenter;
			float _DecalSize;
			float4 _Color;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.wpos = mul(unity_ObjectToWorld, v.vertex);
				o.wnormal = mul(v.normal, (float3x3)unity_WorldToObject);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed3 wLightDir = normalize(_WorldSpaceLightPos0.xyz);
				half3 albedo = col.rgb;
				half3 rpos = i.wpos - _DecalCenter.xyz;
				if (length(rpos) < _DecalSize) {
					return fixed4(1, 0, 0, 1);
				}
				albedo *= _LightColor0.rgb * _Color.rgb * saturate(dot(i.wnormal, wLightDir));
				return fixed4(albedo, 1);
			}
			ENDCG
		}
	}
}
