// Based off of Unity's default sprite shaders:
// - https://github.com/TwoTailsGames/Unity-Built-in-Shaders/blob/master/CGIncludes/UnitySprites.cginc

#ifndef UNITY_SPRITES_INCLUDED
#define UNITY_SPRITES_INCLUDED

#include "UnityCG.cginc"
#include "Lighting.cginc"

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

struct VertexData {
    float4 vertex   : POSITION;
    float3 normal   : NORMAL;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Interpolators {
    float4 vertex      : SV_POSITION;
    fixed4 color       : COLOR;
    float2 texcoord    : TEXCOORD0;
    float3 worldPos    : TEXCOORD1;
    float3 worldNormal : TEXCOORD2;
    UNITY_VERTEX_OUTPUT_STEREO
};

inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip) {
    return float4(pos.xy * flip, pos.z, 1.0);
}

Interpolators VertexProgram(VertexData IN) {
    Interpolators OUT;

    UNITY_SETUP_INSTANCE_ID (IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
    OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
    OUT.worldNormal = UnityObjectToWorldNormal(IN.normal);

    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color * _RendererColor;

    #ifdef PIXELSNAP_ON
        OUT.vertex = UnityPixelSnap (OUT.vertex);
    #endif

    return OUT;
}

sampler2D _MainTex;
sampler2D _AlphaTex;

fixed4 SampleSpriteTexture (float2 uv) {
    fixed4 color = tex2D (_MainTex, uv);

    #if ETC1_EXTERNAL_ALPHA
        fixed4 alpha = tex2D (_AlphaTex, uv);
        color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
    #endif

    return color;
}

fixed4 FragmentProgram(Interpolators IN) : SV_Target {
    IN.worldNormal = normalize(IN.worldNormal);

    float3 lightDir = _WorldSpaceLightPos0.xyz;
    float diffuse = max(dot(IN.worldNormal, lightDir), 0);

    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
    c.rgb *= c.a * _LightColor0.rgb * diffuse;
    return c;
}

#endif // UNITY_SPRITES_INCLUDED