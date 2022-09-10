using GameEngine.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;

namespace GameEngine.Misc
{
    class Player : MonoBehaviour
    {
        private Camera? camera;

        private float moveSpeed = 5f;

        public Player()
        {
            Setup();
        }

        int wKeyIndex = KeyManager.KeyDown("W")!.index, 
            aKeyIndex = KeyManager.KeyDown("A")!.index, 
            sKeyIndex = KeyManager.KeyDown("S")!.index, 
            dKeyIndex = KeyManager.KeyDown("D")!.index,
            qKeyIndex = KeyManager.KeyDown("Q")!.index,
            eKeyIndex = KeyManager.KeyDown("E")!.index,
            escapeKeyIndex = KeyManager.KeyDown("Escape")!.id;
        void Setup()
        {
            camera = gameWindow!.RenderingCamera();
            gameWindow!.RenderFrame += Update;
            gameWindow!.KeyDown += ToggleMouse;
            gameWindow!.MouseMove += OnMouseMove;
        }

        void ToggleMouse(KeyboardKeyEventArgs e)
        {
            if ((int)e.Key == escapeKeyIndex)
            {
                mouseToggled = !mouseToggled;
                gameWindow!.CursorState = (mouseToggled) ? CursorState.Normal : CursorState.Grabbed;
            }
        }

        bool mouseToggled = false;
        void Update(FrameEventArgs e)
        {
            Vector3 movement = new Vector3();


            bool moveThisFrame = false;

            if (KeyManager.KeyDownByIndex(wKeyIndex)!.value)
            {
                movement.Z += 1;
                moveThisFrame = true;
            }
            if (KeyManager.KeyDownByIndex(sKeyIndex)!.value)
            {
                movement.Z -= 1;
                moveThisFrame = true;
            }
            if (KeyManager.KeyDownByIndex(dKeyIndex)!.value)
            {
                movement.X += 1;
                moveThisFrame = true;
            }
            if (KeyManager.KeyDownByIndex(aKeyIndex)!.value)
            {
                movement.X -= 1;
                moveThisFrame = true;
            }
            if (KeyManager.KeyDownByIndex(qKeyIndex)!.value)
            {
                movement.Y += 1;
                moveThisFrame = true;
            }
            if (KeyManager.KeyDownByIndex(eKeyIndex)!.value)
            {
                movement.Y -= 1;
                moveThisFrame = true;
            }


            if (moveThisFrame)
            {
                camera!.Move((camera!.Forward() * movement.Z + camera!.Right() * movement.X
                    + Vector3.UnitY * movement.Y) * moveSpeed * (float)e.Time);
            }

        }

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            if(!mouseToggled)
                camera!.Rotate(new Vector3(e.DeltaY, e.DeltaX, 0f));
        }
    }
}
