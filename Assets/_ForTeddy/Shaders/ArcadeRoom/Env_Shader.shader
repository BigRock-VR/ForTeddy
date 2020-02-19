Shader "Custom/Env"
{
	//questo è lo standard shader che verrà usato per il gioco arcade



	Properties
	{
		_Color("Color", Color) = (1,1,1,1)				 //colore nel caso non ci sia una text HSV
		_MainTex("Albedo (RGB)", 2D) = "white" {}		 //texture da applicare
		[HDR] _Emission("Emission" ,2D) = "white" {}
		_Bump("Normal", 2D) = "bump" {}
		_Maos("MAOS", 2D) = "white" {}

		//per la sfera della saturazione
		
		_ColorStrenght("Color Strenght", Range(0,4)) = 1
		_EmissionColor("Emission Color", Color) = (1,1,1,1)				 
		_EmissionTex("Emission (RGB)", 2D) = "white" {}
		_EmissionStrenght("Emission Strenght", Range(0,4)) = 1

			/*per settare le variabili come globali vanno levate da properties
			_Position("World Position", vector) = (0,0,0,0)*/

			//_Radius("Sphere Radius", Range(1,100)) = 0
			//_Softness("Sphere Softness", Range(1,100)) = 0


	}
		SubShader
		{
			//viene renderizzato insieme ai materiali opachi e nella queue della geometry
			Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

		

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex, _EmissionTex, _Maos, _Bump;
		half3 _Emission;
		half _Glossiness, _Metallic, _ColorStrenght, _EmissionStrenght;
		fixed4 _Color, _EmissionColor;

		//vanno rese uniform per far si che siano richiamabili
		uniform float4 PlayerMask_Position;
		uniform half PlayerMask_Radius, PlayerMask_Softness;
		

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_EmissionTex;
			float3 worldPos;
			float2 uv_Bump;
			float2 uv_Maos;
		};




		void surf(Input i, inout SurfaceOutputStandard o)
		{
			//COLOR
			fixed4 c = tex2D(_MainTex, i.uv_MainTex);

			//GRAYSCALE
			//viene diviso per 3 canali in quanto li deve mediare (conviene moltiplicare per un numero finito piuttosto che dividere avendo infiniti numeri)
			half grayscale = (c.r, c.g, c.b) *0.333;
			//il mio fixed3 è formato da tre canali rgb che sono tutti in grayscale
			fixed3 c_g = (grayscale, grayscale, grayscale);
			//EMISSION	
			fixed4 e = tex2D(_EmissionTex, i.uv_EmissionTex) * _EmissionColor * _EmissionStrenght;


			half d = distance(PlayerMask_Position, i.worldPos);
			half sum = saturate((d - PlayerMask_Radius) / -PlayerMask_Softness);

			fixed4 M = tex2D(_Maos, i.uv_Maos);
			fixed4 lerpColor = lerp(fixed4(c_g, 1), c * _ColorStrenght, sum);
			fixed4 lerpEmission = lerp(fixed4(0, 0, 0, 0), e, sum);

			o.Albedo = lerpColor.rgb;
			o.Alpha = c.a;
			o.Metallic = M.r;
			o.Smoothness = M.b;
			o.Emission = _Emission + lerpEmission.rgb;
			o.Normal = UnpackNormal(tex2D(_Bump, i.uv_Bump));
		}
		ENDCG
		}
}