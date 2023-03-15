#version 330 core

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform vec3 LightPos;



layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aTexture;
layout (location = 3) in vec3 aNormal;

out vec4 vColor;
out vec2 vTexture;
out float vBrightness;


void main()
{
    gl_Position = vec4(aPosition, 1.0) * Model * View * Projection;
    
    vTexture = aTexture;
    vColor = aColor;
    vBrightness = 
        (
            dot(
                normalize(vec3(vec4(aNormal,1) * Model - vec4(0,0,0,1) * Model)), 
                LightPos) 
            + 1
        ) 
        / 2 * 0.8 + 0.2;
}