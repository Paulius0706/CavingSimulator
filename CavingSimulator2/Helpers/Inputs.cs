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
        public static bool RMouseHold { get { return Game.mouse.IsButtonDown(MouseButton.Right) ? true : false; } }
        public static bool LMouseHold { get { return Game.mouse.IsButtonDown(MouseButton.Left) ? true : false; } }

        public static bool RMouseClick { get { return Game.mouse.IsButtonPressed(MouseButton.Right) ? true : false; } }
        public static bool LMouseClick { get { return Game.mouse.IsButtonPressed(MouseButton.Left) ? true : false; } }
        public static bool BuildMode { get { return Game.input.IsKeyPressed(Keys.B); } }
        public static bool InputBindMode { get { return Game.input.IsKeyPressed(Keys.L); } }
        public static bool Pause { get { return Game.input.IsKeyPressed(Keys.P); } }

        public static bool Left { get { return Game.input.IsKeyPressed(Keys.A) || Game.input.IsKeyPressed(Keys.Left); } }
        public static bool Right { get { return Game.input.IsKeyPressed(Keys.D) || Game.input.IsKeyPressed(Keys.Right);} }
        public static bool Forward { get { return Game.input.IsKeyPressed(Keys.W) || Game.input.IsKeyPressed(Keys.Up); } }
        public static bool Back { get { return Game.input.IsKeyPressed(Keys.S) || Game.input.IsKeyPressed(Keys.Down); } }
        public static bool Up { get { return Game.input.IsKeyPressed(Keys.Space); } }
        public static bool Down { get { return Game.input.IsKeyPressed(Keys.LeftShift); } }

        public static bool RorateRight { get { return Game.input.IsKeyPressed(Keys.E); } }
        public static bool RorateLeft { get { return Game.input.IsKeyPressed(Keys.Q); } }

        public static bool ShiftScroolUp { get { return Game.mouse.ScrollDelta.Y == -1 && Game.input.IsKeyDown(Keys.LeftShift); } }
        public static bool ShiftScroolDown { get { return Game.mouse.ScrollDelta.Y == 1 && Game.input.IsKeyDown(Keys.LeftShift); } }

        public static bool ScroolUp { get { return Game.mouse.ScrollDelta.Y == -1 && !Game.input.IsKeyDown(Keys.LeftShift); } }
        public static bool ScroolDonw { get { return Game.mouse.ScrollDelta.Y == 1 && !Game.input.IsKeyDown(Keys.LeftShift); } }

    }
}
