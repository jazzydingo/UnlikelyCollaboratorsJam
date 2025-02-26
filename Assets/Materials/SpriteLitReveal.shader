Shader "Custom/SpriteLitRevealSpotlight"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [PerRendererData] _RevealTex ("Reveal Sprite Texture", 2D) = "white" {}

        _LightMultiplier ("Light Intensity", Range(0, 2)) = 1
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off  // Disable standard lighting (we use 2D light maps)
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "Universal2D" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _RevealTex;
            float4 _Color;
            float _LightMultiplier;

            // Unity's 2D Light Texture (captures spotlight effect)
            UNITY_DECLARE_TEX2D(unity_2DLightTex);

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the sprite texture
                fixed4 mainCol = tex2D(_MainTex, i.uv) * i.color;
                fixed4 revealCol = tex2D(_RevealTex, i.uv);

                // Sample Unity's 2D Light Texture (handles spotlight effects)
                fixed4 lightColor = UNITY_SAMPLE_TEX2D(unity_2DLightTex, i.uv);

                // Blend reveal texture based on alpha
                mainCol.rgb = lerp(mainCol.rgb, revealCol.rgb, revealCol.a);

                // Apply 2D lighting from Unity (spotlight effect)
                mainCol.rgb *= lightColor.rgb * _LightMultiplier;

                return mainCol;
            }
            ENDCG
        }
    }
}
