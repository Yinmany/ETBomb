// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Circular"
{
    Properties
    {
        _MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15
        // 以 1 - _Radius 长度为半径的圆形
        _Radius("Radius", Range(0,0.5)) = 0.5
    }

    SubShader
    {
        LOD 100

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Offset -1, -1
        Fog
        {
            Mode Off
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]



        Pass
        {
            CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Radius;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
#ifdef UNITY_HALF_TEXEL_OFFSET
				o.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
#endif
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float2 uv = i.texcoord.xy;
				float4 c = i.color;
				// 左下四方块
				if (uv.x < _Radius && uv.y < _Radius)
				{
					float2 r;
					r.x = uv.x - _Radius;
					r.y = uv.y - _Radius;
					float rr = length(r);
					// 裁剪
					if (rr > _Radius)
					{
						c.a = 0;
					}
				}
				// 左上四方块
				else if (uv.x < _Radius && uv.y > 1 - _Radius)
				{
					float2 r;
					r.x = uv.x - _Radius;
					r.y = uv.y + _Radius - 1;
					float rr = length(r);
					// 裁剪
					if (rr > _Radius)
					{
						c.a = 0;
					}
				}
				// 右下四方块
				else if (uv.x > 1 - _Radius && uv.y < _Radius)
				{
					float2 r;
					r.x = uv.x + _Radius - 1;
					r.y = uv.y - _Radius;
					float rr = length(r);
					// 裁剪
					if (rr > _Radius)
					{
						c.a = 0;
					}
				}
				// 右上四方块
				else if (uv.x > 1 - _Radius && uv.y > 1 - _Radius)
				{
					float2 r;
					r.x = uv.x + _Radius - 1;
					r.y = uv.y + _Radius - 1;
					float rr = length(r);
					// 裁剪
					if (rr > _Radius)
					{
						c.a = 0;
					}
				}

				fixed4 col = tex2D(_MainTex, i.texcoord) * c;
				clip(col.a - 0.01);
				return col;
			}

			ENDCG
        }
    }
}