/*
Created by jiadong chen
https://jiadong-chen.medium.com/
*/

Shader "chenjd/URP/AnimMapWithShadowShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

        [Header(AnimMap)]
        _AnimMap("AnimMap", 2D) = "white" {}
        _AnimLen("Anim Length", Float) = 0
    }

        CGINCLUDE

		#include "UnityCG.cginc"

        sampler2D _MainTex;
        float4 _MainTex_ST;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        fixed4 frag(v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv);
            return col;
        }

		struct v2fShadow {
            V2F_SHADOW_CASTER;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        float4 fragShadow(v2fShadow i) : SV_Target
        {
            SHADOW_CASTER_FRAGMENT(i)
        }

		sampler2D _AnimMap;
		float4 _AnimMap_TexelSize;//x == 1/width

		float _AnimLen;


		v2f vert(appdata v, uint vid : SV_VertexID)
		{
			UNITY_SETUP_INSTANCE_ID(v);

			float f = _Time.y / _AnimLen;

			fmod(f, 1.0);

			float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;
			float animMap_y = f;

			float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));

			v2f o;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.vertex = UnityObjectToClipPos(pos);
			return o;
		}

		ENDCG



		SubShader
        {
			Pass
            {
				Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}
				Tags { "LightMode"="UniversalForward" }
				Cull off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                //开启gpu instancing
                #pragma multi_compile_instancing

                #include "UnityCG.cginc"


                ENDCG
            }

			Pass
			{
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }
				LOD 100

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragShadow
				#pragma target 2.0
				#pragma multi_compile_shadowcaster
				ENDCG
			}
        }
}

