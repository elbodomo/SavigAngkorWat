Shader "Custom/ToonShader"
{
    Properties
    {
        _Albedo ("color", COLOR) = (1,1,1,1) 
        _Shades("Shades", Range(1,20)) = 3
        _ShadeStrength("Strength", Range(0,1)) = 0.3
        _Shift("ShadeShift", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            float4 _Albedo;
            float _Shades;
            float _ShadeStrength;
            float _Shift;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float cosineAngle = dot(normalize(i.worldNormal), normalize(_WorldSpaceLightPos0.xyz));
                float specl = 0;
                if(cosineAngle>0.9)
                {
                    specl = 0.5;
                }
                cosineAngle = cosineAngle + _Shift;
                cosineAngle = max(cosineAngle,0.3);
                cosineAngle = floor(cosineAngle * _Shades)/_Shades;
                cosineAngle = min(cosineAngle,1.0);
                return _Albedo * cosineAngle * _ShadeStrength + specl;   
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}
