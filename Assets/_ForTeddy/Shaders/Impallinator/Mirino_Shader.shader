// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "mirino"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Texture1("Texture 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture1;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color11 = IsGammaSpace() ? float4(1,0.4142866,0,0) : float4(1,0.143102,0,0);
			float4 color12 = IsGammaSpace() ? float4(0.1336713,0,1,0) : float4(0.01606527,0,1,0);
			float grayscale43 = Luminance(float3( ( ( i.uv_texcoord - float2( 0.82,0.82 ) ) * ( i.uv_texcoord - float2( 0.18,0.18 ) ) ) ,  0.0 ));
			float smoothstepResult26 = smoothstep( -0.14 , 0.09 , grayscale43);
			float4 lerpResult27 = lerp( color11 , color12 , smoothstepResult26);
			o.Emission = lerpResult27.rgb;
			o.Alpha = 1;
			clip( (0.33*UnpackScaleNormal( tex2D( _Texture1, i.uv_texcoord ), 2.0 ).x + 0.65) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
330;265;1243;639;961.0405;326.2326;2.095491;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-974.3903,-563.1768;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-604.7802,-285.4453;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0.18,0.18;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;-589.8262,-571.0395;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0.82,0.82;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-302.0898,-419.2416;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1413.481,485.9143;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;8;-1344.741,270.8151;Float;True;Property;_Texture1;Texture 1;1;0;Create;True;0;0;False;0;dbb10702a4fb853478e35eebe0010f90;9068c265b588d0b439c5586658dfd116;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TFHCGrayscale;43;-34.27247,-384.8964;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;81.74153,-824.7547;Float;False;Constant;_Color2;Color 2;2;0;Create;True;0;0;False;0;1,0.4142866,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-818.427,422.0946;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;85.51453,-646.2197;Float;False;Constant;_Color3;Color 3;2;0;Create;True;0;0;False;0;0.1336713,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;26;223.2698,-407.9553;Float;True;3;0;FLOAT;0;False;1;FLOAT;-0.14;False;2;FLOAT;0.09;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;15;-423.7367,413.5451;Float;False;3;0;FLOAT;0.33;False;1;FLOAT;1;False;2;FLOAT;0.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;502.311,-608.6017;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;905.6317,-35.35012;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;mirino;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;28;0
WireConnection;18;0;28;0
WireConnection;21;0;18;0
WireConnection;21;1;20;0
WireConnection;43;0;21;0
WireConnection;13;0;8;0
WireConnection;13;1;6;0
WireConnection;26;0;43;0
WireConnection;15;1;13;0
WireConnection;27;0;11;0
WireConnection;27;1;12;0
WireConnection;27;2;26;0
WireConnection;0;2;27;0
WireConnection;0;10;15;0
ASEEND*/
//CHKSM=20A64364951901B6D8CC9A9AB99ED3D24D3837DC