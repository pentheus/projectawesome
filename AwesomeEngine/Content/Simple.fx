#define SHADER20_MAX_BONES 80
#define MAX_LIGHTS 8
float4x3 matBones[SHADER20_MAX_BONES];

//------- XNA-to-HLSL variables --------     
float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float3 xCenter;
float xRange;
float4x4 xWorldViewProjection;  
bool xTextureEnabled;

float4 xAmbientColor = {0.3,0.3,0.3,1};
float xAmbientIntensity = 0.1f;

float4 xDirectionalColor = {0,0,0.4,1};
float3 xLightDirection = {1,1,1};
float xLightIntensity = 0.2;

float4 xDiffuseColor = { 1, 1, .5, 1};
float4 xPointColor = {0.7, 0.7, 0.7, 1};
float xPointIntensity = 2;
//------- Texture Samplers --------    
Texture xTexture;       // Input a texture from XNA code via effect.Parameters["xTexture"].SetValue(texture)  
 
sampler TextureSampler = sampler_state     
{     
    Texture = <xTexture>;     
         
    MinFilter = Linear;     
    MagFilter = Linear;     
    MipFilter = Linear;     
         
    AddressU = Wrap;   // OR Mirror OR Clamp (Wrap works with Level Editor Grid)  
    AddressV = Wrap;   // OR Mirror OR Clamp (Wrap works with Level Editor Grid)  
};    
 

/////////////////// OUTPUTS ////////////////////////////////

struct VS_OUTPUT
{
    float4 Pos : POSITION;
    float3 Light : TEXCOORD2;
    float3 Norm : TEXCOORD1;
    float2 TexCoord : TEXCOORD0;  
    float4 Position3D : TEXCOORD3;
};

/////////////////// VERTEX SHADERS //////////////////////////

VS_OUTPUT VS(float4 Pos : POSITION, float3 Normal : NORMAL, float2 TexCoord : TEXCOORD0)
{
    VS_OUTPUT Out = (VS_OUTPUT)1;
    Out.Pos = mul(Pos, mul(xWorld, mul(xView,xProjection))); // transform Position
    Out.Light = xLightDirection; // output light vector
    Out.Norm = normalize(mul(Normal, xWorld)); // transform       Normal and normalize it
    Out.TexCoord = TexCoord;
    Out.Position3D = mul(Pos, xWorld);
    return Out;
}

/////////////////// PIXEL SHADERS ////////////////////////////////

float4 PS(VS_OUTPUT input) : COLOR
{
	float4 diffuseColor = tex2D(TextureSampler, input.TexCoord);
	float3 LightVector = input.Position3D - xCenter;
	float dist = length(LightVector);
	LightVector = normalize(LightVector);
	
	
    if(xTextureEnabled == false)
		diffuseColor = xDiffuseColor;
	float NdL1 = saturate(dot(input.Norm, input.Light));
	float NdL2 = saturate(dot(input.Norm, -LightVector))/dist;
	float4 DirLightColor = xDirectionalColor * xLightIntensity;
	float4 PointLightColor = xPointColor * xPointIntensity;
	float4 AmbiColor = xAmbientColor * xAmbientIntensity;
		
    return diffuseColor*(NdL1*DirLightColor + NdL2*4*PointLightColor + AmbiColor);
}


///////////////////// TECHNIQUES ////////////////////////////////////////////////////////////////    

technique LambertTest    
{  
    pass Pass0      // Always Start at Pass 0  
    {   
		ZEnable = true;
		ZWriteEnable = true;
		AlphaBlendEnable = false;
		CullMode = ccw;
        VertexShader = compile vs_3_0 VS();   // Vertex Shader Version  
        PixelShader = compile ps_3_0 PS(); 
    }  
} 


struct AnimatedVSIn
{
	float4 pos : POSITION;
	float3 Normal : NORMAL;
	float2 TexCoord : TEXCOORD0; 
	float4 inBoneIndex : BLENDINDICES0;
	float4 inBoneWeight		: BLENDWEIGHT0;
};

struct AnimatedVSOut
{
	float4 Position : POSITION;
    float3 Light : TEXCOORD2;
    float3 Norm : TEXCOORD1;
    float2 TexCoord : TEXCOORD0;
    float4 Position3D : TEXCOORD3;
    float4 Position2D : TEXCOORD4;
};

AnimatedVSOut AnimatedVS(AnimatedVSIn input)
{
	AnimatedVSOut Output = (AnimatedVSOut)1;
	
	float4x3 matSmoothSkin = 0;
    matSmoothSkin += matBones[input.inBoneIndex.x] * input.inBoneWeight.x;
    matSmoothSkin += matBones[input.inBoneIndex.y] * input.inBoneWeight.y;
    matSmoothSkin += matBones[input.inBoneIndex.z] * input.inBoneWeight.z;
    matSmoothSkin += matBones[input.inBoneIndex.w] * input.inBoneWeight.w;
    
    // Combine skin and world transformations
    float4x4 matSmoothSkinWorld = 0;
    matSmoothSkinWorld[0] = float4(matSmoothSkin[0], 0);
    matSmoothSkinWorld[1] = float4(matSmoothSkin[1], 0);
    matSmoothSkinWorld[2] = float4(matSmoothSkin[2], 0);
    matSmoothSkinWorld[3] = float4(matSmoothSkin[3], 1);
    matSmoothSkinWorld = mul(matSmoothSkinWorld, xWorld);
    
    // Transform vertex position and normal
    Output.Position3D = mul(input.pos, matSmoothSkinWorld);
    Output.Position = mul(Output.Position3D, mul(xView, xProjection));
    
    // Transform vertex normal
    Output.Norm = mul(input.Normal, (float3x3)matSmoothSkinWorld);
    
    // Calculate eye vector
    //outEyeVector = matVI[3].xyz - outPosition;
    
    // Texture coordinate
    Output.TexCoord = input.TexCoord;
    
    return Output;
}

    
//I can probably delete the pixel shader and the AnimatedPSOut because it's identical to the other Pixel shader and struct
float4 AnimatedPS(AnimatedVSOut input) : COLOR
{
    float4 diffuseColor = tex2D(TextureSampler, input.TexCoord);
	float3 LightVector = input.Position2D - xCenter;
	float dist = length(LightVector);
	LightVector = normalize(LightVector);
	
	
    if(xTextureEnabled == false)
		diffuseColor = xDiffuseColor;
	float NdL1 = saturate(dot(input.Norm, input.Light));
	float NdL2 = saturate(dot(input.Norm, -LightVector))/dist;
	float4 DirLightColor = xDirectionalColor * xLightIntensity;
	float4 PointLightColor = xPointColor * xPointIntensity;
	float4 AmbiColor = xAmbientColor * xAmbientIntensity;
		
    return diffuseColor*(NdL1*DirLightColor + NdL2*4*PointLightColor + AmbiColor);
}

technique AnimatedLambertTest    
{  
    pass Pass0      // Always Start at Pass 0  
    {   
		ZEnable = true;
		ZWriteEnable = true;
		AlphaBlendEnable = false;
		CullMode = None;
        VertexShader = compile vs_3_0 AnimatedVS();   // Vertex Shader Version  
        PixelShader = compile ps_3_0 AnimatedPS(); 
    }  
} 

////////////////////////ATTENUATED LIGHTSHAFT//////////////////////////

struct LightShaftVS_Out
{
	float4 Position : POSITION;
    float2 Position3D : TEXCOORD0;
    float2 TexCoord : TEXCOORD1;
};

LightShaftVS_Out LightShaftVS(float4 Pos : POSITION, float2 TexCoord : TEXCOORD1 )
{
	LightShaftVS_Out Output = (LightShaftVS_Out)0;
	Output.Position = mul(Pos, mul(xWorld, mul(xView, xProjection)));
	Output.Position3D = mul(Pos, xWorld);
	Output.TexCoord = TexCoord;
	return Output;
}

float4 LightShaftPS(LightShaftVS_Out input) : COLOR0
{
	float4 color; 
	if(xTextureEnabled)
		color = tex2D(TextureSampler, input.TexCoord);
	else
		color = xDiffuseColor;
	//float att = smoothstep(0, xRange, distance(xCenter, input.Position3D));
	return color*0.8;
}

technique LightShaftTest
{  
    pass Pass0      // Always Start at Pass 0  
    {   
		ZEnable = true;
		ZWriteEnable = false;
		AlphaBlendEnable = true;
		SrcBlend = One;
		DestBlend = One;
		CullMode = ccw;
        VertexShader = compile vs_3_0 LightShaftVS();   // Vertex Shader Version  
        PixelShader = compile ps_3_0 LightShaftPS(); 
    }  
} 