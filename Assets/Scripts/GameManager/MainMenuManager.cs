using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class MainMenuManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnApplicationQuit()
        {
            Debug.Log("Quit App called");
        }

        public void SelectScene(string val)
        {
            SceneManager.LoadScene(val);
        }

        public void SelectScene(string val, byte sceneMode)
        {
            SceneManager.LoadScene(val, (LoadSceneMode)sceneMode);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}

