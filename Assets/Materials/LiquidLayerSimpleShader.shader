Shader "Custom/LiquidLayerSimpleShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _FillAmount ("Fill Amount", float) = 0.0
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
            float _FillAmount;

            struct VertIn {
                float4 pos : POSITION;
                float4 color: COLOR;
                float2 uv : TEXCOORD0;
            };

            struct VertOut {
                float4 pos : SV_Position;
                float4 color: COLOR;
                float4 model_pos : MODEL_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertOut vert (VertIn v)
            {
                VertOut o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = v.uv;
                o.color = v.color;
                o.model_pos = v.pos;
                return o;
            }

            fixed4 frag(VertOut input) : SV_Target {

                if (_FillAmount <= 0) discard;
                if (_FillAmount >= 1) return tex2D(_MainTex, input.uv) * input.color;
                
                float2 uv = input.uv;
                uv.y = uv.y + sin(input.model_pos.x * 3.0 + _Time.y * 3.0) * 0.1;

                if (uv.y >= _FillAmount) discard;

                if (input.model_pos.y > 0.1) return tex2D(_MainTex, uv) * input.color;

                return tex2D(_MainTex, input.uv) * input.color;
            }
            ENDCG
        }
    }
}
