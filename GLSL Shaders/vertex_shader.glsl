#version 330

layout(location=0) in vec3 position;
layout(location=1) in vec2 texture_coords;

out vec2 texture_coordinates;

uniform mat4 projection = mat4(1);
uniform mat4 camera = mat4(1);

void main(){
	texture_coordinates = texture_coords;
	gl_Position = projection * camera * vec4(position, 1.0);
}