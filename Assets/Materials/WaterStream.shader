Shader "Custom/WaterStream"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BubbleTex("Bubble Texture", 2D) = "white" {}
        _Speed("Speed", Float) = 0.1
        _Strength("Strength", Float) = 0.1
        _BubbleSpeed("Bubble Speed", Float) = 1.0
        _BubbleSize("Bubble Size", Float) = 0.1
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
                float4 model_pos : MODEL_POS;
            };

            sampler2D _MainTex;
            sampler2D _BubbleTex;
            float _Speed;
            float _Strength;
            float _BubbleSpeed;
            float _BubbleSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.model_pos = v.vertex;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                /*float2 sus = i.uv;
                fixed4 waterColor = tex2D(_MainTex, sus) * i.color;
                float bubbleOffset = sin(i.uv.x + _Time.y);//_Time.y * _BubbleSpeed;
                float bubbleIntensity = smoothstep(0.5 - _BubbleSize, 0.5 + _BubbleSize, frac(i.uv.y + bubbleOffset));
                float a1 = i.uv.x + bubbleOffset;
                float a2 = i.uv.y;
                fixed4 bubbleColor = tex2D(_BubbleTex, float2(frac(a1), a2));
                fixed4 finalColor = lerp(waterColor, bubbleColor, bubbleIntensity);
                return finalColor;*/
                float2 uv = i.uv;
                uv.y = uv.y + sin(i.model_pos.x + _Time.y) * 0.2;
                if (uv.y >= 0.75) discard;
                fixed4 color = tex2D(_MainTex, uv) * i.color;
                float2 modif = float2(-0.8, -0.6);
                modif.x = modif.x + cos(_Time.y) * 0.5;
                modif.y = modif.y + cos(_Time.y) * 0.5;

                float2 pos2 = i.model_pos.xy;
                pos2.x = frac(pos2.x);
                pos2.y = frac(pos2.y);
                float2 sus = pos2 * 4 + modif;
                if (!(sus.x > 1.0 || sus.y > 1.0 || sus.x < 0 || sus.y < 0))
                    color = color + tex2D(_BubbleTex, sus);
                return color;
            }
            ENDCG
        }
    }
}
