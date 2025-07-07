using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class RankData
    {
        public string playerName;
        public int kills;
        public string time;
    }

    [Serializable]
    public class RankListWrapper
    {
        public List<RankData> list;
    }

    public class RankDataManager
    {
        private static List<RankData> rankList = new List<RankData>();
        private static string filePath = System.IO.Path.Combine(Application.dataPath, "Data/RankData.json");


        private static RankDataManager _instance;
        public static RankDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RankDataManager();
                }
                return _instance;
            }
        }

        private RankDataManager()
        {
            LoadRankData();
        }
        
        /// <summary>
        /// GameMap에서 게임이 끝나면 SCORE를 기록
        /// </summary>
        /// <param name="time"></param>
        /// <param name="kills"></param>
        /// <param name="name"></param>
        public void SaveRank(string time, int kills, string name)
        {
            rankList.Add(new RankData
            {
                playerName = name,
                kills = kills,
                time = time
            });

            var wrapper = new RankListWrapper { list = rankList };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(filePath, json);
        }

        //기록된 Rank들을 불러옴
        private void LoadRankData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var wrapper = JsonUtility.FromJson<RankListWrapper>(json);
                rankList = wrapper.list.OrderByDescending(x => x.kills).ToList();
            }
        }

        //start 씬에서 rank들을 호출할 수 있는 메소드
        public List<RankData> GetRankList()
        {
            return rankList;
        }
    }
}
