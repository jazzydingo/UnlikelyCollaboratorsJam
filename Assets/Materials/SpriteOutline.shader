Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)  // Default red outline
        _OutlineThickness ("Outline Thickness", Range(0.001, 0.05)) = 0.01
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;

                // Discard fully transparent pixels to avoid square outline
                if (alpha == 0)
                    discard;

                // Outline offsets in 8 directions
                float2 offsets[8] = {
                    float2(_OutlineThickness, 0),
                    float2(-_OutlineThickness, 0),
                    float2(0, _OutlineThickness),
                    float2(0, -_OutlineThickness),
                    float2(_OutlineThickness, _OutlineThickness),
                    float2(-_OutlineThickness, _OutlineThickness),
                    float2(_OutlineThickness, -_OutlineThickness),
                    float2(-_OutlineThickness, -_OutlineThickness)
                };

                // Check if any neighboring pixel is transparent (edge detection)
                float outline = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (tex2D(_MainTex, i.uv + offsets[j]).a < 0.1)
                        outline = 1;
                }

                // If on an edge, return the outline color
                if (outline > 0)
                    return _OutlineColor;

                // Otherwise, return the original sprite color
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}