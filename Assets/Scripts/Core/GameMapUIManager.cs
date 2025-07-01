using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Data;
using UnityEngine.UI;
using Core.Unit;


namespace Core.GameUI
{
    public class GameMapUIManager : MonoBehaviour
    {
        private RunTimeDataManager resource;
        [SerializeField]
        private SpawnManager spawn;

        void Start()
        {
            resource = RunTimeDataManager.Instance;
            resource.OnClayChanged += UpdateClayText;
            resource.OnGravelChanged += UpdateGravelText;
        }

        #region resource
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
    }
}