Shader "Unlit/Wireframe"
{
    Properties
    {
        _WireColor("Wire Color", Color) = (1, 1, 1, 1)
        
        _WireframeAliasing("Wireframe Aliasing", Float) = 1.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        LOD 100

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

            half4 _WireColor;

            float _WireframeAliasing;

            fixed4 frag (g2f i) : SV_Target
            {
                float3 unitWidth = fwidth(i.barycentric);
                float3 aliased = smoothstep(float3(0.0, 0.0, 0.0), unitWidth * _WireframeAliasing, i.barycentric);
                float alpha = 1 - min(aliased.x, min(aliased.y, aliased.z));
                
                fixed4 col = fixed4(_WireColor.xyz, alpha);
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
        }
    }
}
