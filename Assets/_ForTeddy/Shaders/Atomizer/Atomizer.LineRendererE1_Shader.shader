// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atomizer/laserLineE1"
{
	Properties
	{
		_NoiseDir("NoiseDir", Vector) = (1,0,0,0)
		_NoiseGenE("NoiseGenE", Vector) = (0.1,0.1,0,0)
		_NoiseSpeed("NoiseSpeed", Float) = -0.71
		[HDR]_Color1("Color 1", Color) = (0,0,0,0)
		_Texture0("Texture 0", 2D) = "white" {}
		_TimeMulti("TimeMulti", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha , DstColor Zero
		
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

		uniform float4 _Color1;
		uniform float _NoiseSpeed;
		uniform float2 _NoiseDir;
		uniform float2 _NoiseGenE;
		uniform sampler2D _Texture0;
		uniform float _TimeMulti;


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
			float mulTime122 = _Time.y * _NoiseSpeed;
			float2 uv_TexCoord117 = i.uv_texcoord * _NoiseGenE;
			float2 panner121 = ( mulTime122 * _NoiseDir + uv_TexCoord117);
			float simplePerlin2D116 = snoise( panner121 );
			float clampResult233 = clamp( simplePerlin2D116 , -0.25 , 1.0 );
			float temp_output_166_0 = ( clampResult233 + ( ( i.uv_texcoord.y - 0.06 ) * ( ( i.uv_texcoord.y - 0.93 ) * -4.0 ) ) );
			o.Emission = ( _Color1 * temp_output_166_0 ).rgb;
			float2 uv_TexCoord219 = i.uv_texcoord * float2( 1,0.5 );
			float temp_output_4_0_g1 = 6.0;
			float temp_output_5_0_g1 = 1.0;
			float2 appendResult7_g1 = (float2(temp_output_4_0_g1 , temp_output_5_0_g1));
			float totalFrames39_g1 = ( temp_output_4_0_g1 * temp_output_5_0_g1 );
			float2 appendResult8_g1 = (float2(totalFrames39_g1 , temp_output_5_0_g1));
			float mulTime203 = _Time.y * _TimeMulti;
			float clampResult42_g1 = clamp( 0.0 , 0.0001 , ( totalFrames39_g1 - 1.0 ) );
			float temp_output_35_0_g1 = frac( ( ( mulTime203 + clampResult42_g1 ) / totalFrames39_g1 ) );
			float2 appendResult29_g1 = (float2(temp_output_35_0_g1 , ( 1.0 - temp_output_35_0_g1 )));
			float2 temp_output_15_0_g1 = ( ( uv_TexCoord219 / appendResult7_g1 ) + ( floor( ( appendResult8_g1 * appendResult29_g1 ) ) / appendResult7_g1 ) );
			float temp_output_179_0 = ( ( i.uv_texcoord.x - 0.89 ) * -2.16 );
			float2 uv_TexCoord183 = i.uv_texcoord * float2( 2,2 ) + ( float2( -2,-2 ) * _Time.y );
			float simplePerlin2D182 = snoise( uv_TexCoord183 );
			o.Alpha = ( ( ( tex2D( _Texture0, temp_output_15_0_g1 ).a * ( ( temp_output_166_0 * ( ( i.uv_texcoord.y - 0.0 ) * ( ( i.uv_texcoord.y - 1.03 ) * -4.0 ) ) ) * temp_output_179_0 ) ) * ( ( pow( ( i.uv_texcoord.x - 0.0 ) , 0.82 ) * pow( ( temp_output_179_0 + ( ( simplePerlin2D182 - temp_output_179_0 ) * 0.43 ) ) , 0.35 ) ) * 1.65 ) ) * 2.0 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
177;457;1025;562;3050.592;41.38363;2.205706;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;187;-2480,2768;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;185;-2560,2576;Float;False;Constant;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;-2,-2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;177;-2416,1904;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;176;-2352,2032;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;0.89;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-2256,2656;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;184;-2512,2352;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;2,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;118;-2782.793,143.7464;Float;False;Property;_NoiseGenE;NoiseGenE;2;0;Create;True;0;0;False;0;0.1,0.1;0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;120;-2784.305,435.5355;Float;False;Property;_NoiseSpeed;NoiseSpeed;3;0;Create;True;0;0;False;0;-0.71;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;119;-2612.299,290.3966;Float;False;Property;_NoiseDir;NoiseDir;1;0;Create;True;0;0;False;0;1,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;183;-2272,2384;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;145;-2242.449,912.7745;Float;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;0.93;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;144;-2293.741,793.9625;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;122;-2607.418,423.4405;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;178;-2160,1968;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-2598.346,142.2346;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;121;-2305.046,293.4207;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;169;-2279.078,1300.691;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;146;-2160.878,563.7175;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;148;-2089.183,696.3978;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;0.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;147;-2009.473,841.4506;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;182;-1936,2368;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;-1888,2000;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;-2.16;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-2227.786,1419.503;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;1.03;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;172;-1994.81,1348.179;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;171;-2074.521,1203.126;Float;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;170;-2146.215,1070.446;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;190;-1600,2192;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;150;-1908.652,628.3417;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;-1854.654,846.1044;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-4;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;116;-2090.095,314.12;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;223;-1337.561,1594.767;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;222;-1273.561,1722.767;Float;False;Constant;_Float5;Float 5;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-565.9105,545.8395;Float;False;Property;_TimeMulti;TimeMulti;6;0;Create;True;0;0;False;0;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-1656.083,699.0406;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;174;-1893.989,1135.07;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;-1839.991,1352.833;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-4;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;233;-1837.469,337.0449;Float;True;3;0;FLOAT;0;False;1;FLOAT;-0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;-1376,2192;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;224;-1081.561,1658.767;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;-1641.42,1205.769;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;202;-501.5054,349.3553;Float;True;Property;_Texture0;Texture 0;5;0;Create;True;0;0;False;0;None;b21b0612d338c884187da112434ea7a6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;219;-449.2217,656.3529;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;203;-384.6646,549.7234;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;166;-1535.258,339.3141;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;193;-1136,2016;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-1074.438,525.6405;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;200;-194.1947,372.0826;Float;True;Flipbook;-1;;1;53c2488c220f6564ca6c90721ee16673;2,71,1,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;6;False;5;FLOAT;1;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.PowerNode;195;-912,1984;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;228;-850.4828,1672.303;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.82;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;180;-815.5303,995.6383;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;227;-658.8364,1653.818;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;209;207.8538,371.7458;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;230;-310.0188,1648.334;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;1.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;7.388607,951.6327;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;386.2545,944.1843;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;154;-1420.262,-59.49107;Float;False;Property;_Color1;Color 1;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;8,8,8,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;231;631.6511,944.2156;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;221;-797.1396,153.5458;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;21;1084.826,93.45029;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Atomizer/laserLineE1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;6;2;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;186;0;185;0
WireConnection;186;1;187;0
WireConnection;183;0;184;0
WireConnection;183;1;186;0
WireConnection;122;0;120;0
WireConnection;178;0;177;1
WireConnection;178;1;176;0
WireConnection;117;0;118;0
WireConnection;121;0;117;0
WireConnection;121;2;119;0
WireConnection;121;1;122;0
WireConnection;147;0;144;2
WireConnection;147;1;145;0
WireConnection;182;0;183;0
WireConnection;179;0;178;0
WireConnection;172;0;169;2
WireConnection;172;1;168;0
WireConnection;190;0;182;0
WireConnection;190;1;179;0
WireConnection;150;0;146;2
WireConnection;150;1;148;0
WireConnection;149;0;147;0
WireConnection;116;0;121;0
WireConnection;151;0;150;0
WireConnection;151;1;149;0
WireConnection;174;0;170;2
WireConnection;174;1;171;0
WireConnection;173;0;172;0
WireConnection;233;0;116;0
WireConnection;191;0;190;0
WireConnection;224;0;223;1
WireConnection;224;1;222;0
WireConnection;175;0;174;0
WireConnection;175;1;173;0
WireConnection;203;0;204;0
WireConnection;166;0;233;0
WireConnection;166;1;151;0
WireConnection;193;0;179;0
WireConnection;193;1;191;0
WireConnection;167;0;166;0
WireConnection;167;1;175;0
WireConnection;200;51;202;0
WireConnection;200;13;219;0
WireConnection;200;2;203;0
WireConnection;195;0;193;0
WireConnection;228;0;224;0
WireConnection;180;0;167;0
WireConnection;180;1;179;0
WireConnection;227;0;228;0
WireConnection;227;1;195;0
WireConnection;209;0;200;53
WireConnection;230;0;227;0
WireConnection;212;0;209;3
WireConnection;212;1;180;0
WireConnection;194;0;212;0
WireConnection;194;1;230;0
WireConnection;231;0;194;0
WireConnection;221;0;154;0
WireConnection;221;1;166;0
WireConnection;21;2;221;0
WireConnection;21;9;231;0
ASEEND*/
//CHKSM=F1EEAF7495237B41F97829652FA56EC79C56C0F5