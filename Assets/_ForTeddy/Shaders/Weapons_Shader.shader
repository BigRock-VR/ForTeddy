// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom\Weapons"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Metallic("Metallic", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_NormalScale("NormalScale", Float) = 0.79
		_Smooth("Smooth", Range( 0 , 1)) = 0
		[HDR]_HDR("HDR", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float _NormalScale;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float4 _HDR;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _Smooth;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv0_Normal ), _NormalScale );
			float2 uv0_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv0_Albedo ).rgb;
			float2 uv0_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			o.Emission = ( tex2D( _Emission, uv0_Emission ) * _HDR ).rgb;
			float2 uv0_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			float4 tex2DNode7 = tex2D( _Metallic, uv0_Metallic );
			float4 appendResult18 = (float4(tex2DNode7.r , tex2DNode7.g , tex2DNode7.b , 0.0));
			o.Metallic = appendResult18.x;
			o.Smoothness = ( tex2DNode7.a * _Smooth );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-62;454;2110;1053;2255.295;-442.4653;1.390058;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;9;-1365.936,713.7119;Float;True;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;919d3be81b9664a4fa3478cfcfcb99ef;919d3be81b9664a4fa3478cfcfcb99ef;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;12;-1366.223,1219.065;Float;True;Property;_Emission;Emission;3;0;Create;True;0;0;False;0;None;4806bb230bfb15242b09301746caf6c9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1030.156,797.7011;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1490.118,386.338;Float;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;None;a34822292c26a654c9e95e89f87750d0;False;bump;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1395.644,-37.14548;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;3daac5c59d85f5b4881601cdad422c10;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1032.443,1300.054;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-526.063,1504.47;Float;False;Property;_HDR;HDR;6;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1089.573,482.0466;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-662.4072,974.326;Float;False;Property;_Smooth;Smooth;5;0;Create;True;0;0;False;0;0;0.458;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-774.0411,714.9574;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1059.864,43.84371;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-699.5773,588.4678;Float;False;Property;_NormalScale;NormalScale;4;0;Create;True;0;0;False;0;0.79;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-776.328,1217.311;Float;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-350.9156,1301.521;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;-833.4585,399.3029;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;18;-332.0486,700.9767;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-438.4072,925.326;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-803.7499,-38.89998;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;66.3462,628.6348;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;m;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;2;9;0
WireConnection;11;2;12;0
WireConnection;5;2;6;0
WireConnection;7;0;9;0
WireConnection;7;1;8;0
WireConnection;2;2;3;0
WireConnection;10;0;12;0
WireConnection;10;1;11;0
WireConnection;21;0;10;0
WireConnection;21;1;22;0
WireConnection;4;0;6;0
WireConnection;4;1;5;0
WireConnection;4;5;13;0
WireConnection;18;0;7;1
WireConnection;18;1;7;2
WireConnection;18;2;7;3
WireConnection;19;0;7;4
WireConnection;19;1;20;0
WireConnection;1;0;3;0
WireConnection;1;1;2;0
WireConnection;0;0;1;0
WireConnection;0;1;4;0
WireConnection;0;2;21;0
WireConnection;0;3;18;0
WireConnection;0;4;19;0
ASEEND*/
//CHKSM=5F5559D2152C002DEF54FDE15DE980AC0DED990E