// Based off of Unity's default sprite shaders:
// - https://github.com/TwoTailsGames/Unity-Built-in-Shaders/blob/master/CGIncludes/UnitySprites.cginc

#ifndef UNITY_SPRITES_INCLUDED
#define UNITY_SPRITES_INCLUDED

#include "UnityCG.cginc"

#ifdef UNITY_INSTANCING_ENABLED

    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
        // SpriteRenderer.Color while Non-Batched/Instanced.
        UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
        // this could be smaller but that's how bit each entry is regardless of type
        UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

#endif // instancing

CBUFFER_START(UnityPerDrawSprite)
#ifndef UNITY_INSTANCING_ENABLED
    fixed4 _RendererColor;
    fixed2 _Flip;
#endif
    float _EnableExternalAlpha;
CBUFFER_END

// Material Color.
fixed4 _Color;

struct appdata_t {
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f {
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    float4 worldPos : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip) {
    return float4(pos.xy * flip, pos.z, 1.0);
}

v2f SpriteVert(appdata_t IN) {
    v2f OUT;

    UNITY_SETUP_INSTANCE_ID (IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color * _RendererColor;
    OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);

    #ifdef PIXELSNAP_ON
        OUT.vertex = UnityPixelSnap (OUT.vertex);
    #endif

    return OUT;
}

sampler2D _MainTex;
sampler2D _RevealTex;
sampler2D _AlphaTex;

float4 _LightPos;
float _LightRadius;

fixed4 SampleSpriteTexture (float2 uv, sampler2D tex) {
    fixed4 color = tex2D (tex, uv);

    #if ETC1_EXTERNAL_ALPHA
        fixed4 alpha = tex2D (_AlphaTex, uv);
        color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
    #endif

    return color;
}

fixed4 SpriteFrag(v2f IN) : SV_Target {
    float3 lightDirection = (float3)_LightPos - IN.worldPos;
    float distanceSq = dot(lightDirection.xy, lightDirection.xy); // Ignores Y for now

    float lightRadiusSq = _LightRadius * _LightRadius; 
    float alpha = saturate(distanceSq - lightRadiusSq);

    fixed4 colorMain = SampleSpriteTexture(IN.texcoord, _MainTex) * IN.color;
    fixed4 colorReveal = SampleSpriteTexture(IN.texcoord, _RevealTex) * IN.color;

    colorMain.rgb *= colorMain.a;
    colorReveal.rgb *= colorReveal.a;

    return lerp(colorReveal, colorMain, alpha);
}

#endif // UNITY_SPRITES_INCLUDED