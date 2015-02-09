#include "FullScreenQuad.fxh"

texture Texture : register(t0);
sampler TheSampler : register(s0) = sampler_state
{
	Texture = <Texture>;
	MinFilter = Linear;
	MipFilter = Linear;
	MagFilter = Linear;
};

//#ifdef XBOX
//	#define FXAA_360 1
//#else
#define FXAA_PC 1
//#endif
#define FXAA_HLSL_3 1
#define FXAA_GREEN_AS_LUMA 1

#include "Fxaa3_11.fxh"

float2 InverseViewportSize;
float4 ConsoleSharpness;
float4 ConsoleOpt1;
float4 ConsoleOpt2;
float SubPixelAliasingRemoval : FXAA_SUBPIXELALIASINGREMOVAL;
float EdgeThreshold : FXAA_EDGETHRESHOLD;
float EdgeThresholdMin : FXAA_EDGETHRESHOLDMIN;
float ConsoleEdgeSharpness;

float ConsoleEdgeThreshold;
float ConsoleEdgeThresholdMin;

// Must keep this as constant register instead of an immediate
float4 Console360ConstDir = float4(1.0, -1.0, 0.25, -0.25);

float4 PixelShaderFunction_FXAA(in float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 theSample = tex2D(TheSampler, texCoords);

	float4 value = FxaaPixelShader(
	texCoords,
	0,	// Not used in PC or Xbox 360
	TheSampler,
	TheSampler,			// *** TODO: For Xbox, can I use additional sampler with exponent bias of -1
	TheSampler,			// *** TODO: For Xbox, can I use additional sampler with exponent bias of -2
	InverseViewportSize,	// FXAA Quality only
	ConsoleSharpness,		// Console only
	ConsoleOpt1,
	ConsoleOpt2,
	SubPixelAliasingRemoval,	// FXAA Quality only
	EdgeThreshold,// FXAA Quality only
	EdgeThresholdMin,
	ConsoleEdgeSharpness,
	ConsoleEdgeThreshold,	// TODO
	ConsoleEdgeThresholdMin, // TODO
	Console360ConstDir
	);

	return float4(value.rgb, 1);
}

float4 PixelShaderFunction_Standard(in float2 texCoords : TEXCOORD0) : COLOR0
{
	return tex2D(TheSampler, texCoords);
}

technique Standard
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 FullScreenQuadVS();
		PixelShader = compile ps_3_0 PixelShaderFunction_Standard();
	}
}

technique FXAA
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 FullScreenQuadVS();
		PixelShader = compile ps_3_0 PixelShaderFunction_FXAA();
	}
}
