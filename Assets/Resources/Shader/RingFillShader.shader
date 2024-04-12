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
                // 根据_GrowthAmount调整纹理坐标，创建增长效果
                float2 adjustedUV = IN.uv_MainTex * (1 + _GrowthAmount * (1 - IN.uv_MainTex.x));
                o.Albedo = color.rgb * color.a; // 使用纹理颜色作为Albedo
                o.Alpha = color.a; // 使用纹理的Alpha作为透明度
                o.Normal = WorldNormalVector(IN, o.Normal); // 使用世界空间法线
                o.Specular = 0;
                o.Gloss = 0;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
