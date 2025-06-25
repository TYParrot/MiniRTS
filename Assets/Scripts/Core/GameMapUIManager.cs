using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Data;
using UnityEngine.UI;

namespace Core.GameUI
{
    public class GameMapUIManager : MonoBehaviour
    {
        private RunTimeDataManager resource;

        void Start()
        {
            resource = RunTimeDataManager.Instance;
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

        // 수치에 변화가 생겼을 때만 업데이트하도록 하면 좋을 듯.
        #endregion

        #region Generation

        #endregion
    }
}