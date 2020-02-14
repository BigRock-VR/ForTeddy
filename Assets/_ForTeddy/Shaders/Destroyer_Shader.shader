Shader "Custom/Destroyer"
{
    Properties
    {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_DissolveTex ("Dissolve Texture", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_DissolveScale ("Dissolve Progression", Range(0.0, 1.0)) = 0.0
		_EmissionScale ("Emission Scale", Range(0.0, 1.0)) = 0.0
		_GlowIntensity ("Glow Intensity", Range(0.0, 5.0)) = 0.0
		_GlowScale ("Glow Size", Range(0.0, 5.0)) = 1.0
		_Glow ("Glow Color", Color) = (1, 1, 1, 1)
		_GlowEnd ("Glow End Color", Color) = (1, 1, 1, 1)
		_GlowColFac ("Glow Colorshift", Range(0.01, 2.0)) = 0.75
		_DissolveStart("Dissolve Start Point", Vector) = (1, 1, 1, 1)
		_DissolveEnd("Dissolve End Point", Vector) = (0, 0, 0, 1)
		_DissolveBand("Dissolve Band Size", Float) = 0.25
		_isHitting("isHitting", Range(0.0, 1.0)) = 0.0
		_isDissolving("isDissolving", Range(0.0, 1.0)) = 0.0

		//MAOS
			_Maos("MAOS", 2D) = "white" {}
			_Emission("EmiTex", 2D) = "white" {}

    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Fade" }
        LOD 200

		Cull Off

		Pass 
		{
			
			ZWrite On
			ColorMask 0
		}

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
		#pragma vertex vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		// Texture
        sampler2D _MainTex;
		sampler2D _DissolveTex;
		sampler2D _BumpMap;
		sampler2D _Maos;
		

		// Parameter
		half _DissolveScale;
		half _EmissionScale;
		half _DissolveBand;
		half _GlowScale;
		half _GlowColFac;
		half _GlowIntensity;

		// Check State Variables
		half _isHitting;
		half _isDissolving;

		float4 _DissolveStart;
		float4 _DissolveEnd;
		float4 _Glow;
		float4 _GlowEnd;


		//Precompute dissolve direction.
		static float3 dDir = normalize(_DissolveEnd - _DissolveStart);

		//Precompute gradient start position.
		static float3 dissolveStartConverted = _DissolveStart - _DissolveBand * dDir;

		//Precompute reciprocal of band size.
		static float dBandFactor = 1.0f / _DissolveBand;

        struct Input
        {
            float2 uv_DissolveTex;
			float2 uv2_MainTex;
			float2 uv_BumpMap;
			float3 dGeometry;
			float2 uv_Maos;
			
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half dBase = -2.0f * _DissolveScale + 1.0f;

			//Read from noise texture.
			fixed4 dTex = tex2D(_DissolveTex, IN.uv_DissolveTex);

			// Read the main texture
			fixed4 mTex = tex2D(_MainTex, IN.uv2_MainTex);

			//Convert dissolve texture sample based on dissolve progression.
			half dTexRead = dTex.r + dBase;
			half dFinal = dTexRead + IN.dGeometry;

			//Clamp and set alpha.
			half alpha = clamp(dFinal, 0.0f, 1.0f);

			//Shift the computed raw alpha value based on the scale factor of the glow.
			//Scale the shifted value based on effect intensity.
			//half dPredict = (_GlowScale - dFinal) * _GlowIntensity;

			half dEmissionBase = -6.0f * _EmissionScale + 1.0f;
			half dEmissionRead = dTex.r + dEmissionBase;
			half dEmissionFinal = dEmissionRead + IN.dGeometry;

			half dPredict = (_GlowScale -  dEmissionFinal) * _GlowIntensity;
			//Change colour interpolation by adding in another factor controlling the gradient.
			//half dPredictCol = (_GlowScale * _GlowColFac - dFinal) * _GlowIntensity;
			half dPredictCol = (_GlowScale * _GlowColFac - dEmissionFinal); //* _GlowIntensity;
			//Calculate and clamp glow colour.
			fixed4 glowCol = dPredict * lerp(_Glow, _GlowEnd, clamp(dPredictCol, 0.0f, 1.0f));
			glowCol = clamp(glowCol, 0.0f, 1.0f);

			fixed4 M = tex2D(_Maos, IN.uv_Maos);

			
			o.Albedo = mTex.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Metallic = M.r;
			o.Smoothness = M.b;
			

			if (_isDissolving == 0) 
			{
				o.Alpha = 1;
			}
			else 
			{
				o.Alpha = alpha;
			}

			if (_isHitting == 0) 
			{
				return;
			}

			o.Emission = glowCol;
        }

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			//Calculate geometry-based dissolve coefficient.
			//Compute top of dissolution gradient according to dissolve progression.
			float3 dPoint = lerp(dissolveStartConverted, _DissolveEnd, _DissolveScale);

			//Project vector between current vertex and top of gradient onto dissolve direction.
			//Scale coefficient by band (gradient) size.
			if (_isHitting == 0) {
				return;
			}
			o.dGeometry = dot(v.vertex - dPoint, dDir) * dBandFactor;
			//Combine texture factor with geometry coefficient from vertex routine.
		}

        ENDCG
    }
    FallBack "Diffuse"
}
