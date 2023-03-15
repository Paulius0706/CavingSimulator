#version 330 core

uniform mat4 View;
uniform mat4 Projection;
uniform vec3 Offset;
uniform float MeshSize;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aTexture;
layout (location = 3) in vec3 aOffset;
layout (location = 4) in float aTextureOffset;
layout (location = 5) in float aVertexIndex;

out vec2 vTexture;
out vec4 vColor;

void main()
{
    vec4 vec = vec4(aPosition + aOffset, 1.0) * View * Projection;
    
    gl_Position = vec;
     
    if (    aVertexIndex == 0 ) { vColor = vec4(aColor.x,aColor.x,aColor.x,1);}
    else if(aVertexIndex == 1 ) { vColor = vec4(aColor.y,aColor.y,aColor.y,1);}
    else if(aVertexIndex == 2 ) { vColor = vec4(aColor.z,aColor.z,aColor.z,1);}
    else if(aVertexIndex == 3 ) { vColor = vec4(aColor.w,aColor.w,aColor.w,1);}
    else { vColor = vec4(0,0,0,1);}
    //vColor = aColor;
    vTexture = aTexture + vec2(0,aTextureOffset*MeshSize);
}