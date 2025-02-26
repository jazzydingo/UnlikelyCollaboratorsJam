Shader "Custom/SpriteLitSpotlight"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Cull Off
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
            #include "UnityLightingCommon.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            // Unity's 2D Lighting Texture (captures spotlight influence)
            UNITY_DECLARE_TEX2D(unity_2DLightTex);

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // World Position
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the sprite texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Sample the 2D lighting texture (adjust world position for correct mapping)
                fixed4 lightColor = UNITY_SAMPLE_TEX2D(unity_2DLightTex, i.worldPos.xy * 0.5 + 0.5);

                // Multiply sprite color by light intensity
                col.rgb *= lightColor.rgb;

                return col;
            }
            ENDCG
        }
    }
}
