using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Data;
using UnityEngine.UI;
using Core.Unit;
using Core.Util;
using Core.Enemy;
using System;
using System.Linq;


namespace Core.GameUI
{
    public class GameMapUIManager : MonoBehaviour
    {
        private RunTimeDataManager resource;
        [SerializeField]
        private SpawnManager spawn;
        private RankDataManager dataManager;

        void Start()
        {
            resource = RunTimeDataManager.Instance;
            resource.OnClayChanged += UpdateClayText;
            resource.OnGravelChanged += UpdateGravelText;

            dataManager = RankDataManager.Instance;
        }

        #region resource
        [Header("Resource")]
        private bool gravelClicked = false;
        [SerializeField] private GameObject gravelCoolDown;
        [SerializeField] private float gravelCoolTime = 3f; // 애니메이션 클립 수동으로 작성
        [SerializeField] private int clayIncreasing = 1;
        [SerializeField] private int gravelIncreasing = 3;

        // clay 자원 생성
        public void ResourceBtn01()
        {
            resource.UpdateClayResource(clayIncreasing);
        }

        // gravel 자원 생성(3초에 한번)
        public void ResourceBtn02()
        {
            if (!gravelClicked)
            {
                StartCoroutine(GravelCoolTime());
                resource.UpdateGravelResource(gravelIncreasing);
            }
        }

        // 3초 쿨타임
        private IEnumerator GravelCoolTime()
        {
            gravelClicked = true;
            gravelCoolDown.SetActive(true);

            yield return new WaitForSeconds(gravelCoolTime);

            gravelCoolDown.SetActive(false);
            gravelClicked = false;
        }
        #endregion

        #region ResourceText
        [SerializeField] private Text clayText;
        [SerializeField] private Text gravelText;

        /// <summary>
        /// RunTimeDataManager에서 값이 바뀌면 Action으로 Invoke
        /// </summary>
        void UpdateClayText(int clay)
        {
            clayText.text = $"점토: {clay.ToString()}";
        }

        /// <summary>
        /// RunTimeDataManager에서 값이 바뀌면 Action으로 Invoke
        /// </summary>
        void UpdateGravelText(int gravel)
        {
            gravelText.text = $"자갈: {gravel.ToString()}";
        }
        #endregion

        #region Generation
        [Header("Generation")]
        [SerializeField]
        private UnitBaseStatsData unit01;
        [SerializeField]
        private UnitBaseStatsData unit02;
        [Header("Warning")]
        public GameObject warningText;
        private Coroutine warningCoroutine;
        private float warningCoolTime = 2.0f;
        [Header("Success")]
        public GameObject successText;
        private Coroutine successCoroutine;
        private float successCoolTime = 0.8f;

        public void GenerateBtn01()
        {
            TrySpawnUnit(unit01);
        }

        public void GenerationBtn02()
        {
            TrySpawnUnit(unit02);
        }

        /// <summary>
        /// unit의 생성 여부와 최종 생성 요청까지 관리
        /// 성공 시, 성공 패널 활성화
        /// 실패 시, 실패 패널 활성화
        /// </summary>
        /// <param name="unit"></param>
        void TrySpawnUnit(UnitBaseStatsData unit)
        {
            if (resource.clay >= unit.cost.clay && resource.gravel >= unit.cost.gravel)
            {
                //스폰 성공 시, 자원 차감 및 성공 패널 표기
                if (spawn.SpawnUnit(unit.unitPrefab))
                {
                    resource.UpdateClayResource(-unit.cost.clay);
                    resource.UpdateGravelResource(-unit.cost.gravel);

                    if (successCoroutine != null)
                    {
                        StopCoroutine(successCoroutine);
                    }

                    successCoroutine = StartCoroutine(SuccessCoolTime(successText, successCoolTime));
                    warningCoroutine = null;
                }
                else
                {
                    // 반복 생성 가능하도록
                    if (warningCoroutine == null)
                    {
                        warningCoroutine = StartCoroutine(WarningCoolTime(warningText, warningCoolTime));
                    }
                }
            }
            else
            {
                // 반복 생성 가능하도록
                if (warningCoroutine == null)
                {
                    warningCoroutine = StartCoroutine(WarningCoolTime(warningText, warningCoolTime));
                }
            }
        }

        /// <summary>
        /// WarningPanel을 주어진 시간만큼 활성화.
        /// 확장 할거면 해당 이벤트를 관리하는 매니저를 별도로 빼기.
        /// </summary>
        /// <param name="= "panel"></parm>
        /// <param name="time"></param>
        /// <returns></returns>
        IEnumerator WarningCoolTime(GameObject panel, float time)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(time);
            panel.SetActive(false);
            warningCoroutine = null;

        }

        /// <summary>
        /// SuccessPanel을 주어진 시간만큼 활성화.
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        IEnumerator SuccessCoolTime(GameObject panel, float time)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(time);
            panel.SetActive(false);
            successCoroutine = null;
        }
        #endregion

        #region Time
        [Header("Time")]
        public Text time;
        private float timer = 0f;

        void Update()
        {
            if (!isRunning)
            {
                return;
            }

            timer += Time.deltaTime;

            int minutes = (int)(timer / 60);
            int seconds = (int)(timer % 60);
            int milliseconds = (int)((timer * 100) % 100);

            time.text = $"Time: {minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        }

        #endregion

        #region Exit
        [Header("Exit_Score")]
        public GameObject scorePanel;
        public GameObject userName;
        public Text scoreTime;
        public Text kills;
        private int killEnemy = 0;
        private bool isRunning = true;
        //클리어 여부: 적군 기지 파괴(소요된 시간이 플러스 점수로 기입되지 않도록 사용할 변수)
        private bool isClear = false;

        /// <summary>
        /// 시간과 킬 수, 유저 이름을 저장
        /// 유저 이름이 없을 경우 날짜와 시간으로 기본 세팅
        /// 게임 맵 상의 시간이 더이상 흐르지 않도록 관련 변수 설정
        /// </summary>
        public void ExitBtn()
        {
            isRunning = false;

            scoreTime.text = time.text;
            kills.text = $"Kills: {killEnemy}";
            var input = userName.GetComponent<InputField>();
            input.text = $"{DateTime.Now.ToString("MM:dd:HH:mm")}";
            scorePanel.SetActive(true);
        }

        //로비로 씬 전환
        //작성된 점수와 사용자 정보를 dataManager에게 기록하도록 함.
        public void GoToStart()
        {
            //'Time:' 부분 삭제
            var rawTime = scoreTime.text.Split(" ").ToList();

            dataManager.SaveRank(rawTime[1], killEnemy, userName.GetComponent<InputField>().text, isClear);

            SceneChangeManager.ChangeTo("Start");
        }

        private void OnEnable()
        {
            EnemyController.OnEnemyKilled += HandleEnemyKilled;
        }

        private void HandleEnemyKilled()
        {
            killEnemy++;
        }
        
        #endregion
    }
}