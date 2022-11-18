using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class Meshes
    {
        private Dictionary<string, MeshBuffer> meshes = new Dictionary<string, MeshBuffer>();

        public MeshBuffer this[string name]
        {
            get { return meshes[name]; }
        }
        public void Add(string name, string path, string texture)
        {
            meshes.Add(name, new MeshBuffer(path, Game.textures.GetIndex(texture)));
        }
        public void Remove(string name)
        {
            if (meshes.ContainsKey(name))
            {
                meshes[name].Dispose();
                meshes.Remove(name);
            }
        }

    }
}
