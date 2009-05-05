float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 WorldViewProjection;
float4x4 LightWorldViewProjection;

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

technique CreateShadowMap
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 ShadowMapVertexShader();
        PixelShader = compile ps_3_0 ShadowMapPixelShader();
    }
}

