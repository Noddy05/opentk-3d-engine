using OpenTK.Mathematics;
using System;

namespace GameEngine.Misc
{
    struct Vertex
    {
        private float[] position;
        private float[] textureCoordinates;
        private float[] data;

        public void SetPosition(Vector3 position)
        {
            this.position[0] = position.X;
            this.position[1] = position.Y;
            this.position[2] = position.Z;
            data[0] = position.X;
            data[1] = position.Y;
            data[2] = position.Z;
        }

        public void SetTextureCoordinates(Vector2 textureCoordinates)
        {
            this.textureCoordinates[0] = textureCoordinates.X;
            this.textureCoordinates[1] = textureCoordinates.Y;
            data[3] = textureCoordinates.X;
            data[4] = textureCoordinates.Y;
        }

        public Vertex(float[] position, float[] textureCoordinates)
        {
            this.position = position;
            this.textureCoordinates = textureCoordinates;
            data = new float[] { position[0], position[1], position[2], position[3], position[4] };
        }

        public Vertex(Vector3 position, Vector2 textureCoordinates)
        {
            this.position = new float[] { position.X, position.Y, position.Z };
            this.textureCoordinates = new float[] { textureCoordinates.X, textureCoordinates.Y };
            data = new float[] { position.X, position.Y, position.Z, position.X, position.Y };
        }

        public Vertex(float[] data)
        {
            if (data.Length != 5)
                throw new Exception($"Data length (length: {data.Length}) has to be a 5" +
                    $" (3 position components and 2 texture coordinate components)");

            position = new float[] { data[0], data[1], data[2] };
            textureCoordinates = new float[] { data[3], data[4] };
            this.data = new float[] { data[0], data[1], data[2], data[3], data[4] };
        }

        public float[] GetPosition()
            => position;

        public float[] GetTextureCoordinates()
            => textureCoordinates;

        public float[] GetData()
            => data;
    }
}
