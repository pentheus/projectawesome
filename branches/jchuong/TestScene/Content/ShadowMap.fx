float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 LightWorldViewProjection;

// TODO: add effect parameters here.


//This is the output for the Shadow Map Vertex Shader
struct ShadowVSOutput
{
    float4 Position : POSITION;
	float Depth : TEXCOORD0;
};

struct SMapPixel
{
	float4 Color : COLOR0;
};

ShadowVSOutput ShadowMapVertexShader(float4 inPos : POSITION)
{
	ShadowVSOutput Output = (ShadowVSOutput)0;
	
	Output.Position = mul(inPos, LightWorldViewProjection);
	Output.Depth = Output.Position.z/Output.Position.w;
	
	return Output;
}

SMapPixel ShadowMapPixelShader(ShadowVSOutput input)
{
	SMapPixel Output = (SMapPixel)0;

	Output.Color = float4(input.Depth,input.Depth,input.Depth,1.0f);
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
