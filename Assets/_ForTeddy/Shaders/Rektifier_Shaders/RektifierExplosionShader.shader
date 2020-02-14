// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RektifierExplosionShader"
{
	Properties
	{
		_MainAlphaTexture("Main Alpha Texture", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float _Float0;
		uniform sampler2D _MainAlphaTexture;
		uniform float4 _MainAlphaTexture_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_MainAlphaTexture = i.uv_texcoord * _MainAlphaTexture_ST.xy + _MainAlphaTexture_ST.zw;
			float2 panner10 = ( 1.0 * _Time.y * float2( -0.5,-5 ) + uv0_MainAlphaTexture);
			float4 tex2DNode1 = tex2D( _MainAlphaTexture, panner10 );
			o.Emission = ( _Float0 * ( i.vertexColor * tex2DNode1 ) ).rgb;
			o.Alpha = ( i.vertexColor.a * tex2DNode1.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
222;302;1338;588;1243.072;38.14882;1.732992;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;4;-700.3052,148.2467;Inherit;True;Property;_MainAlphaTexture;Main Alpha Texture;0;0;Create;True;0;0;False;0;70e3c254684d001468973642dceb7a88;70e3c254684d001468973642dceb7a88;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-388.7264,216.6626;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;10;-89.19449,219.0832;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,-5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;100.8917,153.7902;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;70e3c254684d001468973642dceb7a88;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;29;228.3756,-8.68131;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;467.6501,-109.8767;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;489.4416,32.16861;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-348.7486,-1118.748;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;225.4932,-1093.84;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;12;-732.6522,-1226.439;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;713.1459,-71.47075;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-119.2793,-1113.076;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;28;-679.7994,-1431.09;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-926.6677,-1151.964;Inherit;False;Property;_FresnelPower;Fresnel Power;2;0;Create;True;0;0;False;0;2.95;2.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;502.4341,181.4143;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-909.5329,-1221.321;Inherit;False;Property;_FresnelScale;Fresnel Scale;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;27;950.1558,-92.09035;Float;False;True;6;ASEMaterialInspector;0;0;Standard;RektifierExplosionShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;2;4;0
WireConnection;10;0;3;0
WireConnection;1;0;4;0
WireConnection;1;1;10;0
WireConnection;6;0;29;0
WireConnection;6;1;1;0
WireConnection;17;0;28;0
WireConnection;17;1;12;0
WireConnection;19;0;30;0
WireConnection;12;2;13;0
WireConnection;12;3;14;0
WireConnection;50;0;51;0
WireConnection;50;1;6;0
WireConnection;30;0;17;0
WireConnection;39;0;29;4
WireConnection;39;1;1;4
WireConnection;27;2;50;0
WireConnection;27;9;39;0
ASEEND*/
//CHKSM=154DF3414996B60BF640E2BBD36B802013703C0B