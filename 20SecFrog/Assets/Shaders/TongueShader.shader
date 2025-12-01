Shader "Custom/TongueShader"
{
    Properties
    {
        _MainTex ("Tongue Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _EndRoundness ("End Roundness", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float linePos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _EndRoundness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.linePos = v.color.a;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                float distFromCenter = abs(i.uv.y - 0.5) * 2.0;
                
                float endFactor = 0.0;
                
                // Round the start (origin end)
                if (i.uv.x < _EndRoundness)
                {
                    float normalizedPos = i.uv.x / _EndRoundness;
                    endFactor = max(endFactor, distFromCenter - sqrt(1.0 - (1.0 - normalizedPos) * (1.0 - normalizedPos)));
                }
                
                // Round the tip end
                if (i.uv.x > (1.0 - _EndRoundness))
                {
                    float normalizedPos = (1.0 - i.uv.x) / _EndRoundness;
                    endFactor = max(endFactor, distFromCenter - sqrt(1.0 - (1.0 - normalizedPos) * (1.0 - normalizedPos)));
                }
                
                if (endFactor > 0.0)
                {
                    discard;
                }
                
                float4 texMask = tex2D(_MainTex, i.uv);
                fixed4 finalColor = lerp(_Color, texMask, i.uv.x);
                
                float edgeAlpha = 1.0 - smoothstep(0.8, 1.0, distFromCenter);
                finalColor.a *= edgeAlpha;
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    Fallback "Sprites/Default"
}