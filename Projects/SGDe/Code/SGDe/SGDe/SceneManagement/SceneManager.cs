using System;
using System.Collections.Generic;

namespace SGDE.SceneManagement
{
    public sealed class SceneManager
    {
        private static SceneManager mInstance;

        public List<Entity> mKeyboardListeners;

        private SceneManager()
        {
            mKeyboardListeners = new List<Entity>();
        }

        public void RegisterKeyboardListener(Entity e)
        {
            mKeyboardListeners.Add(e);
        }

        public List<Entity> GetKeyboardListeners()
        {
            return mKeyboardListeners;
        }

        public static SceneManager GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = new SceneManager();
            }
            return mInstance;
        }
    }
}