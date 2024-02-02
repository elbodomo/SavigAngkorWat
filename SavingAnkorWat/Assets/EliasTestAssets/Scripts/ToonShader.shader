Shader "Custom/ToonShader"
{
    Properties
    {
        _Albedo ("color", COLOR) = (1,1,1,1) 
        _Shades("Shades", Range(1,20)) = 3
        _SpeclSize("SpeclSize", float) = 5
        _SpeclShades("SpeclShades", Range(0,10)) = 1
        _SpeclStrength("SpeclStrength", Range(0,1)) = 0.5
        _ShadeStrength("LightStrength", Range(-1,1)) = -0.3
        _ColorStrength("ShadowStrength", Range(-1,1)) = -0.3
        _Shift("ShadeShift", Range(0,1)) = 0.7
        _ShiftSize("ShiftSize", Range(0,2)) = 0
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
                float3 posWorld : TEXCOORD1;
            };
            uniform float4 _LightColor0;
            uniform float4 _Albedo;
            uniform float _Shades;
            uniform float _SpeclSize;
            uniform float _SpeclShades;
            uniform float _SpeclStrength;
            uniform float _ShadeStrength;
            uniform float _ColorStrength;
            uniform float _Shift;
            uniform float _ShiftSize;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul(float4(v.normal, 0), unity_WorldToObject).xyz);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float cosineAngle = dot(normalize(i.worldNormal), normalize(_WorldSpaceLightPos0.xyz));
                float4 specl = 0;
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
                float3 normalDir = normalize(i.worldNormal); 
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                /*if(cosineAngle>_SpeclSize)
                {
                    specl = _SpeclStrength;
                }*/
                if(dot(normalDir,lightDir) < 0){
                    specl.xyz = 0;
                }
                else{
                    specl = pow(max(0.0,dot(reflect(-lightDir,normalDir),viewDir)), _SpeclSize) ;
                    specl = floor(specl * _SpeclShades)/_SpeclShades;
                    specl = specl * _SpeclStrength;
                    
                }
                
                cosineAngle = cosineAngle * _ShiftSize + _Shift;
                cosineAngle = max(cosineAngle,0);
                cosineAngle = floor(cosineAngle * _Shades)/_Shades;
                cosineAngle = min(cosineAngle,1);
                cosineAngle = cosineAngle + _ColorStrength;
                return _Albedo + specl +  (_LightColor0 * cosineAngle)*_ShadeStrength;   
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}
