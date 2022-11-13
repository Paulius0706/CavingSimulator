using CavingSimulator2.Debugger;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Objects;
using CavingSimulator2.Render;
using CavingSimulator2.Render.Meshes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2
{
    public class Game : GameWindow
    {


        public static ShaderPrograms shaderPrograms = new ShaderPrograms();
        public static Textures textures = new Textures();
        public static BlockMeshes blockMeshes;
        public static BlockTextures blockTextures = new BlockTextures("Render/Images/Blocks.jpg");

        public static Dictionary<int, BaseObject> objects = new Dictionary<int, BaseObject>();

        public static KeyboardState input;
        public static MouseState mouse;
        public static CursorState cursorState = CursorState.Normal;
        public static Vector2 mouseMoveDelta = Vector2.Zero;
        public static float deltaTime = 0;
        public static Matrix4 view = Matrix4.Identity;

        private int fpsCounter = 0;
        private float second = 0;
        private int playerid = -1;

        public Game(int width, int height, string title) : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Size = (width, height),
                Title = title,
                WindowBorder = WindowBorder.Fixed,
                StartVisible = false,
                StartFocused = true,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            }
            )
        {
            CenterWindow();
        }
        protected override void OnLoad()
        {
            this.IsVisible = true;

            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            Game.blockMeshes =  new BlockMeshes();

            Game.input = KeyboardState;
            Game.mouse = MouseState;

            // Create shaders
            Game.shaderPrograms.Add("object", new ShaderProgram("Render/Shaders/shader.vert", "Render/Shaders/shader.frag"));
            Game.shaderPrograms.Add("block", new ShaderProgram("Render/Shaders/blockShader.vert", "Render/Shaders/blockShader.frag"));


            // set Model View Projection 
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.LookAt(Camera.position, Camera.position + Camera.lookToPoint, Camera.up); ;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), (float)viewport[2] / (float)viewport[3], 0.1f, 1000.0f);

            // Add Model View Projection to shaders
            Game.shaderPrograms.UseProgram("object");
            Game.shaderPrograms.Current.SetUniform("Model", ref model);
            Game.shaderPrograms.Current.SetUniform("View", ref view);
            Game.shaderPrograms.Current.SetUniform("Projection", ref projection);
            Game.shaderPrograms.UnUseProgram();

            Game.shaderPrograms.UseProgram("block");
            Game.shaderPrograms.Current.SetUniform("View", ref view);
            Game.shaderPrograms.Current.SetUniform("Projection", ref projection);
            Game.shaderPrograms.Current.SetUniform("MeshSize", 1f / (float)Game.blockTextures.spriteHeight);
            Game.shaderPrograms.UnUseProgram();

            // Add Textures
            Game.textures.Add("container", new Texture("Render/Images/container.jpg"));
            Game.textures.Add("grassBlock", new Texture("Render/Images/grass_block.png"));

            // Set CameraPosition
            Camera.position = new Vector3(0, -1, 0f);

            // Add objects
            playerid = BaseObject.incremeter;
            Game.objects.Add(BaseObject.incremeter, new CameraObject(new CavingSimulator.GameLogic.Components.Transform(new Vector3(0, 0, 15)) ));

            
            Debug.Add("FPS", 0, 1);
            Debug.Add("LoadedChunkCount", 1, 1 );
            Debug.Add("PlayerPosition", 2, 1);

            base.OnLoad();
        }
        protected override void OnUnload()
        {

            //Game.objectShader?.Dispose();
            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Game.input = KeyboardState;
            Game.cursorState = CursorState;
            Game.mouse = MouseState;
            Game.deltaTime = ((float)e.Time);

            ///////////////////////UPDATE//////////////////////////////

            foreach (BaseObject baseObject in objects.Values) baseObject.Update();

            fpsCounter++;
            second += Game.deltaTime;
            if(second > 1f) 
            {
                Debug.WriteLine("FPS", 0, "FPS:" + fpsCounter);
                second = 0f;
                fpsCounter = 0;
            }
            Debug.WriteLine("PlayerPosition", 0, String.Format("Player Position: {0,8:F2} {1,8:F2} {2,8:F2}", 
                (double)(Game.objects[playerid] as CameraObject).transform.GlobalPosition.X,
                (double)(Game.objects[playerid] as CameraObject).transform.GlobalPosition.Y,
                (double)(Game.objects[playerid] as CameraObject).transform.GlobalPosition.Z));
            Debug.WriteLine("LoadedChunkCount", 0, "Loaded Chunks:" + ChunkGenerator.chunks.Count);
            //Debug.WriteLine("FPS", 0, "FPS:" + (1f / Game.deltaTime));

            /////////////////////////////////////////////////////
            Camera.Update();
            CursorState = Game.cursorState;

            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {

            Camera.Update();
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Game.shaderPrograms.UseProgram("object");
            Game.shaderPrograms.Current.SetUniform("View", ref Game.view);

            foreach (BaseObject baseObject in objects.Values) baseObject.Render();

            Game.shaderPrograms.UnUseProgram();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Game.shaderPrograms.UseProgram("block");
            Game.shaderPrograms.Current.SetUniform("View", ref Game.view);
            Game.blockTextures.UploadTexture();

            foreach (BaseObject baseObject in objects.Values) baseObject.Render();

            Game.shaderPrograms.UnUseProgram();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Context.SwapBuffers();

            Debug.Render();

            base.OnRenderFrame(e);

        }
        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        //public static void Destroy(GameObject gameObject) { destroyedGameObjects.Enqueue(gameObject.id); }
        //public static void Instantiate(GameObject gameObject) { createdGameObjects.Enqueue(gameObject); }
    }
}
