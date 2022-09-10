using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Input
{
    class Key
    {
        public string name;
        public bool value;
        public int id;
        public int index;

        public Key(string name, bool value, int id, int index)
        {
            this.name = name;
            this.value = value;
            this.id = id;
            this.index = index;
        }
    }

    class KeyManager
    {

        private static Key[]? keysDown;

        public static Key? KeyDownByIndex(int keyIndex)
            => keysDown![keyIndex];

        public static Key? KeyDown(int keycode)
        {
            for(int i = 0; i < keysDown!.Length; i++)
                if(keysDown[i].id == keycode) 
                    return keysDown[i];

            return null;
        }
        public static Key? KeyDown(string key)
        {
            for (int i = 0; i < keysDown!.Length; i++)
                if (keysDown[i].name == key) 
                    return keysDown[i];

            return null;
        }

        public static void LoadKeys()
        {
            Keys[] keys = (Keys[])Enum.GetValues(typeof(Keys));
            keysDown = new Key[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                keysDown[i] = new Key(keys[i].ToString(), false, (int)keys[i], i);
            }
        }

        private static Keys? previousKey;
        public static void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (previousKey! == e.Key)
                return;

            previousKey = e.Key;
            for(int i = 0; i < keysDown!.Length; i++)
            {
                if((int)e.Key == keysDown[i].id)
                {
                    keysDown[i].value = true;
                    return;
                }
            }
        }

        public static void OnKeyUp(KeyboardKeyEventArgs e)
        {
            previousKey = null;
            for (int i = 0; i < keysDown!.Length; i++)
            {
                if ((int)e.Key == keysDown[i].id)
                {
                    keysDown[i].value = false;
                    return;
                }
            }
        }
    }
}
