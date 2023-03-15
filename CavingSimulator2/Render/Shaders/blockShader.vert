#version 330 core

uniform mat4 View;
uniform mat4 Projection;
uniform vec3 LightPos;
uniform float MeshSize;


layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aOffset; // par
layout (location = 2) in vec2 aTexture; 
layout (location = 3) in float aTextureOffset; // par
layout (location = 4) in vec3 aNormal; 

out vec2 vTexture;
out float vBrightness;

void main()
{
    gl_Position = vec4(aPosition + aOffset, 1.0) * View * Projection;

    vTexture = aTexture + vec2(0,aTextureOffset*MeshSize);
    vBrightness =  ( dot(aNormal, LightPos) + 1 ) / 2 * 0.7 + 0.3;
}