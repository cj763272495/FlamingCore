// jave.lin 2020.04.17 - 高度雾 - 代码写法很多可以优化，这里做学习用，便于高可读性
Shader "Custom/HeightFog" {
    Properties{
        [KeywordEnum(VIEWSPACE,WORLDSPACE)] _DIST_TYPE("Distance type", int) = 0
        [KeywordEnum(LINEAR,EXP,EXP2)] _FUNC_TYPE("Calculate Func type", int) = 0
        _MainTex("Texture", 2D) = "white" {}                                       // source tex
        _NoiseTex("NoiseTex", 2D) = "white" {}                                     // 噪点图

        _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1)                         // 雾的颜色

        _WorldPosScale("WorldPosScale", Range(0, 0.1)) = 0.05                      // 世界坐标XY采样noiseTex的UV缩放值
        _NoiseSpX("Noise Speed X", Range(0, 1)) = 1                                // 噪点滚动的速度（类似风吹动雾的效果）
        _NoiseSpY("Noise Speed Y", Range(0, 1)) = 1                                // 噪点滚动的速度（类似风吹动雾的效果）

        _HeightStart("Height Start", Float) = 1                                    // 淡入雾效的开始高度
        _HeightEnd("Height End", Float) = 0                                        // 完全雾效的结束高度
        _HeightNoiseScale("Height Noise Scale", Range(0, 10)) = 1                  // 高度噪点强度缩放

        _WholeIntensity("WholeIntensity", Range(0, 1)) = 1                         // 整体效果的强度
    }
        SubShader{
            ZWrite Off ZTest Always Cull Off
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile _DIST_TYPE_VIEWSPACE _DIST_TYPE_WORLDSPACE
                #pragma multi_compile _FUNC_TYPE_LINEAR _FUNC_TYPE_EXP _FUNC_TYPE_EXP2
                #include "UnityCG.cginc"
                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    uint id : SV_VertexID;
                };
                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 ray : TEXCOORD1;
                };
                sampler2D _CameraDepthTexture;
                sampler2D _MainTex;
                sampler2D _NoiseTex;
                fixed4 _FogColor;
                float4x4 _Ray;
                float _WorldPosScale;
                float _NoiseSpX;
                float _NoiseSpY;
                float _HeightStart;
                float _HeightEnd;
                float _HeightNoiseScale;
                float _WholeIntensity;
                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.ray = _Ray[v.id].xyz;
                    return o;
                }
                fixed4 frag(v2f i) : SV_Target {

                    float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                // return depth;

                // world pos
                float3 wp = _WorldSpaceCameraPos.xyz + i.ray * Linear01Depth(depth);

                // move the noise fog
                float noise = tex2D(_NoiseTex, wp.xz * _WorldPosScale + _Time.x * fixed2(_NoiseSpX, _NoiseSpY)).r;
                float heightNoise = noise * _HeightNoiseScale;

                // height fog
                float factor = (_HeightEnd - wp.y - heightNoise) / (_HeightEnd - _HeightStart);

                factor = saturate(factor);

                fixed4 texCol = tex2D(_MainTex, i.uv);
                fixed4 fogColor = lerp(texCol, _FogColor, _FogColor.a);
                return lerp(fogColor, texCol, lerp(1, factor, _WholeIntensity));
            }
            ENDCG
        }
        }
}

