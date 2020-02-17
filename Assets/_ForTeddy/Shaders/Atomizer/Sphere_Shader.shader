// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "M"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_Cutoff( "Mask Clip Value", Float ) = 0.08
		_Noise1("Noise1", Vector) = (2,3,0,0)
		_Time1("Time1", Float) = 1
		[HDR]_Color1("Color 1", Color) = (0.9974991,1,0,0)
		_Color0("Color 0", Color) = (1,0.103951,0,0)
		_Speed1("Speed1", Vector) = (1,0,0,0)
		_Speed2("Speed2", Vector) = (0,1,0,0)
		_Time2("Time2", Float) = 2
		_Noise2("Noise2", Vector) = (5,6,0,0)
		_Offset("Offset", Float) = 1.54
		_VertexOffset("VertexOffset", Float) = 0
		_Smooth("Smooth", Vector) = (0.11,-0.1,0,0)
		_MultiSmoothstep("MultiSmoothstep", Float) = 0.39
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float2 _Smooth;
		uniform float _Time1;
		uniform float2 _Speed1;
		uniform float2 _Noise1;
		uniform float _Time2;
		uniform float2 _Speed2;
		uniform float2 _Noise2;
		uniform float _MultiSmoothstep;
		uniform float _Offset;
		uniform float _VertexOffset;
		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform float _Cutoff = 0.08;
		uniform float _EdgeLength;


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


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime5 = _Time.y * _Time1;
			float2 uv_TexCoord2 = v.texcoord.xy * _Noise1;
			float2 panner4 = ( mulTime5 * _Speed1 + uv_TexCoord2);
			float simplePerlin2D1 = snoise( panner4 );
			float mulTime13 = _Time.y * _Time2;
			float2 uv_TexCoord18 = v.texcoord.xy * _Noise2;
			float2 panner17 = ( mulTime13 * _Speed2 + uv_TexCoord18);
			float simplePerlin2D21 = snoise( panner17 );
			float smoothstepResult24 = smoothstep( _Smooth.x , _Smooth.y , ( simplePerlin2D1 * simplePerlin2D21 ));
			float temp_output_29_0 = ( smoothstepResult24 * _MultiSmoothstep );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( temp_output_29_0 * ase_vertexNormal ) * _Offset ) * _VertexOffset );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime5 = _Time.y * _Time1;
			float2 uv_TexCoord2 = i.uv_texcoord * _Noise1;
			float2 panner4 = ( mulTime5 * _Speed1 + uv_TexCoord2);
			float simplePerlin2D1 = snoise( panner4 );
			float mulTime13 = _Time.y * _Time2;
			float2 uv_TexCoord18 = i.uv_texcoord * _Noise2;
			float2 panner17 = ( mulTime13 * _Speed2 + uv_TexCoord18);
			float simplePerlin2D21 = snoise( panner17 );
			float smoothstepResult24 = smoothstep( _Smooth.x , _Smooth.y , ( simplePerlin2D1 * simplePerlin2D21 ));
			float temp_output_29_0 = ( smoothstepResult24 * _MultiSmoothstep );
			float Noise55 = temp_output_29_0;
			float4 lerpResult28 = lerp( _Color1 , _Color0 , Noise55);
			o.Emission = ( i.vertexColor * ( lerpResult28 * 1.3 ) ).rgb;
			o.Alpha = 1;
			clip( Noise55 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
439;515;1025;504;941.9611;259.65;1.889798;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-3820.076,476.918;Float;False;Property;_Time1;Time1;7;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-3822.131,825.3882;Float;False;Property;_Time2;Time2;12;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-3674.669,482.5928;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;7;-3663.534,365.0137;Float;False;Property;_Speed1;Speed1;10;0;Create;True;0;0;False;0;1,0;0.3,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-3665.589,713.4839;Float;False;Property;_Speed2;Speed2;11;0;Create;True;0;0;False;0;0,1;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;13;-3676.724,831.063;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;8;-3502.382,495.8876;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;10;-3519.942,395.3214;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;3;-3906.059,257.3857;Float;False;Property;_Noise1;Noise1;6;0;Create;True;0;0;False;0;2,3;0.5,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WireNode;20;-3521.997,743.7916;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;15;-3504.437,844.3577;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;22;-3902.1,600.6727;Float;False;Property;_Noise2;Noise2;13;0;Create;True;0;0;False;0;5,6;5,6;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WireNode;19;-3506.034,686.3253;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;11;-3503.979,337.855;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-3737.924,596.9254;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-3735.869,248.4551;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;16;-3526.786,662.3809;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;9;-3524.731,313.9108;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;4;-3460.836,249.7805;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;17;-3462.891,598.2507;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;21;-3264.501,593.6489;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-3262.446,245.1788;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2917.832,407.6614;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;25;-2858.604,624.2678;Float;False;Property;_Smooth;Smooth;18;0;Create;True;0;0;False;0;0.11,-0.1;0.44,-0.49;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;30;-2577.612,709.2385;Float;False;Property;_MultiSmoothstep;MultiSmoothstep;19;0;Create;True;0;0;False;0;0.39;1.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;24;-2628.46,441.5062;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2347.06,457.2488;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-2083.704,879.503;Float;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;51;-1778.96,465.0333;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;27;-1128.251,-97.86773;Float;False;Property;_Color1;Color 1;8;1;[HDR];Create;True;0;0;False;0;0.9974991,1,0,0;0,0.173912,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;56;-895.5595,133.3587;Float;False;55;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-1155.628,129.6212;Float;False;Property;_Color0;Color 0;9;0;Create;True;0;0;False;0;1,0.103951,0,0;0,0.8694534,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1515.238,436.4388;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1480.432,647.5142;Float;True;Property;_Offset;Offset;15;0;Create;True;0;0;False;0;1.54;0.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;28;-720.9962,31.51138;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-653.0593,259.1846;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;1.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-869.6038,541.8828;Float;False;Property;_VertexOffset;VertexOffset;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1264.649,452.99;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;99;129.5543,-72.56004;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-455.1987,98.25556;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-2184.073,1014.858;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;6.04,-20.05;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;73;-1710.65,1011.581;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;94;-1007.68,1125.327;Float;True;2;0;FLOAT;0;False;1;FLOAT;0.59;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;358.2199,42.71765;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;95;-1369.879,1160.261;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-607.865,458.6022;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-2183.457,1251.333;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0.43;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;79;-2347.633,1255.08;Float;False;Property;_Subs;Subs;17;0;Create;True;0;0;False;0;0,2;0,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;88;-576.1667,883.9299;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;68;-2348.249,1018.605;Float;False;Property;_Vector1;Vector 1;14;0;Create;True;0;0;False;0;1,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;83;-1908.424,1252.658;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;72;-1909.041,1016.183;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;529.8849,310.496;Float;False;55;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;84;-1710.034,1248.057;Float;True;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;90;-268.6003,877.5296;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;869.0161,136.0477;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;M;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.08;True;True;0;True;Transparent;;Overlay;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;13;0;14;0
WireConnection;8;0;5;0
WireConnection;10;0;7;0
WireConnection;20;0;12;0
WireConnection;15;0;13;0
WireConnection;19;0;15;0
WireConnection;11;0;8;0
WireConnection;18;0;22;0
WireConnection;2;0;3;0
WireConnection;16;0;20;0
WireConnection;9;0;10;0
WireConnection;4;0;2;0
WireConnection;4;2;9;0
WireConnection;4;1;11;0
WireConnection;17;0;18;0
WireConnection;17;2;16;0
WireConnection;17;1;19;0
WireConnection;21;0;17;0
WireConnection;1;0;4;0
WireConnection;23;0;1;0
WireConnection;23;1;21;0
WireConnection;24;0;23;0
WireConnection;24;1;25;1
WireConnection;24;2;25;2
WireConnection;29;0;24;0
WireConnection;29;1;30;0
WireConnection;55;0;29;0
WireConnection;48;0;29;0
WireConnection;48;1;51;0
WireConnection;28;0;27;0
WireConnection;28;1;26;0
WireConnection;28;2;56;0
WireConnection;49;0;48;0
WireConnection;49;1;50;0
WireConnection;52;0;28;0
WireConnection;52;1;53;0
WireConnection;70;0;68;0
WireConnection;73;0;72;0
WireConnection;94;0;95;0
WireConnection;98;0;99;0
WireConnection;98;1;52;0
WireConnection;95;0;73;0
WireConnection;95;1;84;0
WireConnection;96;0;49;0
WireConnection;96;1;97;0
WireConnection;81;0;79;0
WireConnection;88;0;55;0
WireConnection;88;1;84;0
WireConnection;83;0;81;0
WireConnection;72;0;70;0
WireConnection;84;0;83;0
WireConnection;90;0;88;0
WireConnection;0;2;98;0
WireConnection;0;10;57;0
WireConnection;0;11;96;0
ASEEND*/
//CHKSM=0725DBF8D11FD48619C13F848C5BAABB8F8A12B9