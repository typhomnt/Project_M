Shader "Unlit/Gate"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_GateStatus("Gate Status", Range(0, 1)) = 1.0
		_MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _GateStatus;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float alpha1 = abs(0.5 - frac((i.uv.y + _Time.x * 2) * 25));
				float alpha2 = abs(0.5 - frac((i.uv.x) * 25));
				float d = max(abs(0.5 - i.uv.x), abs(0.5 - i.uv.y));
				d = pow(d, 4);
				fixed4 col = _Color;
				col.a = (alpha1 * alpha2 + d) * max(0, _GateStatus) * 20;
                return col;
            }
            ENDCG
        }
    }
}
