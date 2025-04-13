Shader "Custom/PixelSpriteOutline"
{
    Properties
    {
        _MainTex("Sprite", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineSize("Outline Size (in pixels)", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineSize;
            float4 _MainTex_TexelSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float alpha = tex2D(_MainTex, uv).a;

                // Если есть альфа — показываем пиксель спрайта
                if (alpha > 0)
                {
                    return tex2D(_MainTex, uv);
                }

                // Иначе — проверим соседние пиксели
                float outline = 0.0;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        float2 offset = float2(x, y) * _OutlineSize * _MainTex_TexelSize.xy;
                        outline += tex2D(_MainTex, uv + offset).a;
                    }
                }

                if (outline > 0)
                {
                    return _OutlineColor;
                }

                return float4(0, 0, 0, 0); // полностью прозрачный
            }
            ENDCG
        }
    }
}
