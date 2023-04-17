using CavingSimulator2.GameLogic.Objects;
using CavingSimulator2.GameLogic.Objects.SpaceShipParts;
using CavingSimulator2.GameLogic.UI.Views.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace CavingSimulator2.GameLogic.UI.Views
{
    public class MeniuView : View
    {
        float scale = 3f;
        float letterWidth = 15f;
        float letterHeight = 20f;
        float paddling = 10f;
        float gap = 10f;
        float buttonHeight = 60f;
        public MeniuView() : base()
        {
            
            buttonHeight = (paddling * 2f + letterHeight) * scale;
            Button button0 = new Button(
                "Name",
                new Vector2(Game.ViewPortSize.X / 2f, Game.ViewPortSize.Y / 2f + (buttonHeight + gap) * 2f + buttonHeight),
                "Flight Simulator",
                new Vector2(letterWidth * scale * 2, letterHeight * scale * 2),
                paddling * scale,
                null);
            Button button1 = new Button(
                "start",
                new Vector2(Game.ViewPortSize.X /2f, Game.ViewPortSize.Y /2f + buttonHeight + gap),
                "PLAY",
                new Vector2(letterWidth*scale,letterHeight* scale),
                paddling * scale,
                StartButtonEvent);
            Button button2 = new Button(
                "start", 
                new Vector2(Game.ViewPortSize.X / 2f, Game.ViewPortSize.Y / 2f), 
                "PLAY AS DRONE", 
                new Vector2(letterWidth * scale, letterHeight * scale), 
                paddling * scale,
                DroneButtonEvent);
            Button button3 = new Button(
                "start", 
                new Vector2(Game.ViewPortSize.X / 2f, Game.ViewPortSize.Y / 2f - buttonHeight - gap), 
                "PLAY AS PLANE", 
                new Vector2(letterWidth * scale, letterHeight * scale), 
                paddling * scale,
                PlaneButtonEvent);
            views.Add("Flight Simulator", button0);
            views.Add("start", button1);
            views.Add("drone", button2);
            views.Add("plane", button3);
        }
        public void StartButtonEvent()
        {
            if(Game.objects.TryGetValue(0,out BaseObject baseObject))
            {
                Game.UI.UseView("game");
                PlayerCabin playerCabin = baseObject as PlayerCabin;
                playerCabin.AddSelector();
                //playerCabin.LoadEmpty();
                StartToPause();
            }
        }
        public void PlaneButtonEvent()
        {
            if (Game.objects.TryGetValue(0, out BaseObject baseObject))
            {
                Game.UI.UseView("game");
                PlayerCabin playerCabin = baseObject as PlayerCabin;
                playerCabin.LoadPlane();
                StartToPause();
            }
        }
        public void DroneButtonEvent()
        {
            if (Game.objects.TryGetValue(0, out BaseObject baseObject))
            {
                Game.UI.UseView("game");
                PlayerCabin playerCabin = baseObject as PlayerCabin;
                playerCabin.LoadDrone();
                StartToPause();
            }
        }
        public void StartToPause()
        {
            Button button1 = new Button(
                "start",
                new Vector2(Game.ViewPortSize.X / 2f, Game.ViewPortSize.Y / 2f + buttonHeight + gap),
                "RESUME",
                new Vector2(letterWidth * scale, letterHeight * scale),
                paddling * scale,
                StartButtonEvent);
            views["start"].Dispose();
            views["start"] = button1;
        }
    }
    
}
