#version 330 core

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;


layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aTexture;
layout (location = 3) in vec3 aNormal;

out vec3 vFragPos;
out vec4 vColor;
out vec2 vTexture;
out vec3 vNormal;


void main()
{
    gl_Position = vec4(aPosition, 1.0) * Model * View * Projection;
    
    vTexture = aTexture;
    vFragPos = aPosition;
    vColor = aColor;
    vNormal = aNormal;
}