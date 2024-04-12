Shader "Custom/RingGrowthShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _GrowthAmount("Growth Amount", Range(0, 1)) = 0
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            float _GrowthAmount;

            struct Input
            {
                float2 uv_MainTex;
                float3 worldNormal;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                fixed4 color = tex2D(_MainTex, IN.uv_MainTex);
                // ����_GrowthAmount�����������꣬��������Ч��
                float2 adjustedUV = IN.uv_MainTex * (1 + _GrowthAmount * (1 - IN.uv_MainTex.x));
                o.Albedo = color.rgb * color.a; // ʹ��������ɫ��ΪAlbedo
                o.Alpha = color.a; // ʹ�������Alpha��Ϊ͸����
                o.Normal = WorldNormalVector(IN, o.Normal); // ʹ������ռ䷨��
                o.Specular = 0;
                o.Gloss = 0;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
