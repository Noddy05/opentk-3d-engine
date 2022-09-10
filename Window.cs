using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using GameEngine.GLSL_Shaders;
using GameEngine.Misc;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GameEngine.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameEngine
{
    class Window : GameWindow
    {
        private int indexBuffer = -1;
        private int vertexBuffer = -1;
        private int vao = -1;
        private ShaderProgram shaderProgram = new ShaderProgram() { id = 0 };

        private Matrix4 projectionMatrix;
        private Matrix4 cameraMatrix;
        private int uniformProjection = -1;
        private int uniformCamera = -1;
        private Camera? camera;

        public Camera? RenderingCamera()
            => camera;

        public Window(string title = "GameWindow", uint width = 400, uint height = 400, bool centerWindow = false)
            : base(GameWindowSettings.Default,
                   new NativeWindowSettings()
                   {
                       Title = title,
                       Size = new Vector2i((int)width, (int)height),
                       WindowBorder = WindowBorder.Resizable,
                       StartVisible = false,
                       StartFocused = true,
                       API = ContextAPI.OpenGL,
                       Profile = ContextProfile.Core,
                       APIVersion = new Version(3, 3)
                    })
        {
            if (centerWindow)
                CenterWindow(new Vector2i((int)width, (int)height));

        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            camera = new Camera();
            CursorState = CursorState.Grabbed;

            Console.WriteLine("Window succesfully loaded");
            IsVisible = true;


            GL.ClearColor(0.5f, 0f, 0.8f, 1f);

            float[] vertices = new float[]
            {
                -0.5f,   0.5f,  -5.0f,    0f, 1f,
                 0.5f,   0.5f,  -5.0f,    1f, 1f,
                 0.5f,  -0.5f,  -5.0f,    1f, 0f,
                -0.5f,  -0.5f,  -5.0f,    0f, 0f,
            };

            uint[] indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };

            UpdateVertices(vertices, indices);

            shaderProgram = ShaderMain.LoadProgram("../../../../GLSL Shaders/vertex_shader.glsl",
                    "../../../../GLSL Shaders/fragment_shader.glsl");

            uniformCamera = GL.GetUniformLocation(shaderProgram.id, "camera");
            uniformProjection = GL.GetUniformLocation(shaderProgram.id, "projection");

            KeyManager.LoadKeys();
            KeyDown += KeyManager.OnKeyDown;
            KeyUp += KeyManager.OnKeyUp;

            Player player = new Player();
            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vertexBuffer);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(indexBuffer);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vao);

            GL.UseProgram(0);
            GL.DeleteProgram(shaderProgram.id);

            base.OnUnload();
        }

        private void UpdateVertices(float[] vertices, uint[] indices)
        {
            //Send data to graphics card
            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticCopy);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbind buffer

            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticCopy);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            //After binding vertex array, this code will be bound to vao
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0); // Unbind vertex array
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.UseProgram(shaderProgram.id);

            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(camera!.GetFOV(), (float)Size.X / Size.Y, 0.1f, 1000f);
            GL.UniformMatrix4(uniformProjection, false, ref projectionMatrix);

            cameraMatrix = camera!.CameraMatrix();
            GL.UniformMatrix4(uniformCamera, false, ref cameraMatrix);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);

            #region Texture testing
            GL.Enable(EnableCap.Texture2D);

            Bitmap bitmap = new Bitmap("../../../../Temp/color-test.png");
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //PixelInternalFormat.Rgba includes alpha (PixelFormat.Bgra aswell)
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            #endregion

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
