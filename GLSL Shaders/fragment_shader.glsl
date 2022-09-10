#version 330

in vec2 texture_coordinates;
uniform sampler2D texture_sampler;

out vec4 color;

void main(){
	vec4 texture_color = texture(texture_sampler, texture_coordinates);
	color = texture_color;
}