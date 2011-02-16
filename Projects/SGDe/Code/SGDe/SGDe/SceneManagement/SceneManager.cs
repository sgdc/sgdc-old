using System;
using System.Collections.Generic;

namespace SGDE.SceneManagement
{
    public sealed class SceneManager
    {
        private static SceneManager mInstance;

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