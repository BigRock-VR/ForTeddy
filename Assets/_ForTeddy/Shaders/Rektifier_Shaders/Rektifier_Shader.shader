// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Rektifier_Shader"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 3
		_MainNoise_Texture("Main Noise_Texture", 2D) = "white" {}
		[HDR]_Main_Noise_Alpha_Color("Main_Noise_Alpha_Color", Color) = (0,0,0,0)
		_Second_Noise_Texture("Second_Noise_Texture", 2D) = "white" {}
		[HDR]_Second_Alpha_Noise_Color("Second_Alpha_Noise_Color", Color) = (0,0,0,0)
		[HDR]_FresnelColor("Fresnel Color", Color) = (0,0.0103929,1,0)
		_FlipBookLightningAlpha("FlipBook Lightning Alpha", 2D) = "white" {}
		_LightningTimeSpeed("Lightning Time Speed", Float) = 0
		[HDR]_LightningColor("Lightning Color", Color) = (0,0,0,0)
		_ExtrusionPoint("ExtrusionPoint", Float) = 0
		_ExtrusionAmount("Extrusion Amount", Range( -1 , 20)) = 0.5
		_SmoothstepMin("SmoothstepMin", Float) = 0.3
		_SmoothstepMax("SmoothstepMax", Float) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _ExtrusionPoint;
		uniform float _ExtrusionAmount;
		uniform float4 _FresnelColor;
		uniform float4 _Main_Noise_Alpha_Color;
		uniform sampler2D _MainNoise_Texture;
		uniform sampler2D _Second_Noise_Texture;
		uniform float4 _Second_Alpha_Noise_Color;
		uniform float _SmoothstepMin;
		uniform float _SmoothstepMax;
		uniform sampler2D _FlipBookLightningAlpha;
		uniform float4 _FlipBookLightningAlpha_ST;
		uniform float _LightningTimeSpeed;
		uniform float4 _LightningColor;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 VertexDisplaccement145 = ( ase_vertexNormal * max( ( sin( ( ( ase_vertex3Pos.y + _Time.x ) / _ExtrusionPoint ) ) / _ExtrusionAmount ) , 0.0 ) );
			v.vertex.xyz += VertexDisplaccement145;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float mulTime84 = _Time.y * 30.0;
			float mulTime85 = _Time.y * 5.0;
			float fresnelNdotV47 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode47 = ( 0.0 + (0.3 + (sin( mulTime84 ) - -1.0) * (1.0 - 0.3) / (1.0 - -1.0)) * pow( 1.0 - fresnelNdotV47, (1.25 + (sin( mulTime85 ) - -1.0) * (1.0 - 1.25) / (1.0 - -1.0)) ) );
			float2 temp_cast_0 = (3.0).xx;
			float2 uv_TexCoord22 = i.uv_texcoord * temp_cast_0;
			float cos24 = cos( radians( 110.0 ) );
			float sin24 = sin( radians( 110.0 ) );
			float2 rotator24 = mul( uv_TexCoord22 - float2( 0.5,0.5 ) , float2x2( cos24 , -sin24 , sin24 , cos24 )) + float2( 0.5,0.5 );
			float2 panner30 = ( 1.0 * _Time.y * float2( 1,1 ) + rotator24);
			float4 MainAlphaNoise104 = tex2D( _MainNoise_Texture, panner30 );
			float2 temp_cast_1 = (3.0).xx;
			float2 uv_TexCoord99 = i.uv_texcoord * temp_cast_1;
			float cos100 = cos( radians( 0.0 ) );
			float sin100 = sin( radians( 0.0 ) );
			float2 rotator100 = mul( uv_TexCoord99 - float2( 0.5,0.5 ) , float2x2( cos100 , -sin100 , sin100 , cos100 )) + float2( 0.5,0.5 );
			float2 panner101 = ( 1.0 * _Time.y * float2( 1,1 ) + rotator100);
			float4 SecondAlphaNoise109 = tex2D( _Second_Noise_Texture, panner101 );
			float4 PulsingFresnel111 = ( ( _FresnelColor * fresnelNode47 ) + ( _Main_Noise_Alpha_Color * MainAlphaNoise104 ) + ( SecondAlphaNoise109 * _Second_Alpha_Noise_Color ) );
			float4 temp_cast_2 = (_SmoothstepMin).xxxx;
			float4 temp_cast_3 = (_SmoothstepMax).xxxx;
			float2 uv0_FlipBookLightningAlpha = i.uv_texcoord * _FlipBookLightningAlpha_ST.xy + _FlipBookLightningAlpha_ST.zw;
			float2 panner76 = ( 0.0 * _Time.y * float2( 1,0 ) + uv0_FlipBookLightningAlpha);
			float temp_output_4_0_g1 = 2.0;
			float temp_output_5_0_g1 = 2.0;
			float2 appendResult7_g1 = (float2(temp_output_4_0_g1 , temp_output_5_0_g1));
			float totalFrames39_g1 = ( temp_output_4_0_g1 * temp_output_5_0_g1 );
			float2 appendResult8_g1 = (float2(totalFrames39_g1 , temp_output_5_0_g1));
			float mulTime64 = _Time.y * _LightningTimeSpeed;
			float clampResult42_g1 = clamp( (float)0 , 0.0001 , ( totalFrames39_g1 - 1.0 ) );
			float temp_output_35_0_g1 = frac( ( ( mulTime64 + clampResult42_g1 ) / totalFrames39_g1 ) );
			float2 appendResult29_g1 = (float2(temp_output_35_0_g1 , ( 1.0 - temp_output_35_0_g1 )));
			float2 temp_output_15_0_g1 = ( ( panner76 / appendResult7_g1 ) + ( floor( ( appendResult8_g1 * appendResult29_g1 ) ) / appendResult7_g1 ) );
			float4 smoothstepResult67 = smoothstep( temp_cast_2 , temp_cast_3 , tex2D( _FlipBookLightningAlpha, temp_output_15_0_g1 ));
			float4 FlipBookAlpha72 = smoothstepResult67;
			o.Emission = ( PulsingFresnel111 + ( FlipBookAlpha72 * _LightningColor ) ).rgb;
			float4 temp_cast_6 = (0.01).xxxx;
			float4 temp_cast_7 = (0.64).xxxx;
			float4 smoothstepResult124 = smoothstep( temp_cast_6 , temp_cast_7 , SecondAlphaNoise109);
			float4 temp_cast_8 = (0.01).xxxx;
			float4 temp_cast_9 = (0.64).xxxx;
			float4 smoothstepResult25 = smoothstep( temp_cast_8 , temp_cast_9 , MainAlphaNoise104);
			o.Alpha = ( smoothstepResult124 + smoothstepResult25 + FlipBookAlpha72 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
197;461;1338;558;2830.791;-1655.433;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;108;-2766.147,1101.237;Inherit;False;1578.54;671.5226;Second Alpha Noise;10;109;102;103;96;97;99;100;101;98;95;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;107;-2771.093,374.4476;Inherit;False;1637.936;689.5359;Main Alpha Noise;10;28;33;32;31;22;24;30;21;20;104;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-2573.362,1592.773;Inherit;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2575.308,948.9837;Inherit;False;Constant;_Float9;Float 9;1;0;Create;True;0;0;False;0;110;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-2716.147,1383.203;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-2721.093,656.4136;Inherit;False;Constant;_Float8;Float 8;1;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2484.365,637.9727;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RadiansOpNode;32;-2398.308,903.9835;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;110;-2761.889,-565.5466;Inherit;False;1539.257;920.1841;Pulsing Fresnel;23;111;49;34;52;113;53;35;114;106;47;115;93;92;89;90;88;86;91;87;84;85;82;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;31;-2458.795,776.7963;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RadiansOpNode;97;-2416.362,1593.773;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;99;-2479.419,1364.763;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;98;-2457.849,1477.586;Inherit;False;Constant;_Vector1;Vector 1;1;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;83;-2706.13,-210.1891;Float;False;Constant;_Float5;Float 5;13;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;24;-2217.536,634.7808;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;100;-2212.59,1361.571;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2711.889,-421.8022;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;30;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;94;-2775.785,1815.011;Inherit;False;1864.233;536.7843;FlipBook Lightnings;12;156;72;67;55;69;68;63;76;64;75;77;62;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;144;-2760.807,2392.299;Inherit;False;1626.276;483.0905;Vertex Normal Displacement;12;145;143;142;141;140;138;139;137;135;136;133;134;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;85;-2564.151,-208.5456;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;133;-2710.807,2570.3;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;21;-2571.757,424.4477;Inherit;True;Property;_MainNoise_Texture;Main Noise_Texture;5;0;Create;True;0;0;False;0;None;9789d23040cb1fb45ad60392430c3c15;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleTimeNode;84;-2575.302,-419.4549;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;101;-2014.081,1358.24;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TimeNode;134;-2710.807,2714.3;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;102;-2566.811,1151.237;Inherit;True;Property;_Second_Noise_Texture;Second_Noise_Texture;7;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;62;-2758.905,1871.011;Inherit;True;Property;_FlipBookLightningAlpha;FlipBook Lightning Alpha;10;0;Create;True;0;0;False;0;None;28b97cffd0582884582833f00dc93c48;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;30;-2019.028,631.45;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;86;-2409.439,-417.9708;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-2454.807,2698.3;Float;False;Property;_ExtrusionPoint;ExtrusionPoint;13;0;Create;True;0;0;False;0;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-2445.782,-73.74937;Float;False;Constant;_Float14;Float 14;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-2439.79,1948.24;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;75;-2171.107,2170.711;Inherit;False;Property;_LightningTimeSpeed;Lightning Time Speed;11;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-2435.532,-283.3571;Float;False;Constant;_Float10;Float 10;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;-2454.807,2602.3;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;20;-1753.562,595.002;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;103;-1748.616,1321.792;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;-2463.782,-143.7494;Float;False;Constant;_Float15;Float 15;13;0;Create;True;0;0;False;0;1.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;89;-2408.688,-208.3615;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-2435.532,-352.3581;Float;False;Constant;_Float11;Float 11;13;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;76;-2103.777,1959.873;Inherit;False;3;0;FLOAT2;1,1;False;2;FLOAT2;1,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;64;-1941.638,2174.568;Inherit;False;1;0;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;137;-2230.807,2602.3;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;-1442.941,1331.271;Inherit;False;SecondAlphaNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;92;-2291.782,-208.7485;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;63;-1958.57,2099.783;Inherit;False;Constant;_Int0;Int 0;4;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TFHCRemapNode;93;-2292.533,-418.358;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;104;-1388.16,594.5686;Inherit;False;MainAlphaNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;55;-1770.967,1909.021;Inherit;True;Flipbook;-1;;1;53c2488c220f6564ca6c90721ee16673;2,71,1,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;2;False;5;FLOAT;2;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.FresnelNode;47;-2100.863,-351.8229;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1540.443,2155.956;Inherit;False;Property;_SmoothstepMax;SmoothstepMax;16;0;Create;True;0;0;False;0;0.5;0.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;-2110.875,-179.3812;Inherit;False;Property;_Main_Noise_Alpha_Color;Main_Noise_Alpha_Color;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0.5254822,0.766161,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;106;-2082.719,-13.42651;Inherit;False;104;MainAlphaNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;-2083.732,85.31689;Inherit;False;109;SecondAlphaNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1539.9,2086.121;Inherit;False;Property;_SmoothstepMin;SmoothstepMin;15;0;Create;True;0;0;False;0;0.3;-0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;138;-2054.807,2602.3;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-2230.807,2698.3;Float;False;Property;_ExtrusionAmount;Extrusion Amount;14;0;Create;True;0;0;False;0;0.5;20;-1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;115;-2120.826,157.1852;Inherit;False;Property;_Second_Alpha_Noise_Color;Second_Alpha_Noise_Color;8;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0.4281969,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;-2057.007,-528.4901;Inherit;False;Property;_FresnelColor;Fresnel Color;9;1;[HDR];Create;True;0;0;False;0;0,0.0103929,1,0;1.30083,0,7.129738,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;67;-1322.659,1910.767;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;140;-1862.808,2602.3;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1843.383,-178.0486;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;-1827.303,87.30305;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-1828.949,-390.401;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;142;-1702.808,2602.3;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;141;-1702.808,2442.299;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;72;-1168.216,2148.56;Inherit;False;FlipBookAlpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-1670.555,-262.8287;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;123;288.5135,214.5468;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;271.2428,505.6987;Inherit;False;Constant;_Float7;Float 7;2;0;Create;True;0;0;False;0;0.64;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-1422.616,-267.4299;Inherit;False;PulsingFresnel;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;287.2428,434.6988;Inherit;False;Constant;_Float6;Float 6;2;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;212.2534,144.732;Inherit;False;109;SecondAlphaNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;526.2623,-85.31411;Inherit;False;72;FlipBookAlpha;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;80;530.6104,-16.54447;Inherit;False;Property;_LightningColor;Lightning Color;12;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.7028302,1,0.9171566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;122;272.5136,285.5467;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.64;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-1512.808,2475.3;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;229.2677,364.8741;Inherit;False;104;MainAlphaNoise;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;762.0905,-50.98075;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;25;469.9223,394.6317;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;666.406,-157.7608;Inherit;False;111;PulsingFresnel;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;124;471.1928,174.4798;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;145;-1382.818,2468.821;Inherit;False;VertexDisplaccement;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;687.8704,483.9855;Inherit;False;72;FlipBookAlpha;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;901.6796,394.9924;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;926.3843,-82.76704;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;156;-1084.306,1916.558;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;793.4763,597.0911;Inherit;False;145;VertexDisplaccement;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1092.757,190.9519;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Rektifier_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;1;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;3;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;28;0
WireConnection;32;0;33;0
WireConnection;97;0;96;0
WireConnection;99;0;95;0
WireConnection;24;0;22;0
WireConnection;24;1;31;0
WireConnection;24;2;32;0
WireConnection;100;0;99;0
WireConnection;100;1;98;0
WireConnection;100;2;97;0
WireConnection;85;0;83;0
WireConnection;84;0;82;0
WireConnection;101;0;100;0
WireConnection;30;0;24;0
WireConnection;86;0;84;0
WireConnection;77;2;62;0
WireConnection;135;0;133;2
WireConnection;135;1;134;1
WireConnection;20;0;21;0
WireConnection;20;1;30;0
WireConnection;103;0;102;0
WireConnection;103;1;101;0
WireConnection;89;0;85;0
WireConnection;76;0;77;0
WireConnection;64;0;75;0
WireConnection;137;0;135;0
WireConnection;137;1;136;0
WireConnection;109;0;103;0
WireConnection;92;0;89;0
WireConnection;92;3;87;0
WireConnection;92;4;91;0
WireConnection;93;0;86;0
WireConnection;93;3;88;0
WireConnection;93;4;90;0
WireConnection;104;0;20;0
WireConnection;55;51;62;0
WireConnection;55;13;76;0
WireConnection;55;24;63;0
WireConnection;55;2;64;0
WireConnection;47;2;93;0
WireConnection;47;3;92;0
WireConnection;138;0;137;0
WireConnection;67;0;55;53
WireConnection;67;1;68;0
WireConnection;67;2;69;0
WireConnection;140;0;138;0
WireConnection;140;1;139;0
WireConnection;34;0;35;0
WireConnection;34;1;106;0
WireConnection;113;0;114;0
WireConnection;113;1;115;0
WireConnection;52;0;53;0
WireConnection;52;1;47;0
WireConnection;142;0;140;0
WireConnection;72;0;67;0
WireConnection;49;0;52;0
WireConnection;49;1;34;0
WireConnection;49;2;113;0
WireConnection;111;0;49;0
WireConnection;143;0;141;0
WireConnection;143;1;142;0
WireConnection;81;0;74;0
WireConnection;81;1;80;0
WireConnection;25;0;105;0
WireConnection;25;1;26;0
WireConnection;25;2;27;0
WireConnection;124;0;125;0
WireConnection;124;1;123;0
WireConnection;124;2;122;0
WireConnection;145;0;143;0
WireConnection;71;0;124;0
WireConnection;71;1;25;0
WireConnection;71;2;73;0
WireConnection;70;0;112;0
WireConnection;70;1;81;0
WireConnection;156;0;67;0
WireConnection;0;2;70;0
WireConnection;0;9;71;0
WireConnection;0;11;146;0
ASEEND*/
//CHKSM=7A2571CFD0DD43F8A061DFBE77C81DF6C4250F63