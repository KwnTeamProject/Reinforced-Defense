Shader "Custom/2DOutlineGlow_Fixed"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineThickness ("Outline Thickness (px)", Float) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off

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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // 자동으로 제공됨
            fixed4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float alpha = 0.0;
                float2 offset = _MainTex_TexelSize.xy * _OutlineThickness;

                // 8방향 주변 픽셀 샘플링
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        float2 sampleUV = uv + float2(x, y) * offset;
                        fixed4 sample = tex2D(_MainTex, sampleUV);
                        alpha = max(alpha, sample.a);
                    }
                }

                fixed4 col = tex2D(_MainTex, uv);
                if (col.a < 0.1 && alpha > 0.1)
                {
                    return _OutlineColor;
                }

                return col;
            }
            ENDCG
        }
    }
}