Shader "Unmoving Plaid/Unmoving Plaid (Unlit)"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Color("Color", Color) = (1, 1, 1, 1)
        
        _HueShift("Hue Shift", Float) = 0.0
        
        _FitAspectRatio("Fit to Aspect Ratio", Integer) = 1
        
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull [_Cull]
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            float3 rgb2hsb( in float3 c )
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)),
                    d / (q.x + e),
                    q.x);
            }

            //  Function from Iñigo Quiles
            //  https://www.shadertoy.com/view/MsS3Wc
            float3 hsb2rgb( in float3 c )
            {
                float3 rgb = clamp(
                    abs((c.x*6.0+float3(0.0,4.0,2.0) % 6.0)-3.0)-1.0,
                    0.0,
                    1.0 );
                rgb = rgb*rgb*(3.0-2.0*rgb);
                return c.z * lerp(float3(1.0, 1.0, 1.0), rgb, c.y);
            }

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
            float _HueShift;

            int _FitAspectRatio;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 textureCoordinate = i.screenPos.xy / i.screenPos.w;
                float aspect = _ScreenParams.y / _ScreenParams.x;
                textureCoordinate.y *= _FitAspectRatio * aspect + (1 - _FitAspectRatio) * 1;
                textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
                
                fixed4 col = tex2D(_MainTex, textureCoordinate) * _Color;

                fixed3 hsb = rgb2hsb(col);
                hsb.r += _HueShift;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return fixed4(hsb2rgb(hsb), col.a);
            }
            ENDCG
        }
    }
}
