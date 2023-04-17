using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views
{
    public abstract class View : IDisposable
    {
        public string tag = "";

        public Dictionary<string, View> views = new Dictionary<string, View>();
        public int layer { get; private set; }
        ~View()
        {
            Dispose();
        }
        public void Update()
        {
            UpdateInternal();
            foreach(var view in views.Values)
            {
                view.Update();
            }
        }
        public virtual void UpdateInternal()
        {

        }
        public virtual void Render()
        {

            #region Find Minimum layer
            
            int layer = int.MaxValue;
            foreach (var view in views.Values)
            {
                if (view.layer < layer) layer = view.layer;
            }
            
            #endregion

            bool findNextLayer = true;
            while (findNextLayer)
            {
                foreach (var view in views.Values)
                {
                    if (view.layer == layer)
                    {
                        view.Render();
                    }
                }
                FindNextLayer(ref findNextLayer, ref layer);
            }
        }
        public void FindNextLayer(ref bool doIt, ref int layer)
        {
            int nextLayer = int.MinValue;
            foreach (var view in views.Values)
            {
                if (view.layer < layer && view.layer > nextLayer) { nextLayer = view.layer; }
            }
            if (nextLayer == int.MinValue) doIt = false;
            layer = nextLayer;
        }
        public static void GetCordsFromLowerPositionWidthHeight(Vector2 lowerPosition, Vector2 widthHeight, out Vector2 trueLowerPosition, out Vector2 trueUpperPosition) 
        {
            Vector2 wh = new Vector2(widthHeight.X / Game.ViewPortSize.X * 2f, widthHeight.Y / Game.ViewPortSize.Y * 2f);
            Vector2 lp = new Vector2(lowerPosition.X / Game.ViewPortSize.X * 2f - 1f, lowerPosition.Y / Game.ViewPortSize.Y * 2f - 1f);
            trueUpperPosition = lp + wh;
            trueLowerPosition = lp;
        }
        public static void GetCordsFromCenterPositionWidthHeight(Vector2 centerPostion, Vector2 widthHeight, out Vector2 trueLowerPosition, out Vector2 trueUpperPosition)
        {
            Vector2 wh = new Vector2(widthHeight.X / Game.ViewPortSize.X * 2f, widthHeight.Y / Game.ViewPortSize.Y * 2f);
            Vector2 cp = new Vector2(centerPostion.X / Game.ViewPortSize.X * 2f - 1f, centerPostion.Y / Game.ViewPortSize.Y * 2f - 1f);
            trueUpperPosition = cp + wh / 2f;
            trueLowerPosition = cp - wh / 2f;
        }

        public void Dispose()
        {
            InternalDispose();
            foreach (var view in views)
            {
                view.Value.Dispose();
                views.Remove(view.Key);
            }
        }
        protected virtual void InternalDispose()
        {

        }

        public T GetView<T>(string tag) where T : View
        {
            if (this.tag == tag && this.GetType().Name == typeof(T).Name) return (T)this;
            foreach(View view in views.Values)
            {
                T view1 = view.GetView<T>(tag);
                if (view1 is not null) return view1;
            }
            return null;
        }
    }
}
