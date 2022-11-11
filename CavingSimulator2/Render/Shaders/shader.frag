#version 330 core

in vec4 vColor;
in vec2 vTexture;
out vec4 pixelColor;

uniform sampler2D Texture;

void main(){

    //pixelColor = texture(Texture, vTexture);
    pixelColor = texture(Texture, vTexture) * vColor;
    //pixelColor = vColor;
}
