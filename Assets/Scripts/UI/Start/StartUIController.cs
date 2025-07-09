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
        private SoundManager sound;

        private void Awake()
        {
            startBtn.onClick.AddListener(OnClickStartBtn);
            rankBtn.onClick.AddListener(OnClickRankBtn);
            exitBtn.onClick.AddListener(OnClickExitBtn);
        }


        void Start()
        {
            rankManager = RankDataManager.Instance;
            sound = SoundManager.Instance;
        }

        private void OnClickStartBtn()
        {
            SceneChangeManager.ChangeTo("GameMap");
            sound.PlayUIBtnClick();
        }

        private void OnClickRankBtn()
        {
            //음원 재생 먼저
            sound.PlayUIBtnClick();

            var rankList = rankManager.GetRankList();

            var scoredLists = new List<(string name, int score)>();

            foreach (var rankData in rankList)
            {
                bool isClear = rankData.isClear;
                int totalSec = 0;
                
                // 클리어 여부를 통해 시간에 대한 보너스 점수 부여
                if (isClear)
                {
                    // 시간 파싱
                    var timeParts = rankData.time.Split(":");
                    int hour = int.Parse(timeParts[0]);
                    int min  = int.Parse(timeParts[1]);
                    int sec  = int.Parse(timeParts[2]);
                    totalSec = hour * 3600 + min * 60 + sec;

                }

                // 킬 수 반영
                int finalScore = rankData.kills * 100 + ((1000 - totalSec) == 1000 ? 0 : (1000 - totalSec));

                scoredLists.Add((rankData.playerName, finalScore));
            }

            //내림차순 정렬
            var sorted = scoredLists.OrderByDescending(x => x.score).ToList();

            rank.text = string.Join("\n", sorted.Select((x, idx) => $"{idx + 1}. {x.name} : {x.score}"));

            rankPanel.SetActive(true);
        }

        private void OnClickExitBtn()
        {
            sound.PlayUIBtnClick();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        
        }
    
    }
}