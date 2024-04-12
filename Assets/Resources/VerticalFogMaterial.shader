Shader "URP/SLG_scenario_Fog"
{
    Properties
    {
        _SoftFade("_SoftFade",Float) = 1.0
        _FogColor("_FogColor",Color) = (1,1,1,1)
        _Soft("_Soft",Range(0,3)) = 1.0
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        HLSLINCLUDE


        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


        CBUFFER_START(UnityPerMaterial)
        float _SoftFade,_Soft;
                float4 _MainTex_ST;
        half4 _FogColor;

        CBUFFER_END


        TEXTURE2D(_CameraDepthTexture);
        SAMPLER(sampler_CameraDepthTexture);


        struct a2v
        {
            float3 positionOS : POSITION;
            float4 normalOS : NORMAL;
            float2 texcoord :TEXCOORD0;
        };


        struct v2f
        {
            float4 positionCS : SV_POSITION;
            float3 normalWS : NORMAL;
            float3 positionWS:TEXCOORD1;
            float4 scrPos : TEXCOORD0;
            float4 uv : TEXCOORD2;
        };
        ENDHLSL

        Pass
        {
            Name "Pass"
            Tags
            {
                "LightMode" = "UniversalForward"
                "RenderType" = "Transparent"
            }

            Blend SrcAlpha OneMinusSrcAlpha


            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag


            v2f vert(a2v v) {
                v2f o;

                VertexPositionInputs  PositionInputs = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = PositionInputs.positionCS;
                o.positionWS = PositionInputs.positionWS;

                VertexNormalInputs NormalInputs = GetVertexNormalInputs(v.normalOS.xyz);
                o.normalWS.xyz = NormalInputs.normalWS;

                o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);

                o.scrPos = ComputeScreenPos(o.positionCS);
                return o;
            }


            half4 frag(v2f i) : SV_Target
            {
                float2 ndcPos = i.scrPos.xy / i.scrPos.w;                                                        // [0-1] 透视除法
                float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,ndcPos).r;         // [-w,w] 计算深度
                float LinearDepth = LinearEyeDepth(depth,_ZBufferParams);


                float fade = saturate((LinearDepth - i.scrPos.w - _Soft) / _SoftFade);                                    //使用全部深度 --减去物体的深度

                float4 col = fade;
                col.rgb *= _FogColor.rgb;
                col.a *= clamp(0,1,fade) * _FogColor.a;
                return col;
            }
            ENDHLSL
        }
    }
} 作者：那个人真狗 https ://www.bilibili.com/read/cv16199144/ 出处：bilibili