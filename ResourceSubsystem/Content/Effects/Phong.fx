#define FLIP_TEXTURE_Y

float Script : STANDARDSGLOBAL = 0.8;

//// Standardowe zmienne
float4x4 World : World;
float4x4 WorldViewProjection : WorldViewProjection;
float4x4 WorldInverseTranspose : WorldInverseTranspose;
float4x4 ViewInverse : ViewInverse;

//// Swiatlo punktowe
float3 Lamp0Pos : Position = {10.0f, 5.0f, -10.0f};
float3 Lamp0Color : Specular = {2.0f, 2.0f, 2.0f};

//// Ambient
float3 AmbiColor : Ambient = {0.07f, 0.07f, 0.07f};
float Ks = 0.4;
float SpecExpon : SpecularPower = 55.0;

//// Spot light
float constantAttenuation = 0.0f;
float linearAttenuation = 0.5f;
float quadricAttenuation = 0.0f;

//// Tekstura koloru
texture ColorTexture : DIFFUSE;
sampler2D ColorSampler = sampler_state
{
    Texture = <ColorTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};  

//// Dane wierzcholka
struct vertexInput
{
    float3 Position	: POSITION;
    float4 UV		: TEXCOORD0;
    float4 Normal	: NORMAL;
    float4 Tangent	: TANGENT0;
    float4 Binormal	: BINORMAL0;
};

//// Wyjscie z Vertex shadera
struct vertexOutput
{
    float4 HPosition		: POSITION;
    float2 UV				: TEXCOORD0;
    float3 LightVec			: TEXCOORD1;
    float3 WorldNormal		: TEXCOORD2;
    float3 WorldTangent		: TEXCOORD3;
    float3 WorldBinormal	: TEXCOORD4;
    float3 WorldView		: TEXCOORD5;
};
 
//// VERTEX SHADER
vertexOutput VertexShaderFunction(vertexInput input) 
{
    vertexOutput output = (vertexOutput)0;
    
    output.WorldNormal = mul(input.Normal, WorldInverseTranspose).xyz;
    output.WorldTangent = mul(input.Tangent, WorldInverseTranspose).xyz;
    output.WorldBinormal = mul(input.Binormal, WorldInverseTranspose).xyz;
    
    float4 Po = float4(input.Position.xyz, 1);
    float3 Pw = mul(Po, World).xyz;
    output.LightVec = (Lamp0Pos - Pw);

#ifdef FLIP_TEXTURE_Y
    output.UV = float2(input.UV.x, (1.0 - input.UV.y));
#else
    output.UV = input.UV.xy;
#endif

    output.WorldView = normalize(ViewInverse[3].xyz - Pw);
    output.HPosition = mul(Po, WorldViewProjection);
    
    return output;
}

//// PIXEL SHADER
void phong_shading(vertexOutput input,
		    float3 LightColor,
		    float3 Nn,
		    float3 Ln,
		    float3 Vn,
		    out float3 DiffuseContrib,
		    out float3 SpecularContrib)
{
    float3 Hn = normalize(Vn + Ln);
    float4 litV = lit(dot(Ln,Nn), dot(Hn,Nn), SpecExpon);
    
	float lightDist = length(litV);
	float attenuation = 1.0 / (constantAttenuation +
							   linearAttenuation * lightDist +
							   quadricAttenuation * lightDist * lightDist);
    
    DiffuseContrib = litV.y * LightColor * attenuation;
    SpecularContrib = litV.y * litV.z * Ks * LightColor;
}

float4 PixelShaderFunction(vertexOutput input) : COLOR 
{
    float3 diffContrib;
    float3 specContrib;
    
    float3 Ln = normalize(input.LightVec);
    float3 Vn = normalize(input.WorldView);
    float3 Nn = normalize(input.WorldNormal);
    
	phong_shading(input, Lamp0Color, Nn, Ln, Vn, diffContrib, specContrib);
	
    float3 diffuseColor = tex2D(ColorSampler, input.UV).rgb;
    float3 result = specContrib + (diffuseColor*(diffContrib + AmbiColor));
    return float4(result, 1);
}

//// Definicja technik
technique Technique1
{
    pass Pass1
    {
		ZEnable = true;
		ZWriteEnable = true;
		ZFunc = LessEqual;
		AlphaBlendEnable = false;
		CullMode = None;
        VertexShader = compile vs_2_0 VertexShaderFunction();		
        PixelShader = compile ps_2_a PixelShaderFunction();
    }
}

/////////////////////////////////////// eof //
