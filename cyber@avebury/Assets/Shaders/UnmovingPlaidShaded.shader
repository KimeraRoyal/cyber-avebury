Shader "Unmoving Plaid/Unmoving Plaid (Shaded)"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _FitAspectRatio("Fit to Aspect Ratio", Integer) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float4 screenPos;
        };

        float4 _MainTex_ST;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        int _FitAspectRatio;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            float2 textureCoordinate = i.screenPos.xy / i.screenPos.w;
            float aspect = _ScreenParams.y / _ScreenParams.x;
            textureCoordinate.y *= _FitAspectRatio * aspect + (1 - _FitAspectRatio) * 1;
            textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
            
            fixed4 c = tex2D (_MainTex, textureCoordinate) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}