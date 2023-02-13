using CavingSimulator2.GameLogic.UI.Views;
using CavingSimulator2.Render;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI
{
    public class Interface
    {
        private Dictionary<string, View> views = new Dictionary<string, View>();
        public string Use { get; private set; } = "";

        public View this[string key]
        {
            get { 
                if(views.TryGetValue(key,out View view)) return view;
                return null;
            }
            set { views[key] = value; }
        }
        public View Current
        {
            get
            {
                if (views.TryGetValue(Use, out View view)) return view;
                return null;
            }
        }
        public void Add(string name, View view) { views.Add(name, view); }
        public bool ContainsKey(string key) { return views.ContainsKey(key); }
        public void Remove(string key) { views[key].Dispose(); views.Remove(key); }



        public void UseView(string name)
        {
            Use = name;
        }
        public void UnUseView()
        {
            Use = "";
        }

        public void Render()
        {
            if (Current == null) return;
            Current.Render();
            
        }

        public T GetView<T>(string tag) where T : View
        {
            return Current.GetView<T>(tag);
        }
    }
}
