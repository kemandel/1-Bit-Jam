Shader "Unlit/ColorSwap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float3 _Color_A;
            float3 _Color_B;
            float _Swap_Line_X; // 0 to 1

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 rgba = tex2D(_MainTex, i.uv);
                float3 col = rgba;
                float a = rgba.a;

                float3 d1x = length(_Color_A.x - _Color_B.x);
                float3 d1y = length(_Color_A.y - _Color_B.y);
                float3 d1z = length(_Color_A.z - _Color_B.z);

                float3 d2x = length(_Color_A.x - col.x);
                float3 d2y = length(_Color_A.y - col.y);
                float3 d2z = length(_Color_A.z - col.z);
                
                float r = _Color_B.x + (_Color_A.x - _Color_B.x) * (d2x/d1x);
                float g = _Color_B.y + (_Color_A.y - _Color_B.y) * (d2y/d1y);
                float b = _Color_B.z + (_Color_A.z - _Color_B.z) * (d2z/d1z);

                return (i.uv.x < _Swap_Line_X) ? fixed4(r, g, b, a) : rgba;
            }

            ENDCG
        }
    }
}
