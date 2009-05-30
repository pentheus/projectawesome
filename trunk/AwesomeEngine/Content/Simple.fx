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

float4 xAmbientColor = {0.1,0,0,1};
float xAmbientIntensity = 0.1f;

float4 xDirectionalColor = {0.7,0.7,0.7,1};
float3 xLightDirection = {1,1,1};
float xLightIntensity = 1;

float4 xDiffuseColor = { 1, 0, 0, 1};
 
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
};

/////////////////// VERTEX SHADERS //////////////////////////

VS_OUTPUT VS(float4 Pos : POSITION, float3 Normal : NORMAL, float2 TexCoord : TEXCOORD0)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;
    Out.Pos = mul(Pos, mul(xWorld, mul(xView,xProjection))); // transform Position
    Out.Light = xLightDirection; // output light vector
    Out.Norm = normalize(mul(Normal, xWorld)); // transform       Normal and normalize it
    Out.TexCoord = TexCoord;
    return Out;
}

/////////////////// PIXEL SHADERS ////////////////////////////////

float4 PS(VS_OUTPUT input) : COLOR
{
    float diffuseLightingFactor = saturate(dot(input.Light, input.Norm))*xLightIntensity;
	float4 diffuseColor = tex2D(TextureSampler, input.TexCoord);
    if(xTextureEnabled == false)
		diffuseColor = xDiffuseColor;
    return xAmbientColor*xAmbientIntensity + diffuseColor * diffuseLightingFactor;
}


///////////////////// TECHNIQUES ////////////////////////////////////////////////////////////////    

technique LambertTest    
{  
    pass Pass0      // Always Start at Pass 0  
    {   
		CullMode = None;
        VertexShader = compile vs_3_0 VS();   // Vertex Shader Version  
        PixelShader = compile ps_3_0 PS(); 
    }  
} 


struct AnimatedVSIn
{
	float4 pos : POSITION;
	float3 Normal : NORMAL;
	float2 TexCoord : TEXCOORD2; 
	float4 inBoneIndex : BLENDINDICES0;
	float4 inBoneWeight		: BLENDWEIGHT0;
};

struct AnimatedVSOut
{
	float4 Position			: POSITION;
    float4 Position3D		: TEXCOORD0;
    float3 Norm				: TEXCOORD1;
    float2 TexCoord			: TEXCOORD2;
};

AnimatedVSOut AnimatedVS(AnimatedVSIn input)
{
	AnimatedVSOut Output = (AnimatedVSOut)0;
	
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

    