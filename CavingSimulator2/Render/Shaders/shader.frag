#version 330 core

in vec4 vColor;
in vec2 vTexture;
in vec3 vNormal;
in vec3 vFragPos;

out vec4 finalColor;

uniform sampler2D Texture;
uniform vec3 LightPos;
uniform mat4 Model;

void main(){

    mat3 normalMatrix = transpose(inverse(mat3(Model)));
    vec3 normal = normalize(normalMatrix * vNormal);
    
    vec3 surfaceToLight = LightPos - vec3(Model * vec4(vFragPos, 1));

    float brightness = ((dot(normal, surfaceToLight) / (length(surfaceToLight) * length(normal))) + 1) * 0.5 * 0.8 + 0.2;
    //brightness = (dot(normal, normalize(surfaceToLight))) - 0.4f;
    
    brightness = clamp(brightness, 0, 1);
    //brightness = min(brightness+0.2,1);

    vec4 surfaceColor = texture(Texture, vTexture);
    finalColor = vec4(brightness * vec3(vColor) * surfaceColor.rgb, surfaceColor.a);

}
