Shader "Unmoving Plaid/Unmoving Plaid (Unlit)"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Color("Color", Color) = (1, 1, 1, 1)
        
        _FitAspectRatio("Fit to Aspect Ratio", Integer) = 1
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
            #pragma multi_compile_fog

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
                float4 screenPos : TEXCOORD1;
                
                UNITY_FOG_COORDS(1)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            half4 _Color;

            int _FitAspectRatio;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 textureCoordinate = i.screenPos.xy / i.screenPos.w;
                float aspect = _ScreenParams.y / _ScreenParams.x;
                textureCoordinate.y *= _FitAspectRatio * aspect + (1 - _FitAspectRatio) * 1;
                textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
                
                fixed4 col = tex2D(_MainTex, textureCoordinate) * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
