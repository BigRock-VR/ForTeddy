// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Projectiles_Helics_Shader"
{
	Properties
	{
		_Main_Alpha_Texture("Main_Alpha_Texture", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 0.97
		_FirstColorGradient("First Color Gradient", Color) = (1,0,0,0)
		_SecondColorGradient("Second Color Gradient", Color) = (0.9827943,1,0,0)
		_GradientControl("Gradient Control", Range( -1 , 1)) = 0.1
		_Emission_Intensity("Emission_Intensity", Float) = 5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _FirstColorGradient;
		uniform sampler2D _Main_Alpha_Texture;
		uniform float4 _Main_Alpha_Texture_ST;
		uniform float _GradientControl;
		uniform float4 _SecondColorGradient;
		uniform float _Emission_Intensity;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_Main_Alpha_Texture = i.uv_texcoord * _Main_Alpha_Texture_ST.xy + _Main_Alpha_Texture_ST.zw;
			float4 tex2DNode2 = tex2D( _Main_Alpha_Texture, uv0_Main_Alpha_Texture );
			float Alphamap59 = tex2DNode2.a;
			float2 temp_cast_0 = (_GradientControl).xx;
			float2 uv_TexCoord54 = i.uv_texcoord + temp_cast_0;
			float smoothstepResult67 = smoothstep( -0.06 , 0.14 , uv_TexCoord54.y);
			float4 emissionGradient61 = ( ( _FirstColorGradient * Alphamap59 * smoothstepResult67 ) + ( Alphamap59 * float4( 1,1,1,0 ) * _SecondColorGradient ) );
			o.Emission = ( emissionGradient61 * _Emission_Intensity ).rgb;
			float lerpResult33 = lerp( tex2DNode2.a , 0.0 , (1.0 + (_Opacity - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)));
			float Opacity73 = lerpResult33;
			o.Alpha = Opacity73;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
196;189;1286;596;1999.476;492.8294;2.106708;True;False
Node;AmplifyShaderEditor.CommentaryNode;20;-1611.537,203.0965;Float;False;1232.657;392.7575;Alpha texture;8;73;33;35;34;59;2;1;3;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1594.795,245.8704;Float;True;Property;_Main_Alpha_Texture;Main_Alpha_Texture;0;0;Create;True;0;0;False;0;c70becb8734053f49a77b948c8b82aa2;c70becb8734053f49a77b948c8b82aa2;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.CommentaryNode;37;-1610.501,-377.6586;Float;False;1510.179;555.6242;Comment;12;53;69;61;65;57;56;48;67;55;68;54;41;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1369.193,315.0894;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1601.35,-117.3332;Float;False;Property;_GradientControl;Gradient Control;4;0;Create;True;0;0;False;0;0.1;-0.27;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1101.252,250.3012;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-1335.406,-169.3755;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;68;-1316.028,-55.08246;Float;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;-0.06;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1314.732,12.25824;Float;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;0.14;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;-817.6052,250.5621;Float;False;Alphamap;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;67;-1048.969,-158.5085;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;-1011.605,-342.4992;Float;False;Property;_FirstColorGradient;First Color Gradient;2;0;Create;True;0;0;False;0;1,0,0,0;0.8743455,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;48;-1290.06,-245.4641;Float;False;59;Alphamap;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;53;-1073.975,-27.49057;Float;False;Property;_SecondColorGradient;Second Color Gradient;3;0;Create;True;0;0;False;0;0.9827943,1,0,0;1,0,0.04188482,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-751.823,-249.847;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1255.834,435.0415;Float;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;False;0;0.97;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-818.2813,-118.4234;Float;True;3;3;0;FLOAT;0;False;1;COLOR;1,1,1,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-563.8556,-197.2504;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;35;-992.508,436.2412;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-350.5992,-201.1028;Float;False;emissionGradient;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;33;-806.3412,347.0668;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;60;105.1585,21.75283;Float;False;61;emissionGradient;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-577.9549,345.219;Float;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;120.4057,94.31488;Float;False;Property;_Emission_Intensity;Emission_Intensity;5;0;Create;True;0;0;False;0;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;314.4128,205.5635;Float;False;73;Opacity;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;336.7688,38.17072;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;481.1699,6.731759;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Projectiles_Helics_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;2;3;0
WireConnection;2;0;3;0
WireConnection;2;1;1;0
WireConnection;54;1;41;0
WireConnection;59;0;2;4
WireConnection;67;0;54;2
WireConnection;67;1;68;0
WireConnection;67;2;69;0
WireConnection;57;0;55;0
WireConnection;57;1;48;0
WireConnection;57;2;67;0
WireConnection;56;0;48;0
WireConnection;56;2;53;0
WireConnection;65;0;57;0
WireConnection;65;1;56;0
WireConnection;35;0;34;0
WireConnection;61;0;65;0
WireConnection;33;0;2;4
WireConnection;33;2;35;0
WireConnection;73;0;33;0
WireConnection;71;0;60;0
WireConnection;71;1;72;0
WireConnection;0;2;71;0
WireConnection;0;9;74;0
ASEEND*/
//CHKSM=722231D6CAFDEF45BD7344B2E1D751CCA0D48F44