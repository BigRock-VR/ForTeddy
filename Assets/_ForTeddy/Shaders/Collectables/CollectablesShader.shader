// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CollectablesShader"
{
	Properties
	{
		_FresnelPower("FresnelPower", Float) = 0.78
		[HDR]_EyeColor("EyeColor", Color) = (0,0,0,0)
		_ExternalFresnelScale("ExternalFresnelScale", Float) = 0.42
		[HDR]_TentaclesColor("TentaclesColor", Color) = (0.09019613,1,0,0)
		_ExternalFresnelPower("ExternalFresnelPower", Float) = 0.43
		_NoiseAlpha("NoiseAlpha", 2D) = "white" {}
		_NoiseColor("NoiseColor", Color) = (0.4433962,0.4433962,0.4433962,0)
		_Normals1("Normals1", 2D) = "white" {}
		_Normals2("Normals2", 2D) = "white" {}
		_Alpha1("Alpha1", 2D) = "white" {}
		_Alpha2("Alpha2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normals1;
		uniform float4 _Normals1_ST;
		uniform sampler2D _Normals2;
		uniform float4 _Normals2_ST;
		uniform float _ExternalFresnelScale;
		uniform float _ExternalFresnelPower;
		uniform float4 _TentaclesColor;
		uniform sampler2D _Alpha1;
		uniform sampler2D _Alpha2;
		uniform float4 _EyeColor;
		uniform float _FresnelPower;
		uniform float4 _NoiseColor;
		uniform sampler2D _NoiseAlpha;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime157 = _Time.y * 3.0;
			v.vertex.xyz += ( float3(0.3,0.4,0) * (0.0 + (sin( mulTime157 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_Normals1 = i.uv_texcoord * _Normals1_ST.xy + _Normals1_ST.zw;
			float2 panner101 = ( 1.0 * _Time.y * float2( 0,0 ) + uv0_Normals1);
			float mulTime109 = _Time.y * -1.0;
			float cos107 = cos( mulTime109 );
			float sin107 = sin( mulTime109 );
			float2 rotator107 = mul( panner101 - float2( 0.5,0.5 ) , float2x2( cos107 , -sin107 , sin107 , cos107 )) + float2( 0.5,0.5 );
			float2 uv0_Normals2 = i.uv_texcoord * _Normals2_ST.xy + _Normals2_ST.zw;
			float2 panner114 = ( 1.0 * _Time.y * float2( 0,0 ) + uv0_Normals2);
			float mulTime117 = _Time.y * -1.0;
			float cos115 = cos( mulTime117 );
			float sin115 = sin( mulTime117 );
			float2 rotator115 = mul( panner114 - float2( 0.5,0.5 ) , float2x2( cos115 , -sin115 , sin115 , cos115 )) + float2( 0.5,0.5 );
			float mulTime133 = _Time.y * 6.0;
			float clampResult160 = clamp( (0.0 + (sin( mulTime133 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) , 0.05 , 0.9 );
			float3 lerpResult134 = lerp( UnpackScaleNormal( tex2D( _Normals1, rotator107 ), 3.0 ) , UnpackScaleNormal( tex2D( _Normals2, rotator115 ), 1.02 ) , clampResult160);
			float3 Normals135 = lerpResult134;
			o.Normal = Normals135;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV10 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode10 = ( 0.0 + _ExternalFresnelScale * pow( 1.0 - fresnelNdotV10, _ExternalFresnelPower ) );
			float2 panner83 = ( 1.0 * _Time.y * float2( 0,0 ) + i.uv_texcoord);
			float mulTime111 = _Time.y * -1.0;
			float cos110 = cos( mulTime111 );
			float sin110 = sin( mulTime111 );
			float2 rotator110 = mul( panner83 - float2( 0.5,0.5 ) , float2x2( cos110 , -sin110 , sin110 , cos110 )) + float2( 0.5,0.5 );
			float2 panner121 = ( 1.0 * _Time.y * float2( 0,0 ) + i.uv_texcoord);
			float mulTime120 = _Time.y * -1.0;
			float cos122 = cos( mulTime120 );
			float sin122 = sin( mulTime120 );
			float2 rotator122 = mul( panner121 - float2( 0.5,0.5 ) , float2x2( cos122 , -sin122 , sin122 , cos122 )) + float2( 0.5,0.5 );
			float mulTime128 = _Time.y * 6.0;
			float clampResult161 = clamp( (0.0 + (sin( mulTime128 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) , 0.05 , 0.9 );
			float lerpResult124 = lerp( tex2D( _Alpha1, rotator110 ).a , tex2D( _Alpha2, rotator122 ).a , clampResult161);
			float alphalerp146 = ( lerpResult124 * 25.0 );
			float4 temp_output_46_0 = ( ( fresnelNode10 * _TentaclesColor ) * alphalerp146 );
			float4 albedotentacles144 = temp_output_46_0;
			o.Albedo = albedotentacles144.rgb;
			float mulTime187 = _Time.y * 7.0;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + (1.35 + (sin( mulTime187 ) - -1.0) * (1.85 - 1.35) / (1.0 - -1.0)) * pow( 1.0 - fresnelNdotV1, _FresnelPower ) );
			float2 panner89 = ( 1.0 * _Time.y * float2( -0.3,0 ) + i.uv_texcoord);
			float4 Emission137 = ( ( _EyeColor * ( 1.0 - fresnelNode1 ) ) + ( temp_output_46_0 + ( _NoiseColor * 2.76 * tex2D( _NoiseAlpha, panner89 ) ) ) );
			o.Emission = Emission137.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
212;499;1338;520;3264.213;-74.70624;2.806904;True;False
Node;AmplifyShaderEditor.CommentaryNode;142;-3514.259,421.7312;Inherit;False;726.6002;266.2001;Lerp time;4;127;126;128;162;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-3505.817,534.374;Inherit;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;140;-3513.602,-79.18362;Inherit;False;1289.964;485.9479;AlphaLerp;12;81;123;122;110;121;120;111;83;119;82;183;184;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;82;-3188.513,-10.63679;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;119;-3192.421,182.0138;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;128;-3358.658,475.963;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;83;-2945.517,-1.856695;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;121;-2944.902,189.6628;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;120;-2943.101,303.9042;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;126;-3185.896,471.7311;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;111;-2936.931,123.6945;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;122;-2726.958,178.4844;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;184;-3455.507,140.884;Inherit;True;Property;_Alpha2;Alpha2;11;0;Create;True;0;0;False;0;None;42158e4b0fd2bf64598f61df81b58714;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;183;-3454.17,-48.26815;Inherit;True;Property;_Alpha1;Alpha1;10;0;Create;True;0;0;False;0;None;b4b2642c64a87ac498cc4a53d154a08a;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RotatorNode;110;-2750.077,37.33367;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;127;-3000.659,481.963;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;185;-2102.675,-31.59419;Inherit;False;726.6002;266.2001;Lerp time;4;189;188;187;186;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;148;-1604.375,1075.867;Inherit;False;1205.597;399.509;noise;8;88;89;90;96;93;97;149;150;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;123;-2547.218,156.9327;Inherit;True;Property;_venomAlpha2;venomAlpha2;8;0;Create;True;0;0;False;0;None;42158e4b0fd2bf64598f61df81b58714;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;186;-2094.233,81.04875;Inherit;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;161;-2702.979,486.122;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.05;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;88;-1554.375,1251.455;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;81;-2544.637,-29.18349;Inherit;True;Property;_venomAlpha;venomAlpha;7;0;Create;True;0;0;False;0;None;b4b2642c64a87ac498cc4a53d154a08a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;89;-1336.898,1248.8;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.3,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;151;-2475.431,-941.2722;Inherit;False;1927.937;812.7302;normalsLerp;21;105;112;100;113;117;114;101;133;109;132;118;106;107;115;103;131;116;134;135;160;163;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-2182.322,365.4168;Inherit;False;Constant;_Float19;Float 19;10;0;Create;True;0;0;False;0;25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;143;-1594.036,575.8787;Inherit;False;1124.668;475.2158;tentacles;7;147;46;7;10;8;11;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;187;-1947.073,22.63762;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;124;-2177.079,256.7693;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-2021.954,259.9135;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1573.159,697.2536;Inherit;False;Property;_ExternalFresnelPower;ExternalFresnelPower;5;0;Create;True;0;0;False;0;0.43;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;139;-1327.363,-49.39763;Inherit;False;835.7672;430.2878;Eye Color;5;6;5;2;4;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;105;-2425.431,-891.2722;Inherit;True;Property;_Normals1;Normals1;8;0;Create;True;0;0;False;0;None;1a09c8cb010240643bc8c1db6f35fd18;True;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SinOpNode;188;-1774.311,18.40575;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-1949.065,-377.9283;Inherit;False;Constant;_Float3;Float 3;13;0;Create;True;0;0;False;0;6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;112;-2379.365,-625.6729;Inherit;True;Property;_Normals2;Normals2;9;0;Create;True;0;0;False;0;None;6992c5329f875df4993ce62b1a32458e;True;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1564.69,625.8787;Inherit;False;Property;_ExternalFresnelScale;ExternalFresnelScale;3;0;Create;True;0;0;False;0;0.42;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;90;-1170.662,1180.046;Inherit;True;Property;_NoiseAlpha;NoiseAlpha;6;0;Create;True;0;0;False;0;cd460ee4ac5c1e746b7a734cc7cc64dd;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-1319.448,275.9908;Inherit;False;Property;_FresnelPower;FresnelPower;1;0;Create;True;0;0;False;0;0.78;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;113;-2093.577,-577.3779;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;189;-1589.074,28.63763;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;1.35;False;4;FLOAT;1.85;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;133;-1773.834,-373.3099;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;150;-878.2878,1349.239;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;10;-1333.435,618.9142;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-1313.595,777.8934;Inherit;False;Property;_TentaclesColor;TentaclesColor;4;1;[HDR];Create;True;0;0;False;0;0.09019613,1,0,0;0.01802243,0.1415094,0.09892768,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;-2126.182,-849.7228;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;146;-1895.781,262.4386;Inherit;False;alphalerp;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;93;-842.7565,1125.867;Inherit;False;Property;_NoiseColor;NoiseColor;7;0;Create;True;0;0;False;0;0.4433962,0.4433962,0.4433962,0;0.1886785,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;109;-1883.86,-734.7531;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-1137.799,154.0363;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1051.2,617.3943;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-835.9872,1285.606;Inherit;False;Constant;_Float18;Float 18;9;0;Create;True;0;0;False;0;2.76;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;147;-850.5074,709.7697;Inherit;False;146;alphalerp;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;114;-1867.797,-572.372;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;132;-1601.072,-377.5419;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;117;-1847.254,-460.4083;Inherit;False;1;0;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;149;-678.2878,1360.239;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;101;-1899.732,-844.2735;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-1681.349,-726.9987;Inherit;False;Constant;_Float20;Float 20;11;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;359.9607,555.973;Inherit;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-1646.744,-451.654;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;1.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;131;-1415.834,-367.3099;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-671.8591,642.1901;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;107;-1721.852,-839.3167;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;115;-1684.026,-563.8605;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;2;-850.862,158.0149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-1075.551,-9.68116;Inherit;False;Property;_EyeColor;EyeColor;2;1;[HDR];Create;True;0;0;False;0;0,0,0,0;4,0,0.09411765,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-634.7784,1222.376;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;-399.7742,870.0457;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;116;-1497.052,-584.2993;Inherit;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;None;None;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-695.9279,135.1269;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;103;-1517.657,-840.6442;Inherit;True;Property;_TextureSample2;Texture Sample 2;8;0;Create;True;0;0;False;0;None;None;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;157;498.4404,559.2645;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;160;-1192.445,-371.5262;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.05;False;2;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;156;657.92,557.6187;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;134;-1024.633,-615.3817;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-299.4124,356.1049;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-462.4174,641.337;Inherit;False;albedotentacles;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;159;775.4335,561.8959;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;154;585.684,393.7323;Inherit;False;Constant;_Vector0;Vector 0;13;0;Create;True;0;0;False;0;0.3,0.4,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-790.4949,-610.0502;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;137;-152.509,357.4682;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;955.5917,504.4529;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;988.2666,254.3513;Inherit;False;137;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;145;933.7162,87.9972;Inherit;False;144;albedotentacles;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;136;990.8353,185.8915;Inherit;False;135;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1197.2,163.8092;Float;False;True;2;ASEMaterialInspector;0;0;Standard;CollectablesShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;128;0;162;0
WireConnection;83;0;82;0
WireConnection;121;0;119;0
WireConnection;126;0;128;0
WireConnection;122;0;121;0
WireConnection;122;2;120;0
WireConnection;110;0;83;0
WireConnection;110;2;111;0
WireConnection;127;0;126;0
WireConnection;123;0;184;0
WireConnection;123;1;122;0
WireConnection;161;0;127;0
WireConnection;81;0;183;0
WireConnection;81;1;110;0
WireConnection;89;0;88;0
WireConnection;187;0;186;0
WireConnection;124;0;81;4
WireConnection;124;1;123;4
WireConnection;124;2;161;0
WireConnection;98;0;124;0
WireConnection;98;1;99;0
WireConnection;188;0;187;0
WireConnection;90;1;89;0
WireConnection;113;2;112;0
WireConnection;189;0;188;0
WireConnection;133;0;163;0
WireConnection;150;0;90;0
WireConnection;10;2;11;0
WireConnection;10;3;12;0
WireConnection;100;2;105;0
WireConnection;146;0;98;0
WireConnection;1;2;189;0
WireConnection;1;3;4;0
WireConnection;7;0;10;0
WireConnection;7;1;8;0
WireConnection;114;0;113;0
WireConnection;132;0;133;0
WireConnection;149;0;150;0
WireConnection;101;0;100;0
WireConnection;131;0;132;0
WireConnection;46;0;7;0
WireConnection;46;1;147;0
WireConnection;107;0;101;0
WireConnection;107;2;109;0
WireConnection;115;0;114;0
WireConnection;115;2;117;0
WireConnection;2;0;1;0
WireConnection;96;0;93;0
WireConnection;96;1;97;0
WireConnection;96;2;149;0
WireConnection;91;0;46;0
WireConnection;91;1;96;0
WireConnection;116;0;112;0
WireConnection;116;1;115;0
WireConnection;116;5;118;0
WireConnection;5;0;6;0
WireConnection;5;1;2;0
WireConnection;103;0;105;0
WireConnection;103;1;107;0
WireConnection;103;5;106;0
WireConnection;157;0;158;0
WireConnection;160;0;131;0
WireConnection;156;0;157;0
WireConnection;134;0;103;0
WireConnection;134;1;116;0
WireConnection;134;2;160;0
WireConnection;47;0;5;0
WireConnection;47;1;91;0
WireConnection;144;0;46;0
WireConnection;159;0;156;0
WireConnection;135;0;134;0
WireConnection;137;0;47;0
WireConnection;155;0;154;0
WireConnection;155;1;159;0
WireConnection;0;0;145;0
WireConnection;0;1;136;0
WireConnection;0;2;138;0
WireConnection;0;11;155;0
ASEEND*/
//CHKSM=B467297DDC5F2CEBD9B25C39974249A50712582C