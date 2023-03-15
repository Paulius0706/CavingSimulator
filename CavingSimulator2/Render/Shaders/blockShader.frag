#version 330 core

in vec2 vTexture;
in float vBrightness;

out vec4 finalColor;

uniform sampler2D Texture;

void main(){

    vec4 surfaceColor = texture(Texture, vTexture);
    finalColor = vec4(vBrightness * surfaceColor.rgb, surfaceColor.a);

}
