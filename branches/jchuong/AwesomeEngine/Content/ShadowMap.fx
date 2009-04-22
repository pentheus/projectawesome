float4x4 World;
float4x4 View;
float4x4 Projection;

float4x4 LightWorld;


// TODO: add effect parameters here.


//This is the output for the Shadow Map Vertex Shader
struct ShadowVSOutput
{
    float4 Position : POSITION0;
	float4 Position2D : TEXCOORD0;
};

struct SMapPixel
{
	float4 Depth : COLOR0;
};

ShadowVSOutput ShadowMapVertexShader(float4 inPos : POSITION)
{
	ShadowVSOutput Output = (ShadowVSOutput)0;
	
	Output.Position = mul(inPos, LightWorld);
	Output.Position2D = Output.Position;
	
	return Output;
}

SMapPixel ShadowMapPixelShader(ShadowVSOutput input)
{
	SMapPixel Output = (SMapPixel)0;
	
	Output.Depth = input.Position2D.z/input.Position2D.w;
	
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
