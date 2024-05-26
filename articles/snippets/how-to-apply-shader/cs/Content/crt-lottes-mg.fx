// Found at https://github.com/fhb93/John-Cricket-Fishing-Game/blob/5ee7524df3f8c0a8d43448de2b4043faa1a55f49/JohnCricketFishingGame/Content/Assets/Shaders/crt-lottes-mg.fx

#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0 //0_level_9_1
#define PS_SHADERMODEL ps_5_0 //0_level_9_1
#define HLSL_4
#endif

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 texCoord : TEXCOORD0;
};

//
// PUBLIC DOMAIN CRT STYLED SCAN-LINE SHADER
//
//   by Timothy Lottes
//
// This is more along the style of a really good CGA arcade monitor.
// With RGB inputs instead of NTSC.
// The shadow mask example has the mask rotated 90 degrees for less chromatic aberration.
//
// Left it unoptimized to show the theory behind the algorithm.
//
// It is an example what I personally would want as a display option for pixel art games.
// Please take and use, change, or whatever.
//

// -- config  -- //
// #pragma parameter hardScan "hardScan" -8.0 -20.0 0.0 1.0 // default, minimum, maximum, optional step
// #pragma parameter hardPix "hardPix" -3.0 -20.0 0.0 1.0
// #pragma parameter warpX "warpX" 0.031 0.0 0.125 0.01
// #pragma parameter warpY "warpY" 0.041 0.0 0.125 0.01
// #pragma parameter maskDark "maskDark" 0.5 0.0 2.0 0.1
// #pragma parameter maskLight "maskLight" 1.5 0.0 2.0 0.1
// #pragma parameter scaleInLinearGamma "scaleInLinearGamma" 1.0 0.0 1.0 1.0
// #pragma parameter shadowMask "shadowMask" 3.0 0.0 4.0 1.0
// #pragma parameter brightboost "brightness" 1.0 0.0 2.0 0.05
// #pragma parameter hardBloomPix "bloom-x soft" -1.5 -2.0 -0.5 0.1
// #pragma parameter hardBloomScan "bloom-y soft" -2.0 -4.0 -1.0 0.1
// #pragma parameter bloomAmount "bloom amt" 0.15 0.0 1.0 0.05
// #pragma parameter shape "filter kernel shape" 2.0 0.0 10.0 0.05

float hardScan;
float hardPix;
float warpX;
float warpY;
float maskDark;
float maskLight;
float scaleInLinearGamma;
#define shadowMask 3
float brightboost;
float hardBloomScan;
float hardBloomPix;
float bloomAmount;
float shape;

float2 textureSize;
float2 videoSize;
float2 outputSize;

float2 warp;

//Uncomment to reduce instructions with simpler linearization
//(fixes HD3000 Sandy Bridge IGP)
#define SIMPLE_LINEAR_GAMMA 2
#define DO_BLOOM 1

// ------------- //

Texture2D decal;
sampler2D DecalSampler = sampler_state
{
	Texture = <decal>;
};

float4x4 modelViewProj;

//------------------------------------------------------------------------

// sRGB to Linear.
// Assuing using sRGB typed textures this should not be needed.
#if SIMPLE_LINEAR_GAMMA > 0

float3 ToSrgb(float3 c)
{
#if SIMPLE_LINEAR_GAMMA == 1
	return pow(c, 1 / 2.2);
#else
	return sqrt(c);
#endif
}
#else
float ToLinear1(float c)
{
	if (scaleInLinearGamma == 0) return c;
	return(c <= 0.04045) ? c / 12.92 : pow((c + 0.055) / 1.055, 2.4);
}
float3 ToLinear(float3 c)
{
	if (scaleInLinearGamma == 0) return c;
	return float3(ToLinear1(c.r), ToLinear1(c.g), ToLinear1(c.b));
}

// Linear to sRGB.
// Assuming using sRGB typed textures this should not be needed.
float ToSrgb1(float c)
{
	if (scaleInLinearGamma == 0) return c;
	return(c < 0.0031308 ? c * 12.92 : 1.055*pow(c, 0.41666) - 0.055);
}

float3 ToSrgb(float3 c)
{
	if (scaleInLinearGamma == 0) return c;
	return float3(ToSrgb1(c.r), ToSrgb1(c.g), ToSrgb1(c.b));
}
#endif

// Philisoft Web safe Shader
//
float MyClamp(float input)
{
    return input > 255 ? 255 : input;
}

float CheckRange(int float4intColor)
{
    float temp = 0;
    
    if (float4intColor > 0)
    {
        temp = 51;
        
        if (float4intColor > 51)
        {
            temp = 102;
            
            if (float4intColor > 102)
            {
                temp = 153;
                
                if (float4intColor > 153)
                {
                    temp = 204;
                    
                    if (float4intColor > 204)
                    {
                        temp = 255;
                    }
                }
            }
        }
    }
    
    return temp / 256;
    
}

float4 webColor(float4 inputColor)
{
    int i, j;
    
    int4 float4intColor = int4(inputColor.r * 256, inputColor.g * 256, inputColor.b * 256, inputColor.a * 256);
    
    float4 temp = float4(MyClamp(CheckRange(float4intColor.r)), MyClamp(CheckRange(float4intColor.g)), MyClamp(CheckRange(float4intColor.b)), MyClamp(CheckRange(float4intColor.a)));
    //float4(CheckRange(MyClamp(float4intColor.x)), CheckRange(MyClamp(float4intColor.y)), CheckRange(MyClamp(float4intColor.z)), CheckRange(MyClamp(float4intColor.w)));
       
    return temp;
 
}

//End of Philisoft Web Safe Shader

//Philisoft World Decay Shader

float4 earthClamp(int4 inputColor)
{
    return float4(0.2, 0.2, 0, inputColor.a / 256.0);
}

float4 decayEarth(float4 inputColor)
{
    int4 intInput = int4(inputColor.r * 255, inputColor.g * 255, inputColor.b * 255, inputColor.a * 255);
    
    if (intInput.r < intInput.g)
    {
        if (intInput.g > intInput.b)
        {
            return earthClamp(intInput);
        }
    }
    return inputColor;
}
//End of Philisoft World Decay Shader


// Nearest emulated sample given floating point position and texel offset.
// Also zero's off screen.
float3 Fetch(float2 pos, float2 off, float2 texture_size) {
	pos = (floor(pos*texture_size.xy + off) + float2(0.5, 0.5)) / texture_size.xy;
#if SIMPLE_LINEAR_GAMMA == 1
	return brightboost * pow(tex2D(DecalSampler, pos.xy).rgb, 2.2);
#elif SIMPLE_LINEAR_GAMMA == 2
	return brightboost * pow(tex2D(DecalSampler, pos.xy).rgb, 2);
#else
	return ToLinear(brightboost * tex2D(DecalSampler, pos.xy).rgb);
#endif
}

// Distance in emulated pixels to nearest texel.
float2 Dist(float2 pos, float2 texture_size) { pos = pos * texture_size.xy; return -(frac(pos) - float2(0.5, 0.5)); }

// 1D Gaussian.
float Gaus(float pos, float scale) { return exp2(scale*pos*pos); }

// 3-tap Gaussian filter along horz line.
float3 Horz3(float2 pos, float off, float2 texture_size) {
	float3 b = Fetch(pos, float2(-1.0, off), texture_size);
	float3 c = Fetch(pos, float2(0.0, off), texture_size);
	float3 d = Fetch(pos, float2(1.0, off), texture_size);
	float dst = Dist(pos, texture_size).x;
	// Convert distance to weight.
	float scale = hardPix;
	float wb = Gaus(dst - 1.0, scale);
	float wc = Gaus(dst + 0.0, scale);
	float wd = Gaus(dst + 1.0, scale);
	// Return filtered sample.
	return (b*wb + c * wc + d * wd) / (wb + wc + wd);
}

// 5-tap Gaussian filter along horz line.
float3 Horz5(float2 pos, float off, float2 texture_size) {
	float3 a = Fetch(pos, float2(-2.0, off), texture_size);
	float3 b = Fetch(pos, float2(-1.0, off), texture_size);
	float3 c = Fetch(pos, float2(0.0, off), texture_size);
	float3 d = Fetch(pos, float2(1.0, off), texture_size);
	float3 e = Fetch(pos, float2(2.0, off), texture_size);
	float dst = Dist(pos, texture_size).x;
	// Convert distance to weight.
	float scale = hardPix;
	float wa = Gaus(dst - 2.0, scale);
	float wb = Gaus(dst - 1.0, scale);
	float wc = Gaus(dst + 0.0, scale);
	float wd = Gaus(dst + 1.0, scale);
	float we = Gaus(dst + 2.0, scale);
	// Return filtered sample.
	return (a*wa + b * wb + c * wc + d * wd + e * we) / (wa + wb + wc + wd + we);
}

// 7-tap Gaussian filter along horz line.
float3 Horz7(float2 pos, float off, float2 texture_size) {
	float3 a = Fetch(pos, float2(-3.0, off), texture_size);
	float3 b = Fetch(pos, float2(-2.0, off), texture_size);
	float3 c = Fetch(pos, float2(-1.0, off), texture_size);
	float3 d = Fetch(pos, float2(0.0, off), texture_size);
	float3 e = Fetch(pos, float2(1.0, off), texture_size);
	float3 f = Fetch(pos, float2(2.0, off), texture_size);
	float3 g = Fetch(pos, float2(3.0, off), texture_size);
	float dst = Dist(pos, texture_size).x;
	// Convert distance to weight.
	float scale = hardBloomPix;
	float wa = Gaus(dst - 3.0, scale);
	float wb = Gaus(dst - 2.0, scale);
	float wc = Gaus(dst - 1.0, scale);
	float wd = Gaus(dst + 0.0, scale);
	float we = Gaus(dst + 1.0, scale);
	float wf = Gaus(dst + 2.0, scale);
	float wg = Gaus(dst + 3.0, scale);
	// Return filtered sample.
	return (a*wa + b * wb + c * wc + d * wd + e * we + f * wf + g * wg) / (wa + wb + wc + wd + we + wf + wg);
}

// Return scanline weight.
float Scan(float2 pos, float off, float2 texture_size) {
	float dst = Dist(pos, texture_size).y;
	return Gaus(dst + off, hardScan);
}

// Return scanline weight for bloom.
float BloomScan(float2 pos, float off, float2 texture_size) {
	float dst = Dist(pos, texture_size).y;
	return Gaus(dst + off, hardBloomScan);
}

// Allow nearest three lines to effect pixel.
float3 Tri(float2 pos, float2 texture_size) {
	float3 a = Horz3(pos, -1.0, texture_size);
	float3 b = Horz5(pos, 0.0, texture_size);
	float3 c = Horz3(pos, 1.0, texture_size);
	float wa = Scan(pos, -1.0, texture_size);
	float wb = Scan(pos, 0.0, texture_size);
	float wc = Scan(pos, 1.0, texture_size);
	return a * wa + b * wb + c * wc;
}

// Small bloom.
float3 Bloom(float2 pos, float2 texture_size) {
	float3 a = Horz5(pos, -2.0, texture_size);
	float3 b = Horz7(pos, -1.0, texture_size);
	float3 c = Horz7(pos, 0.0, texture_size);
	float3 d = Horz7(pos, 1.0, texture_size);
	float3 e = Horz5(pos, 2.0, texture_size);
	float wa = BloomScan(pos, -2.0, texture_size);
	float wb = BloomScan(pos, -1.0, texture_size);
	float wc = BloomScan(pos, 0.0, texture_size);
	float wd = BloomScan(pos, 1.0, texture_size);
	float we = BloomScan(pos, 2.0, texture_size);
	return a * wa + b * wb + c * wc + d * wd + e * we;
}

float2 Warp(float2 pos)
{
    pos = pos * 2.0 - 1.0;
    pos *= float2(1.0 + (pos.y * pos.y) * warp.x, 1.0 + (pos.x * pos.x) * warp.y);
    return pos * 0.5 + 0.5;
}

// Shadow mask 
float3 Mask(float2 pos) {
	float3 mask = maskDark;

	// Very compressed TV style shadow mask.
#if shadowMask == 1
	float mask_line = maskLight;
	float odd = 0.0;
	if (frac(pos.y / 6.0) < 0.5) odd = 1.0; // was pos.x
	if (frac((pos.y + odd) / 2.0) < 0.5) mask_line = maskDark;
	pos.y = frac(pos.y / 3.0); // was pos.x

	if (pos.y < 0.333)mask.r = maskLight; // was pos.x
	else if (pos.y < 0.666)mask.g = maskLight; // was pos.x
	else mask.b = maskLight;
	mask *= mask_line;

#elif shadowMask == 2

  // Aperture-grille.
	pos.x = frac(pos.x / 3.0);

	if (pos.x < 0.333)mask.r = maskLight;
	else if (pos.x < 0.666)mask.g = maskLight;
	else mask.b = maskLight;

#elif shadowMask == 3
  // Stretched VGA style shadow mask (same as prior shaders).
	pos.x += pos.y*3.0;
	pos.x = frac(pos.x / 6.0);

	if (pos.x < 0.333)mask.r = maskLight;
	else if (pos.x < 0.666)mask.g = maskLight;
	else mask.b = maskLight;

	// VGA style shadow mask.
#else
	pos.xy = floor(pos.xy*float2(1.0, 0.5));
	pos.x += pos.y*3.0;
	pos.x = frac(pos.x / 6.0);

	if (pos.x < 0.333)mask.r = maskLight;
	else if (pos.x < 0.666)mask.g = maskLight;
	else mask.b = maskLight;
#endif

	return mask;
}

float4 crt_lottes(float2 tex)
{
    
    float2 pos = Warp(tex.xy);
    
    //pos =
    
	float3 outColor = Tri(pos, textureSize);

#ifdef DO_BLOOM
	//Add Bloom
	outColor.rgb += Bloom(pos, textureSize)*bloomAmount;
#endif

#if shadowMask != 0
	outColor.rgb *= Mask(floor(tex.xy*outputSize.xy) + 0.5);
#endif

    //float4 test = decayEarth(float4(outColor, 1));
    //return webColor(float4(outColor, 1));
    return float4(ToSrgb(outColor.rgb), 1);
    //return float4(ToSrgb(test.rgb), 1);
}

float4 main_fragment(VertexShaderOutput VOUT) : COLOR0
{
    return crt_lottes(VOUT.texCoord);
}


technique
{
	pass
    {
        PixelShader = compile PS_SHADERMODEL main_fragment();
    }
}