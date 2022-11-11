using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CavingSimulator2.Render
{
    public readonly struct ShaderUniform
    {
        public readonly string Name;
        public readonly int Location;
        public readonly ActiveUniformType Type;

        public ShaderUniform(string Name, int Location, ActiveUniformType Type)
        {
            this.Name = Name;
            this.Location = Location;
            this.Type = Type;
        }
    }
    public readonly struct ShaderAttribute
    {
        public readonly string Name;
        public readonly int Location;
        public readonly ActiveAttribType Type;

        public ShaderAttribute(string Name, int Location, ActiveAttribType Type)
        {
            this.Name = Name;
            this.Location = Location;
            this.Type = Type;
        }
    }

    public sealed class ShaderProgram : IDisposable
    {
        private bool disposed;

        public readonly string name;
        public readonly int ShaderProgramHandle;

        public readonly int VertexShaderHandle;
        public readonly int FragmentShaderHandle;

        private readonly ShaderUniform[] shaderUniforms;
        private readonly ShaderAttribute[] shaderAttributes;



        public ShaderProgram(string vertexShaderPath = "shader.vert", string fragmentShaderPath = "shader.frag")
        {
            this.name = name;
            string errorMessage;

            if (!CompileVertexShader(out VertexShaderHandle, out errorMessage, vertexShaderPath)) { throw new ArgumentException(errorMessage); }
            if (!CompileFragmentShader(out FragmentShaderHandle, out errorMessage, fragmentShaderPath)) { throw new ArgumentException(errorMessage); }

            ShaderProgramHandle = CreateLinkProgram(VertexShaderHandle, FragmentShaderHandle);

            shaderUniforms = CreateUniformList(ShaderProgramHandle);
            shaderAttributes = CreateAtributeList(ShaderProgramHandle);
        }
        ~ShaderProgram()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            GL.DeleteShader(VertexShaderHandle);
            GL.DeleteShader(FragmentShaderHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(ShaderProgramHandle);

            GC.SuppressFinalize(this);
        }
        public ShaderUniform[] GetShaderUniforms()
        { ShaderUniform[] arr = new ShaderUniform[shaderUniforms.Length]; Array.Copy(shaderUniforms, arr, shaderUniforms.Length); return arr; }
        public ShaderAttribute[] GetShaderAttributes()
        { ShaderAttribute[] arr = new ShaderAttribute[shaderAttributes.Length]; Array.Copy(shaderAttributes, arr, shaderAttributes.Length); return arr; }



        public void SetUniform(string name, int value)
        {
            if (!GetShaderUniform(name, out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + name); }
            if (shaderUniform.Type != ActiveUniformType.Int && shaderUniform.Type != ActiveUniformType.Sampler2D) { throw new ArgumentException("uniform is not float. it is " + shaderUniform.Type); }

            GL.Uniform1(shaderUniform.Location, value);
        }
        public void SetUniform(string name, float value)
        {
            if (!GetShaderUniform(name, out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + name); }
            if (shaderUniform.Type != ActiveUniformType.Float) { throw new ArgumentException("uniform is not float. it is " + shaderUniform.Type); }

            GL.Uniform1(shaderUniform.Location, value);
        }

        public void SetUniform(string name, float value1, float value2)
        {
            if (!GetShaderUniform(name, out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + name); }
            if (shaderUniform.Type != ActiveUniformType.FloatVec2) { throw new ArgumentException("uniform is not floatVec2"); }

            GL.Uniform2(shaderUniform.Location, new Vector2((float)value1, (float)value2));
        }
        public void SetUniform(string name, float value1, float value2, float value3)
        {
            if (!GetShaderUniform(name, out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + name); }
            if (shaderUniform.Type != ActiveUniformType.FloatVec3) { throw new ArgumentException("uniform is not floatVec3"); }

            GL.Uniform3(shaderUniform.Location, new Vector3((float)value1, (float)value2, (float)value3));
        }
        public void SetUniform(string name, Vector3 value)
        {
            if (!GetShaderUniform(name, out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + name); }
            if (shaderUniform.Type != ActiveUniformType.FloatVec3) { throw new ArgumentException("uniform is not floatVec3"); }

            GL.Uniform3(shaderUniform.Location, value);
        }
        public void SetUniform(string name, ref Matrix4 value)
        {
            if (!GetShaderUniform(name, out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + name); }
            if (shaderUniform.Type != ActiveUniformType.FloatMat4) { throw new ArgumentException("uniform is not floatMarix4"); }

            GL.UniformMatrix4(shaderUniform.Location, true, ref value);
        }


        public bool GetShaderUniform(string name, out ShaderUniform shaderUniform)
        {
            for (int i = 0; i < shaderUniforms.Length; i++)
            {
                if (shaderUniforms[i].Name == name) { shaderUniform = shaderUniforms[i]; return true; }
            }
            shaderUniform = new ShaderUniform();
            return false;
        }
        public bool GetShaderAtribute(string name, out ShaderAttribute shaderUniform)
        {
            for (int i = 0; i < shaderUniforms.Length; i++)
            {
                if (shaderUniforms[i].Name == name) { shaderUniform = shaderAttributes[i]; return true; }
            }
            shaderUniform = new ShaderAttribute();
            return false;
        }


        public static bool CompileVertexShader(out int vertexShaderHandle, out string errorMessage, string vertexShaderPath = "shader.vert")
        {
            int success;
            string vertexShaderCode = File.ReadAllText(vertexShaderPath);
            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);
            GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                errorMessage = GL.GetShaderInfoLog(vertexShaderHandle);
                return false;
            }
            errorMessage = "";
            return true;
        }
        public static bool CompileFragmentShader(out int fragmentShaderHandle, out string errorMessage, string fragmentShaderPath = "shader.frag")
        {
            int success;
            string fragmentShaderCode = File.ReadAllText(fragmentShaderPath);
            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderCode);
            GL.CompileShader(fragmentShaderHandle);
            GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                errorMessage = GL.GetShaderInfoLog(fragmentShaderHandle);
                return false;
            }
            errorMessage = "";
            return true;
        }

        public static int CreateLinkProgram(int VertexShaderHandle, int FragmentShaderHandle)
        {
            int ShaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(ShaderProgramHandle, VertexShaderHandle);
            GL.AttachShader(ShaderProgramHandle, FragmentShaderHandle);

            GL.LinkProgram(ShaderProgramHandle);

            // Delete shaders
            GL.DetachShader(ShaderProgramHandle, VertexShaderHandle);
            GL.DetachShader(ShaderProgramHandle, FragmentShaderHandle);

            return ShaderProgramHandle;
        }

        public static ShaderUniform[] CreateUniformList(int ShaderProgramHandle)
        {
            GL.GetProgram(ShaderProgramHandle, GetProgramParameterName.ActiveUniforms, out int uniformsCount);
            ShaderUniform[] shaderUniforms = new ShaderUniform[uniformsCount];
            for (int i = 0; i < uniformsCount; i++)
            {
                GL.GetActiveUniform(ShaderProgramHandle, i, 256, out _, out _, out ActiveUniformType activeUniformType, out string uniformName);
                int location = GL.GetUniformLocation(ShaderProgramHandle, uniformName);
                shaderUniforms[i] = new ShaderUniform(uniformName, location, activeUniformType);
            }
            return shaderUniforms;
        }
        public static ShaderAttribute[] CreateAtributeList(int ShaderProgramHandle)
        {
            GL.GetProgram(ShaderProgramHandle, GetProgramParameterName.ActiveAttributes, out int attributesCount);
            ShaderAttribute[] shaderAttributes = new ShaderAttribute[attributesCount];
            for (int i = 0; i < attributesCount; i++)
            {
                GL.GetActiveAttrib(ShaderProgramHandle, i, 256, out _, out _, out ActiveAttribType activeAttributeType, out string uniformName);
                int location = GL.GetAttribLocation(ShaderProgramHandle, uniformName);
                shaderAttributes[i] = new ShaderAttribute(uniformName, location, activeAttributeType);

            }
            return shaderAttributes;
        }


    }
}
