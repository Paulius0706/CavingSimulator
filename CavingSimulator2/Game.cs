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
using BepuPhysics;
using BepuUtilities.Memory;
using BepuPhysics.CollisionDetection;
using CavingSimulator2.Physics;
using CavingSimulator2.Helpers;
using BepuUtilities;
using CavingSimulator2.GameLogic.Components.Physics;
using BepuPhysics.Collidables;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Objects.SpaceShipParts;

namespace CavingSimulator2
{
    public class Game : GameWindow
    {


        public static ShaderPrograms shaderPrograms = new ShaderPrograms();
        public static Textures textures = new Textures();
        public static BlockMeshes blockMeshes;
        public static BlockTextures blockTextures = new BlockTextures("Render/Images/Blocks.jpg");

        public static Dictionary<int, BaseObject> objects = new Dictionary<int, BaseObject>();
        public static Simulation physicsSpace;
        public static BufferPool bufferPool = new BufferPool();
        public static TimeStepper timeStepper = new TimeStepper();
        public static Shapes shapes; 

        public static KeyboardState input;
        public static MouseState mouse;
        public static CursorState cursorState = CursorState.Normal;
        public static Vector2 mouseMoveDelta = Vector2.Zero;
        public static float deltaTime = 0;
        public static Matrix4 view = Matrix4.Identity;

        private int fpsCounter = 0;
        private float second = 0;
        private int playerid = -1;
        private ThreadDispatcher threadDispatcher = new ThreadDispatcher(Math.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1));

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
            // Set default color
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            Game.shapes = new Shapes(Game.bufferPool, 200);
            // Create Physics
            //Game.physicsSpace = Simulation.Create(
            //    Game.bufferPool, 
            //    new NarrowPhaseCallbacks(),
            //    new PoseIntegratorCallbacks(Adapter.Convert(new Vector3(0f, 0f, -10f))), timeStepper, 8);
            ////new PoseIntegratorCallbacks(Adapter.Convert(new Vector3(0f, 0f, -10f))), new SolveDescription(2, 1));

            Game.physicsSpace = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, 0, -1f)), new PositionLastTimestepper());
            Game.physicsSpace.Timestep(0.1f);
            // Create block meshes for instance rendering
            Game.blockMeshes =  new BlockMeshes();

            // Set inputs
            Game.input = KeyboardState;
            Game.mouse = MouseState;

            // Create shaders
            Game.shaderPrograms.Add("object", new ShaderProgram("Render/Shaders/shader.vert", "Render/Shaders/shader.frag"));
            Game.shaderPrograms.Add("block", new ShaderProgram("Render/Shaders/blockShader.vert", "Render/Shaders/blockShader.frag"));


            // Set Model View Projection 
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.LookAt(Camera.position, Camera.position + Camera.lookToPoint, Camera.up); ;
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(OpenTK.Mathematics.MathHelper.DegreesToRadians(90.0f), (float)viewport[2] / (float)viewport[3], 0.1f, 1000.0f);

            // Add Model View Projection to object shader
            Game.shaderPrograms.UseProgram("object");
            Game.shaderPrograms.Current.SetUniform("Model", ref model);
            Game.shaderPrograms.Current.SetUniform("View", ref view);
            Game.shaderPrograms.Current.SetUniform("Projection", ref projection);
            Game.shaderPrograms.UnUseProgram();

            // Add MeshSize View Projection to object shader
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
            //Game.objects.Add(BaseObject.incremeter, new SpaceShipObject(new CavingSimulator.GameLogic.Components.Transform(new Vector3(0.5f, 0.5f, 60f+300f)) ));
            Game.objects.Add(BaseObject.incremeter, new PlayerCabin(new Transform(new Vector3(0.5f, 0.5f, 60f))));

            // Add interactive console
            Debug.Add("FPS", 0, 1);
            Debug.Add("LoadedChunkCount", 1, 1 );
            Debug.Add("StaticsCount", 2, 1);
            Debug.Add("BlocksClollidersCount", 3, 1);
            Debug.Add("PlayerPosition", 4, 1);

            base.OnLoad();
        }
        protected override void OnUnload()
        {
            physicsSpace.Dispose();
            threadDispatcher.Dispose();
            bufferPool.Clear();
            foreach (BaseObject baseObject in objects.Values) baseObject.Dispose();
            shaderPrograms.UnUseProgram();
            shaderPrograms.Remove("object");
            shaderPrograms.Remove("block");
            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Game.input = KeyboardState;
            Game.cursorState = CursorState;
            Game.mouse = MouseState;
            Game.deltaTime = ((float)e.Time);
            Game.physicsSpace.Timestepper.Timestep(Game.physicsSpace, Game.deltaTime, threadDispatcher);
            Game.physicsSpace.Timestep(Game.deltaTime, threadDispatcher);
            //Game.physicsSpace.Timestep(Game.deltaTime, this.threadDispatcher);

            ///////////////////////UPDATE//////////////////////////////

            foreach (BaseObject baseObject in objects.Values) baseObject.Update();

            this.fpsCounter++;
            this.second += Game.deltaTime;
            if(second > 1f) 
            {
                Debug.WriteLine("FPS", 0, "FPS:" + fpsCounter);
                this.second = 0f;
                this.fpsCounter = 0;
            }
            Debug.WriteLine("PlayerPosition", 0, String.Format("Player Position: {0,8:F2} {1,8:F2} {2,8:F2}", 
                (double)(Game.objects[playerid] as PlayerCabin).transform.Position.X,
                (double)(Game.objects[playerid] as PlayerCabin).transform.Position.Y,
                (double)(Game.objects[playerid] as PlayerCabin).transform.Position.Z));
            Debug.WriteLine("StaticsCount", 0, "StaticsCount: " + BlocksDir.colliderBlocks.Count);
            Debug.WriteLine("LoadedChunkCount", 0, "Loaded Chunks:" + ChunkGenerator.chunks.Count);
            Debug.WriteLine("BlocksClollidersCount", 0, "BlocksClollidersCount:" + BlocksDir.colliderBlocks.Count);

            /////////////////////////////////////////////////////
            
            Camera.Update();
            BlocksDir.Update();
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
