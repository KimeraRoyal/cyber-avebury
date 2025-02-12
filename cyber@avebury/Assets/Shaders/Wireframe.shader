Shader "Unlit/Wireframe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _Color("Color", Color) = (1, 1, 1, 1)
        _WireColor("Wire Color", Color) = (1, 1, 1, 1)
        
        _Exponent("Exponent", Float) = 4.0
        _Cutoff("Cutoff", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite On
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

                float4 screenPos : TEXCOORD2;
            };

            struct g2f
            {
                float4 vertex: SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 distance : TEXCOORD1;

                float4 screenPos : TEXCOORD2;
            };

            v2g vert (appdata v)
            {
                v2g o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            // https://github.com/Scrawk/Wireframe-Shader/blob/master/Assets/Wireframe%20Shader/Wireframe.shader
            [maxvertexcount(3)]
            void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream)
            {
                float2 winScale = float2(_ScreenParams.x / 2.0f, _ScreenParams.y / 2.0f);

                float2 p0 = winScale * i[0].vertex.xy / i[0].vertex.w;
                float2 p1 = winScale * i[1].vertex.xy / i[1].vertex.w;
                float2 p2 = winScale * i[2].vertex.xy / i[2].vertex.w;

                float2 v0 = p2 - p1;
                float2 v1 = p2 - p0;
                float2 v2 = p1 - p0;

                float area = abs(v1.x * v2.y - v1.y * v2.x);

                g2f o;

                o.vertex = i[0].vertex;
                o.uv = i[0].uv;
                o.distance = float3(area / length(v0), 0, 0);
                
                o.screenPos = i[0].screenPos;
                
                triStream.Append(o);
                
                o.vertex = i[1].vertex;
                o.uv = i[1].uv;
                o.distance = float3(0, area / length(v1), 0);
                
                o.screenPos = i[1].screenPos;
                
                triStream.Append(o);
                
                o.vertex = i[2].vertex;
                o.uv = i[2].uv;
                o.distance = float3(0, 0, area / length(v2));
                
                o.screenPos = i[2].screenPos;
                
                triStream.Append(o);
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            half4 _Color;
            half4 _WireColor;

            float _Exponent;
            float _Cutoff;

            fixed4 frag (g2f i) : SV_Target
            {
                float distance = min(i.distance.x, min(i.distance.y, i.distance.z));
                float fade = exp2(-_Exponent * distance * distance);

                float2 textureCoordinate = i.screenPos.xy / i.screenPos.w;
                textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
                
                fixed4 col = tex2D(_MainTex, textureCoordinate) * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);

                bool showWire = fade >= _Cutoff;
                return col * (1.0f - showWire) + _WireColor * showWire;
            }
            ENDCG
        }
    }
}
