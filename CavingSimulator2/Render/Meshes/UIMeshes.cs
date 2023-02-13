using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CavingSimulator2.Render.Meshes
{
    public class UIMeshes
    {
        private Dictionary<string, UIMesh> meshes = new Dictionary<string, UIMesh>();

        public UIMesh this[string name]
        {
            get { return meshes[name]; }
        }
        public void Add(string name, string texture, Vector2 lower, Vector2 upper)
        {
            meshes.Add(name, new UIMesh(Game.textures.GetIndex(texture),upper, lower,1f));
        }
        public void Remove(string name)
        {
            if (meshes.ContainsKey(name))
            {
                meshes[name].Dispose();
                meshes.Remove(name);
            }
        }
        public void RemoveAll()
        {
            var keys = meshes.Keys;
            foreach (var key in keys)
            {
                meshes[key].Dispose();
                meshes.Remove(key);
            }
        }
        public void Render()
        {
            foreach (UIMesh uIMesh in meshes.Values)
            {
                uIMesh.Render();
            }
        }
    }
}
