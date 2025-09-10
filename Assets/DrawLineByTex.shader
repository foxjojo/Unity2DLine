Shader "DrawLineByTex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineW("线宽",Range(0.01,0.00001)) = 0.001
        _Color("颜色",Color) = (0,0,0,1)
        _PointCount("数组长度",Int) = 0


        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Tags
        {
            "Queue"="Transparent"

            "RenderType"="Transparent"

        }
        ColorMask [_ColorMask]
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            sampler2D _MainTex;
            float _LineW;
            float4 _MainTex_TexelSize;
            float4 _Color;
            int _PointCount;
            // 点到线段距离函数
            float distancePointToLineSegment(float2 p, float2 lineStart, float2 lineEnd)
            {
                float2 lineDir = lineEnd - lineStart;
                float2 pointDir = p - lineStart;

                float lineLengthSq = dot(lineDir, lineDir);

                // 如果线段退化为点
                if (lineLengthSq < 1e-6)
                {
                    return length(pointDir);
                }

                // 计算投影参数
                float t = clamp(dot(pointDir, lineDir) / lineLengthSq, 0.0, 1.0);

                // 计算最近点
                float2 closestPoint = lineStart + t * lineDir;

                return length(p - closestPoint);
            }

            fixed4 frag(v2f f) : SV_Target
            {
                fixed4 col = fixed4(1, 1, 1, 0);
                if (_PointCount == 0)
                    return col;
                float step = 1.0 / _PointCount;
                for (int i = 0; i < _PointCount - 1; i++)
                {
                    float2 curP = tex2D(_MainTex, float2(i * step, 0.5));
                    float2 nextP = tex2D(_MainTex, float2((i + 1) * step, 0.5));
                    float area = distancePointToLineSegment(f.uv, curP, nextP);

                    if (area <= _LineW)
                    {
                        col = _Color;
                    }
                }

                return col;
            }
            ENDCG
        }
    }
}