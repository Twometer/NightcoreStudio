#version 330 core

layout(location = 0) in vec2 vertexPosition;
layout(location = 1) in vec2 vertexTextureCoords;

out vec2 textureCoords;

uniform mat4 modelMatrix;
uniform mat4 projectionMatrix;

void main(void) {
	textureCoords = vertexTextureCoords;
	
    mat4 mvp = projectionMatrix * modelMatrix;
    gl_Position = mvp * vec4(vertexPosition, 0.0, 1.0);

}