float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 WorldViewProjection;
float4x4 LightWorldViewProjection;
Texture ShadowMap;

sampler ShadowMapSampler = sampler_state 
{ 
	texture = <ShadowMap>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = clamp; 
	AddressV = clamp;
};

// TODO: add effect parameters here.


//This is the output for the Shadow Map Vertex Shader
struct ShadowVSOutput
{
    float4 Position : POSITION;
	float4 Position2D : TEXCOORD0;
};

struct SMapPixel
{
	float4 Color : COLOR0;
};

struct SSceneVertexToPixel
{
    float4 Position             : POSITION;
    float4 Pos2DAsSeenByLight    : TEXCOORD0;
};

struct SScenePixelToFrame
{
    float4 Color : COLOR0;
};

ShadowVSOutput ShadowMapVertexShader(float4 inPos : POSITION)
{
	ShadowVSOutput Output = (ShadowVSOutput)0;
 
	Output.Position = mul(inPos, LightWorldViewProjection);

	Output.Position2D = Output.Position;
	
	return Output;
}

SMapPixel ShadowMapPixelShader(ShadowVSOutput input) : COLOR0
{
	SMapPixel Output = (SMapPixel)0;

	Output.Color = input.Position2D.z/input.Position2D.w;
	

	return Output;
}

SSceneVertexToPixel SSVertexShader(float4 inPos : POSITION)
{
	SSceneVertexToPixel Output = (SSceneVertexToPixel)0;
	
	Output.Position = mul(inPos, WorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, LightWorldViewProjection);
	
	return Output;
}

SScenePixelToFrame SSPixelShader(SSceneVertexToPixel input) 
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;
	
	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = input.Pos2DAsSeenByLight.x/input.Pos2DAsSeenByLight.w/2.0f + 0.5f;
	ProjectedTexCoords[1] = -input.Pos2DAsSeenByLight.y/input.Pos2DAsSeenByLight.w/2.0f + 0.5f;
	
	Output.Color = tex2D(ShadowMapSampler, ProjectedTexCoords);
	
	return Output;
}

technique CreateShadowMap
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 ShadowMapVertexShader();
        PixelShader = compile ps_2_0 ShadowMapPixelShader();
    }
}

technique ShadowedScene
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 SSVertexShader();
        PixelShader = compile ps_2_0 SSPixelShader();
    }
}


