Shader "Custom/LiquidLayerShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        LOD 100
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }

        Pass
        {
            ZTest Off
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct VertIn {
                float4 pos : POSITION;
                float4 color: COLOR;
                float2 uv : TEXCOORD0;
            };

            struct VertOut {
                float4 pos : SV_Position;
                float4 color: COLOR;
                float2 uv : TEXCOORD0;
            };

            struct FragOut {
                fixed4 color : SV_Target;
            };

            VertOut vert (VertIn v)
            {
                VertOut o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            FragOut frag(VertOut input) {
                FragOut o;
                if (input.color.a == 0) discard;
                fixed4 color = tex2D(_MainTex, input.uv);
                if (color.a > 0.1) {
                    o.color = fixed4(1, 1, 1, 0) * color.g + input.color * (1.0 - color.g);
                    o.color.a = (1 - color.g * 2);
                } else {
                    discard;
                }
                return o;
            }
            ENDCG
        }
    }
}
