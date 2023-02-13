#version 330 core

in vec4 vColor;
in vec2 vTexture;
in vec3 vFragPos;

out vec4 finalColor;

uniform sampler2D Texture;

void main(){

    finalColor = vColor * texture(Texture, vTexture);
}
