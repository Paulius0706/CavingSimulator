using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render
{
    public class ShaderPrograms
    {
        private Dictionary<string, ShaderProgram> shaderPrograms = new Dictionary<string, ShaderProgram>();
        private int incrementer = 0;
        public string Use { get; private set; }

        public ShaderProgram this[string key]
        {
            get { return shaderPrograms[key]; }
            set { shaderPrograms[key] = value; }
        }
        public ShaderProgram Current
        {
            get 
            {
                return shaderPrograms[Use];
            }
        }
        public void Add(string name, ShaderProgram shaderProgram) { shaderPrograms.Add(name, shaderProgram);}
        public bool ContainsKey(string key) { return shaderPrograms.ContainsKey(key); }
        public void Remove(string key) { shaderPrograms.Remove(key); }



        public void UseProgram(string name)
        {
            GL.UseProgram(this[name].ShaderProgramHandle);
            Use = name;
        }
        public void UnUseProgram()
        {
            GL.UseProgram(0);
            Use = "";
        }
    }
}
