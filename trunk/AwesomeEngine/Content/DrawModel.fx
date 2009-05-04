float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;

Texture ColorMap;
Texture ShadowMap;

sampler shadowMapSampler = sampler_state
{
	texture = <ShadowMap>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = clamp; 
	AddressV = clamp;
};

sampler colorMapSampler = sampler_state
{
	texture = <ColorMap>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = Wrap; 
	AddressV = Wrap;
};

struct SceneVSOut
{
	float4 Position				 : POSITION;
    float4 Pos2DAsSeenByLight    : TEXCOORD0;
    float2 TexCoords             : TEXCOORD1;
	float3 Normal                : TEXCOORD2;
	float4 Position3D            : TEXCOORD3;	
}

struct ScenePSOut
{
	float4 Color : COLOR0; 
}

SceneVSOut SceneVertexShader(float4 inPos : POSITION0)
{
	SceneVSOut Output = (SceneVSOut)0;
	return Output;
}

ScenePSOut ScenePixelShader(SceneVSOut input)
{
	ScenePSOut Output = (ScenePSOut)0;
	return Output;
}

technique DrawModel
{
    pass Pass0
    {
        VertexShader = compile vs_3_0 SceneVertexShader();
        PixelShader = compile ps_3_0 ScenePixelShader();
    }
}



