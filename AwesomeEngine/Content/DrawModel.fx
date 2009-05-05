float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;

Texture ColorMap;
Texture ShadowMap;
bool TextureEnabled;
float4 DefaultBaseColor;

float4x4 WorldViewProjection;
float4x4 LightWorldViewProjection;
float3 LightPos;
float LightPower;
float Ambient;


sampler ShadowMapSampler = sampler_state
{
	texture = <ShadowMap>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter=LINEAR; 
	AddressU = clamp; 
	AddressV = clamp;
};

sampler ColorMapSampler = sampler_state
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
};

struct ScenePSOut
{
	float4 Color : COLOR0; 
};

SceneVSOut SceneVertexShader(float4 inPos : POSITION, float2 inTexCoords : TEXCOORD0, float3 inNormal : NORMAL)
{
	SceneVSOut Output = (SceneVSOut)0;
	
	Output.Position = mul(inPos, WorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, LightWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);
	Output.TexCoords = inTexCoords;
	
	return Output;
}

ScenePSOut ScenePixelShader(SceneVSOut input)
{
	ScenePSOut Output = (ScenePSOut)0;
	float2 ProjectedTexCoords;
	
	
	ProjectedTexCoords[0] = input.Pos2DAsSeenByLight.x/input.Pos2DAsSeenByLight.w/2.0f + 0.5f;
	ProjectedTexCoords[1] = -input.Pos2DAsSeenByLight.y/input.Pos2DAsSeenByLight.w/2.0f + 0.5f;
	
	float diffuseLightingFactor = 0;
	if(saturate(ProjectedTexCoords.x) == ProjectedTexCoords.x && saturate(ProjectedTexCoords.y) == ProjectedTexCoords.y)
	{
		float depthStoredInSMap = tex2D(ShadowMapSampler, ProjectedTexCoords).r;
		float realDistance = input.Pos2DAsSeenByLight.z/input.Pos2DAsSeenByLight.w;
		
		if ((realDistance - 1.0f/100.0f) <= depthStoredInSMap) 
		{
			diffuseLightingFactor = dot(normalize(LightPos+input.Position3D), input.Normal);
            diffuseLightingFactor = saturate(diffuseLightingFactor);
            diffuseLightingFactor *= LightPower;  
		}
	}
	
	float4 baseColor = tex2D(ColorMapSampler, input.TexCoords);
		if(TextureEnabled == false)
			baseColor = DefaultBaseColor;
	
	
	Output.Color = baseColor*(diffuseLightingFactor + Ambient);
	
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



