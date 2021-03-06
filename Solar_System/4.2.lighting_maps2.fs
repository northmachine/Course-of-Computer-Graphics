#version 330 core
out vec4 FragColor;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
	sampler2D li;
    float shininess;
}; 

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec3 FragPos;  
in vec3 Normal;  
in vec2 TexCoords;
  
uniform vec3 viewPos;
uniform Material material;
uniform Light light;

void main()
{
    // ambient
    vec3 ambient = light.ambient * mix(texture(material.li, TexCoords), mix(texture(material.diffuse,TexCoords),texture(material.specular, TexCoords),0.5),0.1).rgb;
  	
    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * mix(texture(material.diffuse, TexCoords), mix(texture(material.specular,TexCoords),texture(material.li, TexCoords),0.1),0.5).rgb -  2 * light.diffuse * diff * texture(material.li, TexCoords).rgb;  
    
    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
     vec3 specular = light.specular * spec * mix(texture(material.diffuse, TexCoords), mix(texture(material.li,TexCoords),texture(material.specular, TexCoords),0.5),0.3).rgb;  

    vec3 result = ambient + diffuse + specular*0.04;
    FragColor = vec4(result, 1.0);
} 