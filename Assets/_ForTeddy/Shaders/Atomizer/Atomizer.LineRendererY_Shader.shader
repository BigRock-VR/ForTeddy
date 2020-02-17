// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atomizer/laserLineY"
{
	Properties
	{
		_NoiseGen("NoiseGen", Vector) = (0,0,0,0)
		_NoiseDir("NoiseDir", Vector) = (0,0,0,0)
		_NoiseSpeed("NoiseSpeed", Float) = 0
		[HDR]_2("2", Color) = (0,0,0,0)
		_m("m", Color) = (1,0,0,0)
		_Color("Color", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _2;
		uniform float4 _m;
		uniform float _Color;
		uniform float _NoiseSpeed;
		uniform float2 _NoiseDir;
		uniform float2 _NoiseGen;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_100_0 = pow( ( ( i.uv_texcoord.y - 0.1 ) * ( ( i.uv_texcoord.y - 0.9 ) * -4.0 ) ) , 0.51 );
			float mulTime122 = _Time.y * _NoiseSpeed;
			float2 uv_TexCoord117 = i.uv_texcoord * _NoiseGen;
			float2 panner121 = ( mulTime122 * _NoiseDir + uv_TexCoord117);
			float simplePerlin2D116 = snoise( panner121 );
			float clampResult128 = clamp( simplePerlin2D116 , 0.7 , 0.9 );
			float4 lerpResult141 = lerp( _2 , ( _m * temp_output_100_0 ) , ( _Color * clampResult128 ));
			o.Emission = lerpResult141.rgb;
			float temp_output_151_0 = ( ( i.uv_texcoord.x - 0.89 ) * -2.16 );
			float2 uv_TexCoord150 = i.uv_texcoord * float2( 2,2 ) + ( float2( -2,-2 ) * _Time.y );
			float simplePerlin2D152 = snoise( uv_TexCoord150 );
			o.Alpha = ( ( ( temp_output_100_0 * clampResult128 ) * temp_output_151_0 ) * pow( ( temp_output_151_0 + ( ( simplePerlin2D152 - temp_output_151_0 ) * 0.43 ) ) , 0.35 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
111;529;1025;568;4545.608;-72.80673;4.787261;True;False
Node;AmplifyShaderEditor.Vector2Node;144;-2915.761,2516.508;Float;False;Constant;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;-2,-2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;143;-2835.761,2708.508;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-3262.281,1233.155;Float;False;Property;_NoiseSpeed;NoiseSpeed;2;0;Create;True;0;0;False;0;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;118;-3260.769,941.3657;Float;False;Property;_NoiseGen;NoiseGen;0;0;Create;True;0;0;False;0;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;93;-2862.597,611.3213;Float;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;92;-2913.889,492.5093;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;145;-2867.761,2292.508;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;2,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-2611.761,2596.508;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-2771.761,1844.508;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;148;-2707.761,1972.508;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;0.89;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;150;-2627.761,2324.508;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-2781.026,262.2643;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;96;-2629.621,539.9973;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-2709.331,394.9444;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;122;-3085.394,1221.06;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;119;-3090.275,1088.016;Float;False;Property;_NoiseDir;NoiseDir;1;0;Create;True;0;0;False;0;0,0;-1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-3076.322,939.8539;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;149;-2515.761,1908.508;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;152;-2291.761,2308.508;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-2243.761,1940.508;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;-2.16;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-2474.802,544.6512;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;97;-2528.8,326.8884;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;121;-2783.021,1091.04;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;153;-1955.761,2132.508;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-2276.232,397.5873;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;116;-2571.668,1000.193;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;-1731.761,2132.508;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;128;-2312.555,1002.681;Float;True;3;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;100;-2108.53,391.9943;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.51;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-1491.761,1956.508;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-1647.53,616.15;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-2210.252,-3.253102;Float;False;Property;_Color;Color;5;0;Create;True;0;0;False;0;0;2.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;115;-2009.518,-170.5047;Float;False;Property;_m;m;4;0;Create;True;0;0;False;0;1,0,0,0;0,0.8448343,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;156;-1267.761,1924.508;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-1979.562,21.17291;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;142;-1679.604,-175.0949;Float;False;Property;_2;2;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-1171.291,936.1464;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-1667.453,268.1461;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;141;-1447.354,72.38376;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-347.1115,920.8867;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;21;-1089.95,195.8689;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Atomizer/laserLineY;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;146;0;144;0
WireConnection;146;1;143;0
WireConnection;150;0;145;0
WireConnection;150;1;146;0
WireConnection;96;0;92;2
WireConnection;96;1;93;0
WireConnection;122;0;120;0
WireConnection;117;0;118;0
WireConnection;149;0;147;1
WireConnection;149;1;148;0
WireConnection;152;0;150;0
WireConnection;151;0;149;0
WireConnection;98;0;96;0
WireConnection;97;0;94;2
WireConnection;97;1;95;0
WireConnection;121;0;117;0
WireConnection;121;2;119;0
WireConnection;121;1;122;0
WireConnection;153;0;152;0
WireConnection;153;1;151;0
WireConnection;99;0;97;0
WireConnection;99;1;98;0
WireConnection;116;0;121;0
WireConnection;154;0;153;0
WireConnection;128;0;116;0
WireConnection;100;0;99;0
WireConnection;155;0;151;0
WireConnection;155;1;154;0
WireConnection;123;0;100;0
WireConnection;123;1;128;0
WireConnection;156;0;155;0
WireConnection;130;0;131;0
WireConnection;130;1;128;0
WireConnection;157;0;123;0
WireConnection;157;1;151;0
WireConnection;129;0;115;0
WireConnection;129;1;100;0
WireConnection;141;0;142;0
WireConnection;141;1;129;0
WireConnection;141;2;130;0
WireConnection;158;0;157;0
WireConnection;158;1;156;0
WireConnection;21;2;141;0
WireConnection;21;9;158;0
ASEEND*/
//CHKSM=7A3A3676F85CE5D07244580EEE876211990FCE7B