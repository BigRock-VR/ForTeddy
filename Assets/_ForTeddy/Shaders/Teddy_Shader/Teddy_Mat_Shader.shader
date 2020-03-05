// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Teddy_Mat_Shader"
{
	Properties
	{
		_Albedo_Texture("Albedo_Texture", 2D) = "white" {}
		_AlbedoMapIntensity("Albedo Map Intensity", Float) = 0
		_TextureColorChanger("Texture Color Changer", Color) = (0,0,0,0)
		_Normal_Map("Normal_Map", 2D) = "white" {}
		_NormalMapStrenght("Normal Map Strenght", Float) = 0
		_MAOS_Texture("MAOS_Texture", 2D) = "white" {}
		_MAOSIntensity("MAOS Intensity", Range( 0 , 1)) = 0
		_Damage_Emission_Texture("Damage_Emission_Texture", 2D) = "white" {}
		_Damage_Texture_Panning_Speed("Damage_Texture_Panning_Speed", Vector) = (0,0,0,0)
		[HDR]_EmissionColor0("Emission Color 0", Color) = (1,0,0,0)
		[HDR]_EmissionColor1("Emission Color 1", Color) = (0,1,0.9898968,0)
		_Damage_Tiling("Damage_Tiling", Range( 0 , 1)) = 0.03529412
		_Fresnel_Power("Fresnel Power", Range( 0 , 1)) = 1
		_Colorchanger("Color changer", Int) = 1
		_fresnelscale0off1on("Fresnel Activation", Int) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard fullforwardshadows keepalpha 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normal_Map;
		uniform float _NormalMapStrenght;
		uniform float4 _Normal_Map_ST;
		uniform float4 _TextureColorChanger;
		uniform sampler2D _Albedo_Texture;
		uniform float4 _Albedo_Texture_ST;
		uniform float _AlbedoMapIntensity;
		uniform float4 _EmissionColor0;
		uniform float4 _EmissionColor1;
		uniform int _Colorchanger;
		uniform sampler2D _Damage_Emission_Texture;
		uniform float2 _Damage_Texture_Panning_Speed;
		uniform float _Damage_Tiling;
		uniform int _fresnelscale0off1on;
		uniform float _Fresnel_Power;
		uniform sampler2D _MAOS_Texture;
		uniform float4 _MAOS_Texture_ST;
		uniform float _MAOSIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv0_Normal_Map = i.uv_texcoord * _Normal_Map_ST.xy + _Normal_Map_ST.zw;
			float3 Normals41 = UnpackScaleNormal( tex2D( _Normal_Map, uv0_Normal_Map ), _NormalMapStrenght );
			o.Normal = Normals41;
			float2 uv0_Albedo_Texture = i.uv_texcoord * _Albedo_Texture_ST.xy + _Albedo_Texture_ST.zw;
			float4 AlbedoWithTint39 = ( _TextureColorChanger * tex2D( _Albedo_Texture, uv0_Albedo_Texture ) * _AlbedoMapIntensity );
			o.Albedo = AlbedoWithTint39.rgb;
			int Int67 = _Colorchanger;
			float4 lerpResult63 = lerp( _EmissionColor0 , _EmissionColor1 , (float)Int67);
			float4 appendResult79 = (float4(_Damage_Texture_Panning_Speed.x , ( _Damage_Texture_Panning_Speed.y * Int67 ) , 0.0 , 0.0));
			float3 ase_worldPos = i.worldPos;
			float2 panner19 = ( 1.0 * _Time.y * appendResult79.xy + ( ase_worldPos * ( _Damage_Tiling * Int67 ) ).xy);
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV13 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode13 = ( 0.0 + (float)_fresnelscale0off1on * pow( 1.0 - fresnelNdotV13, ( 1.0 - (-3.0 + (_Fresnel_Power - 0.0) * (1.0 - -3.0) / (1.0 - 0.0)) ) ) );
			float4 Emission56 = ( lerpResult63 * ( tex2D( _Damage_Emission_Texture, panner19 ) * fresnelNode13 ) );
			o.Emission = Emission56.rgb;
			float2 uv0_MAOS_Texture = i.uv_texcoord * _MAOS_Texture_ST.xy + _MAOS_Texture_ST.zw;
			float4 Maos49 = ( tex2D( _MAOS_Texture, uv0_MAOS_Texture ) * _MAOSIntensity );
			float4 break53 = Maos49;
			o.Metallic = break53;
			o.Smoothness = break53.b;
			o.Occlusion = break53.g;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
47;246;1338;416;3170.758;577.0068;3.322212;True;False
Node;AmplifyShaderEditor.IntNode;65;480.4792,588.0508;Inherit;False;Property;_Colorchanger;Color changer;13;0;Create;True;0;0;False;0;1;1;0;1;INT;0
Node;AmplifyShaderEditor.CommentaryNode;54;-1683.202,974.2745;Inherit;False;1622.994;731.3176;Shield Fresnel or Damage;18;15;12;13;34;10;19;32;28;61;20;29;17;75;76;78;79;80;81;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;667.7704,586.4548;Inherit;False;Int;-1;True;1;0;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;32;-1668.035,1392.137;Float;False;Property;_Damage_Texture_Panning_Speed;Damage_Texture_Panning_Speed;8;0;Create;True;0;0;False;0;0,0;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;80;-1589.307,1511.614;Inherit;False;67;Int;1;0;OBJECT;0;False;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;-1649.044,1316.703;Inherit;False;67;Int;1;0;OBJECT;0;False;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1675.702,1227.76;Float;False;Property;_Damage_Tiling;Damage_Tiling;11;0;Create;True;0;0;False;0;0.03529412;0.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;20;-1589.983,1092.214;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1338.307,1431.614;Inherit;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1383.036,1536.826;Float;False;Property;_Fresnel_Power;Fresnel_Power;12;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-1410.044,1236.703;Inherit;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;48;-1518.928,669.9499;Inherit;False;1072.811;291.7099;Maos;6;49;9;8;7;83;82;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1270.786,1210.752;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;37;-1509.89,320.4851;Inherit;False;1014.813;334.2096;Normal Map;7;41;60;43;46;6;5;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;79;-1174.775,1335.104;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;61;-1122.814,1528.681;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-3;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;38;-1507.552,-102.4011;Inherit;False;1205.827;411.844;Albedo Map with tint;7;3;36;35;1;2;39;84;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;-1512.128,707.1496;Float;True;Property;_MAOS_Texture;MAOS_Texture;5;0;Create;True;0;0;False;0;None;6e116c1238147804caac4fa5e8f6ad93;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.OneMinusNode;34;-945.2557,1519.58;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-999.3779,582.1571;Inherit;False;Property;_NormalMapStrenght;Normal Map Strenght;4;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1455.869,110.2518;Float;True;Property;_Albedo_Texture;Albedo_Texture;0;0;Create;True;0;0;False;0;None;8b1aaa92b97a2bc47a0898d75a70f3e0;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1304.497,1024.275;Float;True;Property;_Damage_Emission_Texture;Damage_Emission_Texture;7;0;Create;True;0;0;False;0;61c0b9c0523734e0e91bc6043c72a490;61c0b9c0523734e0e91bc6043c72a490;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;19;-978.758,1126.676;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;75;-1006.582,1285.87;Inherit;False;Property;_fresnelscale0off1on;fresnel scale(0 off 1 on);14;0;Create;True;0;0;False;0;1;1;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1273.507,744.6595;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;68;-230.9558,883.1256;Inherit;False;67;Int;1;0;OBJECT;0;False;1;INT;0
Node;AmplifyShaderEditor.FresnelNode;13;-755.6036,1223.203;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1503.945,367.0536;Float;True;Property;_Normal_Map;Normal_Map;3;0;Create;True;0;0;False;0;None;9c8686719e410db4e8858e1c16e47acf;True;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1166.12,183.1518;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-436.4953,639.5987;Float;False;Property;_EmissionColor0;Emission Color 0;9;1;[HDR];Create;True;0;0;False;0;1,0,0,0;1,0,0.01999569,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;62;-437.2692,801.7275;Inherit;False;Property;_EmissionColor1;Emission Color 1;10;1;[HDR];Create;True;0;0;False;0;0,1,0.9898968,0;0,1,0.9616327,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-798.0408,1022.129;Inherit;True;Global;TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;-1034.448,887.2723;Inherit;False;Property;_MAOSIntensity;MAOS Intensity;6;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-1055.318,704.1168;Inherit;True;Global;TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;60;-799.5927,553.9102;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-934.7402,110.4528;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-434.9007,1025.791;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;63;-22.33199,749.6428;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-630.368,156.993;Inherit;False;Property;_AlbedoMapIntensity;Albedo Map Intensity;1;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-771.8486,722.8313;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;46;-1022.378,544.1571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1248.649,439.9299;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;35;-901.5574,-56.4011;Inherit;False;Property;_TextureColorChanger;Texture Color Changer;2;0;Create;True;0;0;False;0;0,0,0,0;1,0.9242496,0.9009434,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;227.0466,926.1859;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-644.5455,724.5087;Inherit;False;Maos;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-637.3757,25.48262;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-1007.707,369.7106;Inherit;True;Global;TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-511.3337,17.59824;Inherit;False;AlbedoWithTint;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-25.9998,309.0051;Inherit;False;49;Maos;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-719.2397,367.6792;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;389.7912,928.6563;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;186.3044,220.7959;Inherit;False;56;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-1.725159,141.1574;Inherit;False;41;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-42.20056,64.89944;Inherit;False;39;AlbedoWithTint;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;53;136.4597,299.1949;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;511.0371,138.8424;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Teddy_Mat_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;67;0;65;0
WireConnection;81;0;32;2
WireConnection;81;1;80;0
WireConnection;76;0;29;0
WireConnection;76;1;78;0
WireConnection;28;0;20;0
WireConnection;28;1;76;0
WireConnection;79;0;32;1
WireConnection;79;1;81;0
WireConnection;61;0;17;0
WireConnection;34;0;61;0
WireConnection;19;0;28;0
WireConnection;19;2;79;0
WireConnection;8;2;7;0
WireConnection;13;2;75;0
WireConnection;13;3;34;0
WireConnection;3;2;2;0
WireConnection;12;0;10;0
WireConnection;12;1;19;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;60;0;43;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;15;0;12;0
WireConnection;15;1;13;0
WireConnection;63;0;18;0
WireConnection;63;1;62;0
WireConnection;63;2;68;0
WireConnection;82;0;9;0
WireConnection;82;1;83;0
WireConnection;46;0;60;0
WireConnection;5;2;4;0
WireConnection;66;0;63;0
WireConnection;66;1;15;0
WireConnection;49;0;82;0
WireConnection;36;0;35;0
WireConnection;36;1;1;0
WireConnection;36;2;84;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;6;5;46;0
WireConnection;39;0;36;0
WireConnection;41;0;6;0
WireConnection;56;0;66;0
WireConnection;53;0;51;0
WireConnection;0;0;40;0
WireConnection;0;1;47;0
WireConnection;0;2;55;0
WireConnection;0;3;53;0
WireConnection;0;4;53;2
WireConnection;0;5;53;1
ASEEND*/
//CHKSM=47C2F2F6E08EC4449A2C854FEA0B9EFC557A69B3