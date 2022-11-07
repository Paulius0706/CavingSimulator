using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render
{
    public class Textures
    {
        private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        private Dictionary<int, string> indexes = new Dictionary<int, string>();
        private int incrementer = 0;

        public Texture this[string key]
        {
            get { return textures[key]; }
            set { textures[key] = value; }
        }
        public Texture this[int key]
        {
            get { return textures[indexes[key]]; }
            set { textures[indexes[key]] = value; }
        }
        public void Add(string key1, Texture texture) { textures.Add(key1, texture); indexes.Add(incrementer++, key1); }
        public bool ContainsKey(string key) { return textures.ContainsKey(key); }
        public bool ContainsKey(int key) { return indexes.ContainsKey(key); }
        public void Remove(string key)
        {
            textures.Remove(key);
            indexes.Remove(indexes.Keys.Where(a => indexes[a] == key).First());
        }
        public void Remove(int key)
        {
            textures.Remove(indexes[key]);
            indexes.Remove(key);
        }
        public int GetIndex(string key) { return indexes.Keys.Where(a => indexes[a] == key).First(); }
        public string GetName(int key) { return indexes[key]; }


    }
}
