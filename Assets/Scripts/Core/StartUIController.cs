using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Util;
using UnityEngine.UI;
using Core.Data;
using System.Linq;


namespace Core.StartUI
{
    public class StartUIController : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField] private Button startBtn;
        [SerializeField] private Button rankBtn;
        [SerializeField] private Button exitBtn;

        [SerializeField] private GameObject rankPanel;
        [SerializeField] private Text rank;

        private RankDataManager rankManager;
        private int idx = 0;

        private void Awake()
        {
            startBtn.onClick.AddListener(OnClickStartBtn);
            rankBtn.onClick.AddListener(OnClickRankBtn);
            exitBtn.onClick.AddListener(OnClickExitBtn);
        }


        void Start()
        {
            rankManager = RankDataManager.Instance;
        }

        private void OnClickStartBtn()
        {
            SceneChangeManager.ChangeTo("GameMap");
        }

        private void OnClickRankBtn()
        {
            var rankList = rankManager.GetRankList();

            var scoredLists = new List<(string name, int score)>();

            foreach (var rankData in rankList)
            {
                // 시간 파싱
                var timeParts = rankData.time.Split(":");
                int hour = int.Parse(timeParts[0]);
                int min  = int.Parse(timeParts[1]);
                int sec  = int.Parse(timeParts[2]);
                int totalSec = hour * 3600 + min * 60 + sec;

                // 킬 수 반영
                int finalScore = rankData.kills * 100 + ((1000 - totalSec) < 0 ? 0 : (1000 - totalSec));

                scoredLists.Add((rankData.playerName, finalScore));
            }

            //내림차순 정렬
            var sorted = scoredLists.OrderByDescending(x => x.score).ToList();

            rank.text = string.Join("\n", sorted.Select((x, idx) => $"{idx + 1}. {x.name} : {x.score}"));

            rankPanel.SetActive(true);
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