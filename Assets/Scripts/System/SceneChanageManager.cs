using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core.Util
{
    public static class SceneChangeManager
    {
        public static void ChangeTo(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }    
}
