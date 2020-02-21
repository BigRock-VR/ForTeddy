// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Weapons/WeaponMaterial"
{
	Properties
	{
		_Texture0("Albedo", 2D) = "white" {}
		_Texture1("Normal", 2D) = "bump" {}
		_Texture3("MS", 2D) = "white" {}
		_Scale("Scale", Float) = 0.79
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
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
		uniform float _Scale;
		uniform float4 _Texture1_ST;
		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform sampler2D _Texture3;
		uniform float4 _Texture3_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_Texture1 = i.uv_texcoord * _Texture1_ST.xy + _Texture1_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Texture1, uv0_Texture1 ), _Scale );
			float2 uv0_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			o.Albedo = tex2D( _Texture0, uv0_Texture0 ).rgb;
			float2 uv0_Texture3 = i.uv_texcoord * _Texture3_ST.xy + _Texture3_ST.zw;
			float4 tex2DNode10 = tex2D( _Texture3, uv0_Texture3 );
			o.Metallic = tex2DNode10.r;
			o.Smoothness = tex2DNode10.b;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
318;603;1025;416;1557.148;-505.3969;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1395.644,-37.14548;Float;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;False;0;84e011d1fff420845938f36327c1d6b5;84e011d1fff420845938f36327c1d6b5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;12;-1362.223,1114.065;Float;True;Property;_Texture3;Texture 3;3;0;Create;True;0;0;False;0;67d190cb125a6ba4a94f726f5cfb056a;67d190cb125a6ba4a94f726f5cfb056a;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1490.118,386.338;Float;True;Property;_Texture1;Texture 1;1;0;Create;True;0;0;False;0;decdc936dd6f95e42a8e60db41c0c54c;decdc936dd6f95e42a8e60db41c0c54c;True;bump;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1059.864,43.84371;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1089.573,482.0466;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1026.443,1195.054;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-958.8218,649.0781;Float;False;Property;_Scale;Scale;4;0;Create;True;0;0;False;0;0.79;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;9;-1365.936,716.7119;Float;True;Property;_Texture2;Texture 2;2;0;Create;True;0;0;False;0;b20cc4873974ce74e80e3050e305de96;b20cc4873974ce74e80e3050e305de96;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1030.156,797.7011;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-803.7499,-38.89998;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-774.0411,714.9574;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-770.328,1112.311;Float;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-833.4585,399.3029;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-114.3538,792.4349;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Weapons/WeaponMaterial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;2;3;0
WireConnection;5;2;6;0
WireConnection;11;2;12;0
WireConnection;8;2;9;0
WireConnection;1;0;3;0
WireConnection;1;1;2;0
WireConnection;7;0;9;0
WireConnection;7;1;8;0
WireConnection;10;0;12;0
WireConnection;10;1;11;0
WireConnection;4;0;6;0
WireConnection;4;1;5;0
WireConnection;4;5;13;0
WireConnection;0;0;1;0
WireConnection;0;1;4;0
WireConnection;0;3;10;1
WireConnection;0;4;10;3
ASEEND*/
//CHKSM=439E924BD384F421590A3B9FA9390041A1A1C4F7