Shader "Custom/glitch"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_t ("t", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _t;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

		#define AMPLITUDE 0.1
		#define SPEED 5.0
		#define PI 3.14159

		fixed4 rgbShift(float2 p, float4 shift, float offset_amount) {
			shift *= 2.0*shift.w - 1.0;
			float2 rs = p + float2(shift.x + offset_amount, 0);
			float2 gs = p + float2(shift.y + offset_amount, 0);
			float2 bs = p + float2(shift.z + offset_amount, 0);

			float3 upper = 1;
			float3 lower = 0;

			float r = tex2D(_MainTex, rs).x;	
			float g = tex2D(_MainTex, gs).y;
			float b = tex2D(_MainTex, bs).z;

			return fixed4(r, g, b, 1.0);
		}

		float hash11(float p)
		{
			p = frac(p * .1031);
			p *= p + 33.33;
			p *= p + p;
			return frac(p);
		}

		float hash21(float2 p) {
			float3 p3 = frac(float3(p.xyx) * .1031);
			p3 += dot(p3, p3.yzx + 33.33);
			return frac((p3.x + p3.y) * p3.z);
		}

		float3 hash33(float3 p3)
		{
			p3 = frac(p3 * float3(.1031, .1030, .0973));
			p3 += dot(p3, p3.yxz + 33.33);
			return frac((p3.xxy + p3.yxx)*p3.zyx);
		}

		float4 vec4pow(float4 v, float p) {
			return float4(pow(v.x, p), pow(v.y, p), pow(v.z, p), v.w);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float num_buckets = int(sin(_t * PI) * 10) + 1;
			float discretized = float(int(IN.uv_MainTex.y * num_buckets)) / num_buckets;
			float discretized_t = float(int(_t * 20)) / 20;
			
			bool offset = hash21(float2(discretized_t, discretized)) > 0.5;
			float offset_amount = offset ? hash11(discretized) : 0;

			float4 shift = vec4pow(hash21(float2(SPEED * _t, 2.0 * SPEED * _t / 25.0)), 8.0)
				* float4(float3(AMPLITUDE, AMPLITUDE, AMPLITUDE) * hash33(float3(_t, _t * 2, _t / 2)), (hash11( _t) - 0.5) * AMPLITUDE);
			fixed4 c = rgbShift(IN.uv_MainTex, shift, offset_amount);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
