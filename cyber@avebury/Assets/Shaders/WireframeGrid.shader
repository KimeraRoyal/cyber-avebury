Shader "Unlit/Wireframe Grid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Color("Color", Color) = (1, 1, 1, 1)
        _WireColor("Wire Color", Color) = (1, 1, 1, 1)
        _FadeColor("Fade Color", Color) = (0, 0, 0, 1)
        
        _Cutoff("Cutoff", Range(0.0, 1.0)) = 0.

        _Exponent("Fade Exponent", Float) = 4.0
        _MinFade("Min Fade", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 _Color;
            half4 _WireColor;
            half4 _FadeColor;
            
            float _Cutoff;

            float _Exponent;
            float _MinFade;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvValid = (i.uv % 1 < _Cutoff) + (1 - i.uv % 1 < _Cutoff);
                bool showWire = uvValid.x + uvValid.y;
                
                half4 col = _Color * (1.0f - showWire) + _WireColor * showWire;

                float2 fade = (1 - min(1, (i.uv - _MainTex_ST.zw) / _MinFade)) + (1 - min(1, (_MainTex_ST.xy - (i.uv - _MainTex_ST.zw)) / _MinFade));
                float expFade = 1 - exp2(-_Exponent * (fade.x + fade.y));

                return col * (1 - expFade) + _FadeColor * (expFade);
            }
            ENDCG
        }
    }
}
