Shader "Unlit/Wireframe with Unmoving Plaid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Color("Wire Color", Color) = (1, 1, 1, 1)
        
        _WireframeAliasing("Wireframe Aliasing", Float) = 1.5
        
        _HueShift("Hue Shift", Float) = 0.0
        
        _FitAspectRatio("Fit to Aspect Ratio", Integer) = 1
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
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
                
                fixed4 col = tex2D(_MainTex, textureCoordinate);

                fixed3 hsb = rgb2hsb(col);
                hsb.r += _HueShift;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return fixed4(hsb2rgb(hsb), col.a);
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            struct g2f
            {
                float4 vertex: SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 barycentric : TEXCOORD1;
            };

            v2g vert (appdata v)
            {
                v2g o;
                
                o.vertex = v.vertex;
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            // https://www.youtube.com/watch?v=nr5QmPo-HxY
            [maxvertexcount(3)]
            void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream)
            {
                float l0 = length(i[1].vertex - i[2].vertex);
                float l1 = length(i[0].vertex - i[2].vertex);
                float l2 = length(i[0].vertex - i[1].vertex);

                float longestSide = max(l0, max(l1, l2));
                bool s0 = abs(l0 - longestSide) < 0.001f && (abs(l1 - longestSide) < 0.001f || abs(l2 - longestSide) < 0.001f);
                bool s1 = abs(l1 - longestSide) < 0.001f && (abs(l0 - longestSide) < 0.001f || abs(l2 - longestSide) < 0.001f);
                bool s2 = abs(l2 - longestSide) < 0.001f && (abs(l0 - longestSide) < 0.001f || abs(l1 - longestSide) < 0.001f);
                bool valid = 1 - (s0 || s1 || s2);

                bool m0 = (l0 > l1) && (l0 > l2);
                bool m1 = (l1 > l0) && (l1 > l2);
                bool m2 = (l2 > l0) && (l2 > l1);
                float3 modifier = float3(m0, m1, m2) * valid;
                
                g2f o;

                o.vertex = UnityObjectToClipPos(i[0].vertex);
                o.uv = i[0].uv;
                o.barycentric = float3(1.0, 0.0, 0.0) + modifier;
                triStream.Append(o);
                
                o.vertex = UnityObjectToClipPos(i[1].vertex);
                o.uv = i[1].uv;
                o.barycentric = float3(0.0, 1.0, 0.0) + modifier;
                triStream.Append(o);
                
                o.vertex = UnityObjectToClipPos(i[2].vertex);
                o.uv = i[2].uv;
                o.barycentric = float3(0.0, 0.0, 1.0) + modifier;
                triStream.Append(o);
            }

            half4 _Color;

            float _WireframeAliasing;

            fixed4 frag (g2f i) : SV_Target
            {
                float3 unitWidth = fwidth(i.barycentric);
                float3 aliased = smoothstep(float3(0.0, 0.0, 0.0), unitWidth * _WireframeAliasing, i.barycentric);
                float alpha = 1 - min(aliased.x, min(aliased.y, aliased.z));
                
                fixed4 col = fixed4(_Color.xyz, alpha);
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
        }
    }
}
