// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Atomizer/laserLineB"
{
	Properties
	{
		[HDR]_Color4("Color 4", Color) = (0,0.9888339,1,0)
		[HDR]_Color3("Color 3", Color) = (0,0.04619002,1,0)
		_PowerFloat("PowerFloat", Float) = 0
		_NoiseGen("NoiseGen", Vector) = (0,0,0,0)
		_NoiseDir("NoiseDir", Vector) = (0,0,0,0)
		_NoiseSpeed("NoiseSpeed", Float) = 0
		[HDR]_NoiseCol1("NoiseCol1", Color) = (0,0,0,0)
		[HDR]_NoiseCol2("NoiseCol2", Color) = (0,0,0,0)
		[HDR]_NoiseMulti("NoiseMulti", Float) = 0
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

		uniform float4 _Color4;
		uniform float4 _Color3;
		uniform float _PowerFloat;
		uniform float4 _NoiseCol1;
		uniform float4 _NoiseCol2;
		uniform float _NoiseSpeed;
		uniform float2 _NoiseDir;
		uniform float2 _NoiseGen;
		uniform float _NoiseMulti;


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
			float4 lerpResult99 = lerp( _Color4 , _Color3 , pow( ( ( i.uv_texcoord.y - -0.02 ) * ( ( i.uv_texcoord.y - 1.02 ) * -4.0 ) ) , _PowerFloat ));
			float mulTime120 = _Time.y * _NoiseSpeed;
			float2 uv_TexCoord116 = i.uv_texcoord * _NoiseGen;
			float2 panner117 = ( mulTime120 * _NoiseDir + uv_TexCoord116);
			float simplePerlin2D115 = snoise( panner117 );
			float4 lerpResult122 = lerp( _NoiseCol1 , _NoiseCol2 , simplePerlin2D115);
			float temp_output_126_0 = ( simplePerlin2D115 * _NoiseMulti );
			float4 lerpResult125 = lerp( lerpResult99 , lerpResult122 , temp_output_126_0);
			o.Emission = lerpResult125.rgb;
			float temp_output_136_0 = ( ( i.uv_texcoord.x - 0.89 ) * -2.16 );
			float2 uv_TexCoord135 = i.uv_texcoord * float2( 2,2 ) + ( float2( -2,-2 ) * _Time.y );
			float simplePerlin2D137 = snoise( uv_TexCoord135 );
			o.Alpha = ( ( ( temp_output_126_0 * temp_output_136_0 ) * ( pow( ( i.uv_texcoord.x - 0.0 ) , 0.82 ) * pow( ( temp_output_136_0 + ( ( simplePerlin2D137 - temp_output_136_0 ) * 0.43 ) ) , 0.35 ) ) ) * 2.03 );
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
177;615;1025;568;1129.17;-29.54975;2.68354;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;128;-1908.362,2593.572;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;129;-1988.362,2401.573;Float;False;Constant;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;-2,-2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;130;-1940.362,2177.572;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;2,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-1684.362,2481.572;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;132;-1844.362,1729.573;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;133;-1780.362,1857.573;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;0.89;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;134;-1588.362,1793.573;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;135;-1700.362,2209.573;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-1316.362,1825.573;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;-2.16;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;137;-1364.362,2193.572;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;118;-2095.462,279.651;Float;False;Property;_NoiseGen;NoiseGen;3;0;Create;True;0;0;False;0;0,0;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;121;-1892.414,617.8986;Float;False;Property;_NoiseSpeed;NoiseSpeed;5;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1891.34,-11.27954;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;1.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;-1885.095,282.0511;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;120;-1709.014,574.2316;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;119;-1897.855,434.1249;Float;False;Property;_NoiseDir;NoiseDir;4;0;Create;True;0;0;False;0;0,0;-1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-1942.633,-130.0913;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;138;-1028.362,2017.573;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;145;-858.4955,1470.416;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;144;-794.4955,1598.416;Float;False;Constant;_Float5;Float 5;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;117;-1585.787,287.7844;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;-804.3619,2017.573;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;78;-1658.365,-82.60352;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1680.031,-224.242;Float;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;False;0;-0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-1751.726,-356.9222;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;81;-1498.501,-292.2982;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-1217.227,581.1737;Float;False;Property;_NoiseMulti;NoiseMulti;8;1;[HDR];Create;True;0;0;False;0;0;1.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;115;-1357.551,290.3577;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;146;-602.4955,1534.416;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;140;-564.3619,1841.573;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1503.546,-77.94952;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1353.556,-235.8547;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-1334.116,-125.3532;Float;False;Property;_PowerFloat;PowerFloat;2;0;Create;True;0;0;False;0;0;1.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;147;-371.4172,1547.952;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.82;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-1030.24,421.0084;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;141;-340.3618,1809.573;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-179.7709,1529.467;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-243.8921,821.2112;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;124;-1032.64,237.9069;Float;False;Property;_NoiseCol2;NoiseCol2;7;1;[HDR];Create;True;0;0;False;0;0,0,0,0;5.615686,5.992157,0.03137255,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;83;-1167.925,-239.1085;Float;True;2;0;FLOAT;0;False;1;FLOAT;2.08;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;100;-1164.009,-423.446;Float;False;Property;_Color3;Color 3;1;1;[HDR];Create;True;0;0;False;0;0,0.04619002,1,0;0.1411765,2.996078,2.572549,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;123;-1043.313,67.78154;Float;False;Property;_NoiseCol1;NoiseCol1;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;5.992157,4.737255,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;101;-1162.874,-621.4581;Float;False;Property;_Color4;Color 4;0;1;[HDR];Create;True;0;0;False;0;0,0.9888339,1,0;0,2.964706,2.996078,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;99;-864.6522,-341.242;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;580.2877,805.9515;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;122;-725.9244,255.0742;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;125;-41.89722,-5.351868;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;862.0165,788.9916;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;2.03;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;21;1143.885,41.13002;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Atomizer/laserLineB;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;131;0;129;0
WireConnection;131;1;128;0
WireConnection;134;0;132;1
WireConnection;134;1;133;0
WireConnection;135;0;130;0
WireConnection;135;1;131;0
WireConnection;136;0;134;0
WireConnection;137;0;135;0
WireConnection;116;0;118;0
WireConnection;120;0;121;0
WireConnection;138;0;137;0
WireConnection;138;1;136;0
WireConnection;117;0;116;0
WireConnection;117;2;119;0
WireConnection;117;1;120;0
WireConnection;139;0;138;0
WireConnection;78;0;76;2
WireConnection;78;1;75;0
WireConnection;81;0;77;2
WireConnection;81;1;79;0
WireConnection;115;0;117;0
WireConnection;146;0;145;1
WireConnection;146;1;144;0
WireConnection;140;0;136;0
WireConnection;140;1;139;0
WireConnection;80;0;78;0
WireConnection;82;0;81;0
WireConnection;82;1;80;0
WireConnection;147;0;146;0
WireConnection;126;0;115;0
WireConnection;126;1;127;0
WireConnection;141;0;140;0
WireConnection;148;0;147;0
WireConnection;148;1;141;0
WireConnection;142;0;126;0
WireConnection;142;1;136;0
WireConnection;83;0;82;0
WireConnection;83;1;114;0
WireConnection;99;0;101;0
WireConnection;99;1;100;0
WireConnection;99;2;83;0
WireConnection;143;0;142;0
WireConnection;143;1;148;0
WireConnection;122;0;123;0
WireConnection;122;1;124;0
WireConnection;122;2;115;0
WireConnection;125;0;99;0
WireConnection;125;1;122;0
WireConnection;125;2;126;0
WireConnection;149;0;143;0
WireConnection;21;2;125;0
WireConnection;21;9;149;0
ASEEND*/
//CHKSM=FB9EE23785216C120D3B5ACB7748FB116AE8C07C