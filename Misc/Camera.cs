using OpenTK.Mathematics;
using System;

namespace GameEngine.Misc
{
    class Camera
    {
        private Vector3 position = new Vector3();
        private Vector3 rotation = new Vector3();
        private float fov = 60;
        private float cameraSensitivity = 1 / 800f;

        public Camera(Vector3 startingPosition, Vector3 startingRotation, float fov = 60, float cameraSensitivity = 1/800f)
        {
            position = startingPosition;
            rotation = startingRotation;
            this.fov = fov;
            this.cameraSensitivity = cameraSensitivity;
        }

        public Camera()
        {
            position = new Vector3();
            rotation = new Vector3();
            fov = 60;
            cameraSensitivity = 1/800f;
        }

        public void Move(Vector3 movement)
        {
            position += movement;
        }

        public void Rotate(Vector3 rotation)
        {
            this.rotation += rotation * cameraSensitivity;
        }

        public Vector3 Position()
            => position;

        public Vector3 Forward()
            => (Vector4.UnitZ * CameraMatrix().Inverted()).Xyz;
        public Vector3 Right()
            => Vector3.Cross(Forward(), Vector3.UnitY);
        public Vector3 Left()
            => -Right();
        public Vector3 Backward()
            => -Forward();

        public float GetFOV()
            => angleToRad(fov);

        public float angleToRad(float angle)
            => angle * MathF.PI / 180;

        public float radToAngle(float rad)
            => rad / MathF.PI * 180;

        public Matrix4 CameraMatrix()
            => Matrix4.CreateTranslation(position) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation));
    }
}
