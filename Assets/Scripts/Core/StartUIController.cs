using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Util;
using UnityEngine.UI;


namespace Core.UI
{
    public class StartUIController : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField] private Button startBtn;
        [SerializeField] private Button rankBtn;
        [SerializeField] private Button exitBtn;

        private void Awake()
        {
            startBtn.onClick.AddListener(OnClickStartBtn);
            rankBtn.onClick.AddListener(OnClickRankBtn);
            exitBtn.onClick.AddListener(OnClickExitBtn);
        }

        private void OnClickStartBtn()
        {
            SceneChangeManager.ChangeTo("GameMap");
        }

        private void OnClickRankBtn()
        {

        }

        private void OnClickExitBtn()
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        
        }
    
    }
}