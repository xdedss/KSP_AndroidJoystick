Shader "Unlit/UnlitRotate"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	    _CutOff("CutOff", 2D) = "white" {}
		_Overlay("Overlay", 2D) = "white" {}
		_OverlayCutOff("OverlayCutOff", 2D) = "white" {}
		_OverlayRoll("OverlayRoll", 2D) = "white" {}
		_OverlayRollCutOff("OverlayRollCutOff", 2D) = "white" {}
		_SpriteTransform ("SpriteTransform", Vector) = (1, 1, 0.5, 0.5)
		_RotateAngle("RotateAngle", float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CutOff;
			float4 _CutOff_ST;
			sampler2D _Overlay;
			sampler2D _OverlayCutOff;
			sampler2D _OverlayRoll;
			sampler2D _OverlayRollCutOff;
			float4 _SpriteTransform;
			float _RotateAngle;
			
			float2 rotate(float2 v, float2 c, float dangle) {
				float2 d = v - c;
				float mag = length(d);
				float ang = atan2(d.y, d.x);
				ang += dangle;
				return c + float2(mag * cos(ang), mag * sin(ang));
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float2 originalD = (v.uv - _SpriteTransform.zw) * _SpriteTransform.xy;
				float2 center = _MainTex_ST.zw;
				float2 transformed = rotate(originalD + center, center, _RotateAngle);
				o.uv.xy = (transformed - center) / _MainTex_ST.xy + center;
				o.uv.zw = TRANSFORM_TEX(v.uv, _CutOff);

				o.uv2 = rotate(originalD + float2(0.5, 0.5), float2(0.5, 0.5), _RotateAngle);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				fixed4 col = tex2D(_MainTex, i.uv.xy);
				fixed4 col_fg = tex2D(_Overlay, i.uv.xy);
				fixed4 col_fg2 = tex2D(_OverlayRoll, i.uv2);
				fixed4 cut = tex2D(_CutOff, i.uv.zw);
				fixed4 cut_fg = tex2D(_OverlayCutOff, i.uv.zw);
				fixed4 cut_fg2 = tex2D(_OverlayRollCutOff, i.uv.zw);
				float fga = col_fg.a * cut_fg.a;
				col.xyz = col_fg.xyz * fga + col.xyz * (1 - fga);
				float fg2a = col_fg2.a * cut_fg2.a;
				//col.xyz = col_fg2.xyz * fg2a + col.xyz * (1 - fg2a);

				UNITY_APPLY_FOG(i.fogCoord, col);
				return fixed4(col.rgb, col.a * cut.a);
			}
			ENDCG
		}
	}
}
