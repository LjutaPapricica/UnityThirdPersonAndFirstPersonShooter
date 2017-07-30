using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine;
using System;
using GameManager;

namespace GameManager
{
    public class GameManager : NetworkManager
    {
        [SerializeField] private MainMenuManager PauseMenu;
        [SerializeField] private bool IsPaused;
        [SerializeField] private bool CanPauseGameTime;

        // Use this for initialization
        void Start()
        {
            
        }
        
        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if (IsPaused == true)
            //    {
            //        IsPaused = false;
            //    }
            //    else
            //    {
            //        IsPaused = true;
            //    }
            //}
            //PauseMenuSystem(IsPaused);
        }

        private void PauseMenuSystem(bool IsPaused)
        {
            if(IsPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseMenu.gameObject.SetActive(true);
                if (CanPauseGameTime)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
            }
            else
            {
                PauseMenu.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (CanPauseGameTime)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 1;
                }
            }
        }

        public void ResumeButton()
        {
            IsPaused = false;
        }

        public void SetHost(int port)
        {
            singleton.networkPort = port;
            singleton.StartHost();
            
        }

        public override void OnStartHost()
        {

        }

        public void SetClient(string ipAddress, int port)
        {
            singleton.networkAddress = ipAddress;
            singleton.networkPort = port;
            singleton.StartClient();
        }
    }
}
