#version 330 core

uniform float Depth;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aTexture;

out vec3 vFragPos;
out vec4 vColor;
out vec2 vTexture;


void main()
{
    gl_Position = vec4(aPosition, Depth);
    
    vTexture = aTexture;
    vFragPos = aPosition;
    vColor = aColor;
}
