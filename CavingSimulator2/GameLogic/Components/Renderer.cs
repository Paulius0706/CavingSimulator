using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.Render.Meshes;
using CavingSimulator2.Render.Meshes.SpaceShipParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator.GameLogic.Components
{
    public class Renderer : IDisposable
    {
        private List<Mesh> meshes = new List<Mesh>();
        public void Render()
        {
            foreach (Mesh mesh in meshes) mesh.Render();
        }

        public Mesh GetMesh(int i) { return meshes[i]; }
        public void AddMesh(Mesh mesh){ meshes.Add(mesh);}
        public void RemoveAtMesh(int i) { meshes[i].Dispose(); meshes.RemoveAt(i); }
        public void UpdateMesh(int i, Mesh mesh) { meshes[i] = mesh; }
        public void ClearMesh(){meshes.Clear();}

        public void Dispose()
        {
            foreach(Mesh mesh in meshes) mesh.Dispose();
        }
    }
}
