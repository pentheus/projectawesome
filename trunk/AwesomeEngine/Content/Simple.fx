//------- XNA-to-HLSL variables --------     
float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float3 xCenter;
float xRange;
float4x4 xWorldViewProjection;  
bool xTextureEnabled;

float4 xAmbientColor = {0,0,1,1};
float xAmbientIntensity = 0.2f;

float4 xDirectionalColor = {0.7,0.7,0.7,1};
float3 xLightDirection = {-1,-1,-1};
float xLightIntensity = 1;

float4 xDiffuseColor = {.4,.4,.4,1};
 
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
 
//------- Structs for Vertex In and Out -------  
 
struct VertexShaderIn  // VertexShaderInput  
{  
    float4 Position                 : POSITION0;      
    float2 textureCoordinates       : TEXCOORD0;  
    float4 Normal					: NORMAL;
};  
 
struct VertexShaderOut // VertexShaderOutput  
{  
    float4 Position                 : POSITION0;   
    float4 Position2D				: TEXCOORD0;
    float4 Center					: TEXCOORD2;
    float2 textureCoordinates       : TEXCOORD1;  
    float4 Normal					: NORMAL0;
};  
 
///////////////////// VERTEX SHADERS /////////////////////////////////////////////////////////////   
 
VertexShaderOut VertexShaderFunction(VertexShaderIn input)  
{  
    VertexShaderOut Output = (VertexShaderOut)0;  
      
    // input.Position refers to struct VertexShaderIn.Position (POSITION0)  
    float4x4 worldViewProjection = mul(xWorld, mul(xView, xProjection));
    Output.Position = mul(input.Position, worldViewProjection);  
    Output.Position2D = mul(input.Position, xWorld);
    Output.textureCoordinates = input.textureCoordinates;  
	Output.Center = mul(xCenter, xWorld);
	Output.Normal = mul(input.Normal, xWorld);
    return Output;  
}  
 
///////////////////// PIXEL SHADERS /////////////////////////////////////////////////////////////     
// The only semantics that a pixel shader can accept as input are COLOR[ n ] and TEXCOORD[ n ].     
/////////////////////////////////////////////////////////////////////////////////////////////////     
 
float4 PixelShaderFunction(VertexShaderOut input) : COLOR0  // Takes input from output of Vertex Shader  
{  
	float4 output;
	float diffuseLightingFactor;
	
	float4 color = tex2D(TextureSampler, input.textureCoordinates);   
    
	if(xTextureEnabled == false)
		color = xDiffuseColor;
	
    
   diffuseLightingFactor = max(0,dot(normalize(input.Normal),-normalize(xLightDirection)));
   diffuseLightingFactor = saturate(diffuseLightingFactor);
   diffuseLightingFactor *= xLightIntensity;
   

   //output = saturate(color*(diffuseLightingFactor) + xAmbientIntensity*xAmbientColor + 0.2*diffuseLightingFactor*xDirectionalColor);
			
	output = color*(diffuseLightingFactor + xAmbientIntensity);
    
    return output;  
}  
 
///////////////////// TECHNIQUES ////////////////////////////////////////////////////////////////    
 
technique Textured    
{  
    pass Pass0      // Always Start at Pass 0  
    {  
		ZEnable = false;  
        ZWriteEnable = false;  
        AlphaBlendEnable = true;  
        SrcBlend = SrcAlpha;  
        DestBlend = One;   
        VertexShader = compile vs_3_0 VertexShaderFunction();   // Vertex Shader Version  
        PixelShader = compile ps_3_0 PixelShaderFunction(); 
    }  
} 

technique Flat    
{  
    pass Pass0      // Always Start at Pass 0  
    {    
        ZEnable = true;  
        ZWriteEnable = true;  
        AlphaBlendEnable = false;  
        
        VertexShader = compile vs_3_0 VertexShaderFunction();   // Vertex Shader Version  
        PixelShader = compile ps_3_0 PixelShaderFunction(); 
    }  
} 