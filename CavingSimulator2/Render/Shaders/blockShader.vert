#version 330 core

uniform mat4 View;
uniform mat4 Projection;
uniform vec3 Offset;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aTexture;
layout (location = 3) in vec3 aOffset;

out vec2 vTexture;
out vec4 vColor;

void main()
{
    vec4 vec = vec4(aPosition + aOffset, 1.0) * View * Projection;
    
    gl_Position = vec;

    vColor = aColor;
    vTexture = aTexture;
}