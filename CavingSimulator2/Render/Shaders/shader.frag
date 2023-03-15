#version 330 core

in vec4 vColor;
in vec2 vTexture;
in float vBrightness;

out vec4 finalColor;

uniform sampler2D Texture;
uniform mat4 Model;

void main(){

    vec4 surfaceColor = texture(Texture, vTexture);
    finalColor = vec4(vBrightness * vec3(vColor) * surfaceColor.rgb, surfaceColor.a);

}
