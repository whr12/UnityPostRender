// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Post/accumulationBlur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurAmount("BlurAmout", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGINCLUDE
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        half _BlurAmount;

        struct V2F
        {
            float4 position : SV_POSITION;
            half2 uv : TEXCOORD0;
        };

        V2F vert(appdata_full input)
        {
            V2F output;
            output.position = UnityObjectToClipPos(input.vertex);
            output.uv = input.texcoord;
            return output;
        }

        fixed4 fragA(V2F i) : SV_Target
        {
            return tex2D(_MainTex, i.uv);
        }

        fixed4 fragRGB(V2F i) : SV_Target
        {
            return fixed4(tex2D(_MainTex, i.uv).rgb, _BlurAmount);
        }

        ENDCG

        ZTest Always Cull Off ZWrite Off
         Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragRGB
            ENDCG
        }

        Pass
        {
            Blend One Zero
            ColorMask A

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragA
            ENDCG
        }

    }
}
