#version 330 core

in vec2 pass_PixCoord;

uniform sampler2D indexedTexture;
uniform sampler2D paletteTexture;

uniform float in_zoom;
uniform int   in_ScreenWidth;
uniform float in_ScrollPos;
uniform float in_ScrollHeight;
uniform float in_Brightness;
uniform float in_Contrast;

out vec4 out_Color;

const int TEXTURE_FOLD = 4;
const int TEXTURE_WIDTH = 32768;
const int SPECTRA_HEIGHT = 2048;

const int SPECTRA_WIDTH = TEXTURE_WIDTH * TEXTURE_FOLD;
const int TEXTURE_HEIGHT = SPECTRA_HEIGHT * TEXTURE_FOLD;

float sample_texture(int x, int y)
{
  // a scpectrum is stored in 4 rows of texture.
  // convert index-into-spectra to index-into-texture
  int texelX = x % TEXTURE_WIDTH;
	int texelY = y * TEXTURE_FOLD + x / TEXTURE_WIDTH;

  return texelFetch(indexedTexture, ivec2(texelX, texelY), 0).r;
}

void main(void) 
{
  // y coordinate in the spectra after scrolling
  float spectraYfloat = fract(2 - pass_PixCoord.t * in_ScrollHeight + in_ScrollPos);
  int spectraY = int(SPECTRA_HEIGHT * spectraYfloat);	
	
	// texels per pixel: stretch or shrink factor
	float scale = SPECTRA_WIDTH / float(in_ScreenWidth * in_zoom);	
	float luminance = 0;

	if (scale > 1)
	{
    // shrink: take the max of all texels in the range
	  int pixelX = int(pass_PixCoord.s * in_ScreenWidth * in_zoom);
  	int spectraXstart = int(pixelX * scale);
	  int spectraXstop = int((pixelX + 1) * scale);
	  for (int spectraX = spectraXstart; spectraX < spectraXstop; spectraX++)
	    luminance = max(luminance, sample_texture(spectraX, spectraY));	
	}
	else
	{
		// stretch: interpolate between 2 texels
		float spectraX = pass_PixCoord.s * SPECTRA_WIDTH;
		int spectraXint = int(spectraX);
		float a = spectraX - spectraXint;
		float v1 = sample_texture(spectraXint, spectraY);
	  float v2 = sample_texture(spectraXint+1, spectraY);
		luminance = v1 * (1-a) + v2 * a;
	}

	// apply brightness and contrast	
	luminance = in_Brightness + luminance * in_Contrast;

	// get color from palette
	out_Color = texture(paletteTexture, vec2(luminance, 0));
}