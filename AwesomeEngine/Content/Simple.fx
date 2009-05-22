//------- XNA-to-HLSL variables --------     
float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float3 xCenter;
float xRange;
float4x4 xWorldViewProjection;  
 
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
    float4 Position                 : POSITION;      
    float2 textureCoordinates       : TEXCOORD0;  
};  
 
struct VertexShaderOut // VertexShaderOutput  
{  
    float4 Position                 : POSITION;   
    float4 Position2D				: TEXCOORD0;
    float4 Center					: TEXCOORD2;
    float2 textureCoordinates       : TEXCOORD1;  
};  
 
///////////////////// VERTEX SHADERS /////////////////////////////////////////////////////////////   
 
VertexShaderOut VertexShaderFunction(VertexShaderIn input)  
{  
    VertexShaderOut Output = (VertexShaderOut)0;  
      
    // input.Position refers to struct VertexShaderIn.Position (POSITION0)  
    float4x4 worldViewProjection = mul(xWorld, mul(xView, xProjection));
    Output.Position = mul(input.Position, worldViewProjection);  
    Output.textureCoordinates = input.textureCoordinates;  
	Output.Center = mul(xCenter, worldViewProjection);
	//Output.att = distance(Output.Center,Output.Position)/xRange;
    return Output;  
}  
 
///////////////////// PIXEL SHADERS /////////////////////////////////////////////////////////////     
// The only semantics that a pixel shader can accept as input are COLOR[ n ] and TEXCOORD[ n ].     
/////////////////////////////////////////////////////////////////////////////////////////////////     
 
float4 PixelShaderFunction(VertexShaderOut input) : COLOR0  // Takes input from output of Vertex Shader  
{  
	float4 output;
	float4 color = tex2D(TextureSampler, input.textureCoordinates);   
    float att = saturate(xRange/distance(input.Center, input.Position2D));
    //output.a = 0.3; 
    
    output = color*att;
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