#version 430
out vec4 FragColor;

struct Material {
    sampler2D diffuse;
    sampler2D specular;    
    float shininess;
}; 

struct Light {
    vec3 position;
	vec3 position2;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec3 FragPos;
in vec2 TexCoords;					//uvWorld
in vec3 Normal;						//normalWorld
in float visibility;
  
uniform vec3 viewPos;
uniform Material material;
uniform Light light;
uniform vec4 FogRealColor;

void main()
{
    // ambient
    vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;
  	
    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;  

	vec3 lightDir2 = normalize(light.position2 - FragPos);
    float diff2 = max(dot(norm, lightDir2), 0.0);
    vec3 diffuse2 = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;  
	diffuse += diffuse2;
    
    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;

	vec3 reflectDir2 = reflect(-lightDir2, norm);  
    float spec2 = pow(max(dot(viewDir, reflectDir2), 0.0), material.shininess);
    vec3 specular2 = light.specular * spec2 * texture(material.specular, TexCoords).rgb;
	specular += specular2;
        
    vec3 result = ambient + diffuse + specular;
    vec4 daColor = vec4(result, 1.0);
	FragColor = mix(FogRealColor, daColor, visibility);
}