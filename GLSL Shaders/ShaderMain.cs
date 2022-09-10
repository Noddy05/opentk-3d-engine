using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GLSL_Shaders
{
    public struct Shader
    {
        public int id;
    }

    public struct ShaderProgram
    {
        public int id;
    }

    public class ShaderMain
    {
        public static Shader Load(string shaderLocation, ShaderType shaderType)
        {
            int shaderId = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderId, File.ReadAllText(shaderLocation));
            GL.CompileShader(shaderId);
            string infoLog = GL.GetShaderInfoLog(shaderId);

            if (string.IsNullOrEmpty(infoLog))
                return new Shader() { id = shaderId };

            throw new Exception(infoLog);
        }

        public static ShaderProgram LoadProgram(string vertexShaderLocation, string fragmentShaderLocation)
        {
            int programId = GL.CreateProgram();

            Shader vertexShader = Load(vertexShaderLocation, ShaderType.VertexShader);
            Shader fragmentShader = Load(fragmentShaderLocation, ShaderType.FragmentShader);

            GL.AttachShader(programId, vertexShader.id);
            GL.AttachShader(programId, fragmentShader.id);
            GL.LinkProgram(programId);

            //Shaders are linked to program, now you can dispose of them
            GL.DetachShader(programId, vertexShader.id);
            GL.DetachShader(programId, fragmentShader.id);
            GL.DeleteShader(vertexShader.id);
            GL.DeleteShader(fragmentShader.id);

            string infoLog = GL.GetProgramInfoLog(programId);

            if (string.IsNullOrEmpty(infoLog))
                return new ShaderProgram() { id = programId };

            throw new Exception(infoLog);
        }


    }
}
