using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Helpers
{
    internal class Inputs
    {
        public static Vector2 MovementPlane { get { return Vector2.NormalizeFast(new Vector2((float)HorizontalAxis, (float)VerticalAxis)); } }
        public static float VerticalAxis
        {
            get
            {
                return
                    (Game.input.IsKeyDown(Keys.W) || Game.input.IsKeyDown(Keys.Up) ? 1f : 0f) +
                    (Game.input.IsKeyDown(Keys.S) || Game.input.IsKeyDown(Keys.Down) ? -1f : 0f);
            }
        }
        public static float HorizontalAxis
        {
            get
            {
                return
                    (Game.input.IsKeyDown(Keys.A) || Game.input.IsKeyDown(Keys.Left) ? -1f : 0f) +
                    (Game.input.IsKeyDown(Keys.D) || Game.input.IsKeyDown(Keys.Right) ? 1f : 0f);
            }
        }

        public static float JumpCrouchAxis { get { return (Jump ? 1 : 0) + (Crouch ? -1 : 0); } }
        public static bool Jump { get { return Game.input.IsKeyDown(Keys.Space); } }
        public static bool Crouch { get { return Game.input.IsKeyDown(Keys.LeftShift); } }
        public static float RMouseClick { get { return Game.mouse.IsButtonDown(MouseButton.Right) ? 1f : 0f; } }
    }
}
