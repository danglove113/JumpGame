using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animmal.Kubikos
{
    public class LoadSceneByName : MonoBehaviour
    {
        public string SceneName;

        public void LoadLevel()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
    }
}