Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.1)) = 0.05
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offsetX = float2(_OutlineThickness, 0);
                float2 offsetY = float2(0, _OutlineThickness);
                float2 offsetDiag1 = float2(_OutlineThickness, _OutlineThickness);
                float2 offsetDiag2 = float2(_OutlineThickness, -_OutlineThickness);

                fixed4 original = tex2D(_MainTex, i.uv) * i.color;
                fixed4 outline = _OutlineColor;

                // Sample neighboring pixels
                fixed4 sample1 = tex2D(_MainTex, i.uv + offsetX);
                fixed4 sample2 = tex2D(_MainTex, i.uv - offsetX);
                fixed4 sample3 = tex2D(_MainTex, i.uv + offsetY);
                fixed4 sample4 = tex2D(_MainTex, i.uv - offsetY);
                fixed4 sample5 = tex2D(_MainTex, i.uv + offsetDiag1);
                fixed4 sample6 = tex2D(_MainTex, i.uv - offsetDiag1);
                fixed4 sample7 = tex2D(_MainTex, i.uv + offsetDiag2);
                fixed4 sample8 = tex2D(_MainTex, i.uv - offsetDiag2);

                // Determine if pixel is on the edge
                if (original.a == 0 && (sample1.a > 0 || sample2.a > 0 || sample3.a > 0 || sample4.a > 0 || sample5.a > 0 || sample6.a > 0 || sample7.a > 0 || sample8.a > 0))
                {
                    return outline;
                }
                else
                {
                    return original;
                }
            }
            ENDCG
        }
    }
}
