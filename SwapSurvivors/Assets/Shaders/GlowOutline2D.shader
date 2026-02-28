Shader "Custom/GlowOutline2D"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,1,0,1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 2
        _GlowIntensity ("Glow Intensity", Range(1, 10)) = 3
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        // --- PASS 1: GLOW OUTLINE ---
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Unity bunu otomatik doldurur (1/width, 1/height)
            float4 _OutlineColor;
            float _OutlineWidth;
            float _GlowIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Ana sprite alpha'sı
                float alpha = tex2D(_MainTex, i.uv).a;

                // 8 yönde komşu pixellere bak, herhangi birinde alpha varsa outline çiz
                float2 texel = _MainTex_TexelSize.xy * _OutlineWidth;

                float outline = 0;
                outline += tex2D(_MainTex, i.uv + float2( texel.x,  0)).a;
                outline += tex2D(_MainTex, i.uv + float2(-texel.x,  0)).a;
                outline += tex2D(_MainTex, i.uv + float2( 0,  texel.y)).a;
                outline += tex2D(_MainTex, i.uv + float2( 0, -texel.y)).a;
                outline += tex2D(_MainTex, i.uv + float2( texel.x,  texel.y)).a;
                outline += tex2D(_MainTex, i.uv + float2(-texel.x,  texel.y)).a;
                outline += tex2D(_MainTex, i.uv + float2( texel.x, -texel.y)).a;
                outline += tex2D(_MainTex, i.uv + float2(-texel.x, -texel.y)).a;

                // Komşularda sprite varsa ama burada yoksa = outline bölgesi
                outline = saturate(outline);
                float isOutline = outline * (1 - alpha);

                // Glow rengi (HDR benzeri parlama için intensity çarp)
                fixed4 glowColor = _OutlineColor * _GlowIntensity;
                glowColor.a = isOutline;

                // Ana sprite yoksa outline rengini döndür
                fixed4 spriteColor = tex2D(_MainTex, i.uv);

                // Sprite üstüne outline blend et
                return lerp(glowColor, spriteColor, alpha);
            }
            ENDCG
        }
    }
}
