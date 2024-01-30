Shader "Custom/WaterShader"
{
    Properties{
        _MainTex("Texture", 2D)= "black"{}
        _Texture2("Texture", 2D)= "black"{}
        _Speed1("Speed", Range(0,10)) = 1
        _Speed2("Speed", Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            CGPROGRAM

            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform sampler2D _Texture2;
            uniform float4 _Texture2_ST;
            uniform float1 _Speed1;
            uniform float1 _Speed2;
           struct vIN{
                float4 pos : POSITION;
                float4 tex : TEXCOORD0;
            };
 
            struct vOut{
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(1)
                float4 tex : TEXCOORD0;
                
            };

            vOut VS(vIN input)
            {
                vOut output;
                output.pos = UnityObjectToClipPos(input.pos);
                output.tex = input.tex;
                UNITY_TRANSFER_FOG(output,output.pos);
                return output;
            }
            float4 PS(vOut input) : COLOR
            {   
                float4 TexMain = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + (_MainTex_ST.zw + float2(_SinTime.x * _Speed1,_SinTime.y * _Speed1)));
                float4 Tex2 = tex2D(_Texture2, input.tex.xy *_Texture2_ST.xy + (_Texture2_ST.zw - float2(_SinTime.y * _Speed2,_SinTime.x * _Speed2)));
                float4 o = TexMain * Tex2;
                UNITY_APPLY_FOG(input.fogCoord,o);
                return o;
            }

            ENDCG
        }    
    }
    Fallback "VertexLit"
}

