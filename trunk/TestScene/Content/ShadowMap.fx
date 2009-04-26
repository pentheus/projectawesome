float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 WorldViewProjection;
float4x4 LightWorldViewProjection;
float3 LightPos;
float LightPower;
float Ambient;
Texture ShadowMap;
Texture TextureMap;
bool TextureEnabled;

float4 DefaultBaseColor = {0.4f,0.4f,0.4f,1};

sampler ShadowMapSampler = sampler_state 
{ 
	texture = <ShadowMap>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = clamp; 
	AddressV = clamp;
};

sampler TextureMapSampler = sampler_state 
{ 
	texture = <TextureMap>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = Wrap; 
	AddressV = Wrap;
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
    float2 TexCoords            : TEXCOORD1;
	float3 Normal                : TEXCOORD2;
	float4 Position3D            : TEXCOORD3;
};

struct SScenePixelToFrame
{
    float4 Color : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}

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

SSceneVertexToPixel SSVertexShader(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
	SSceneVertexToPixel Output = (SSceneVertexToPixel)0;
	
	
	Output.Position = mul(inPos, WorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, LightWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)World));
	Output.Position3D = mul(inPos, World);
	Output.TexCoords = inTexCoords;
	
	return Output;
}

SScenePixelToFrame SSPixelShader(SSceneVertexToPixel input) 
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;
	
	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = input.Pos2DAsSeenByLight.x/input.Pos2DAsSeenByLight.w/2.0f + 0.5f;
	ProjectedTexCoords[1] = -input.Pos2DAsSeenByLight.y/input.Pos2DAsSeenByLight.w/2.0f + 0.5f;
	
	float diffuseLightingFactor = 0;
	if(saturate(ProjectedTexCoords.x) == ProjectedTexCoords.x && saturate(ProjectedTexCoords.y) == ProjectedTexCoords.y)
	{
		float depthStoredInSMap = tex2D(ShadowMapSampler, ProjectedTexCoords).r;
		float realDistance = input.Pos2DAsSeenByLight.z/input.Pos2DAsSeenByLight.w;
		
		if ((realDistance - 1.0f/100.0f) <= depthStoredInSMap) // 1/100 is our shadow bias 
		{
			diffuseLightingFactor = DotProduct(LightPos, input.Position3D, input.Normal);
            diffuseLightingFactor = saturate(diffuseLightingFactor);
            diffuseLightingFactor *= LightPower;  
		}
	}
	
	float4 baseColor = tex2D(TextureMapSampler, input.TexCoords);
		if(TextureEnabled == false)
			baseColor = DefaultBaseColor;
	
	
	Output.Color = baseColor*(diffuseLightingFactor + Ambient);
	
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


