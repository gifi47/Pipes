Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutlineThickness ("Outline Thickness", float) = 1.0
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
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
            float4 _MainTex_TexelSize;
            fixed4 _OutlineColor;
            float _OutlineThickness;

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

            VertOut vert (VertIn v)
            {
                VertOut o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(VertOut input) : SV_Target {

                fixed4 fragColor = tex2D(_MainTex, input.uv) * input.color;

                float2 up = float2(0.0, _OutlineThickness*_MainTex_TexelSize.y);
                float2 right = float2(_OutlineThickness*_MainTex_TexelSize.x, 0.0);

                //float2 up = float2(0.0, 0.012);
                //float2 right = float2(0.012, 0.0);

                fixed leftPixel = tex2D(_MainTex, input.uv - right).a;

				fixed upPixel = tex2D(_MainTex, input.uv + up).a;

				fixed rightPixel = tex2D(_MainTex, input.uv + right).a;

				fixed bottomPixel = tex2D(_MainTex, input.uv - up).a;

                float outline = (leftPixel + upPixel + rightPixel + bottomPixel);

                if (outline > 0 && fragColor.a == 0)
                    return _OutlineColor;
                else 
                {
                    if (fragColor.a != 0) fragColor.a = 1;
                    return fragColor;
                }
            }
            ENDCG
        }
    }
}
