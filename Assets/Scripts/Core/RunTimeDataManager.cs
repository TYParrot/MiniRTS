using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Core.Data
{
    public class RunTimeDataManager
    {
        private static RunTimeDataManager _instance;
        public static RunTimeDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RunTimeDataManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 인스턴스 생성 시 Init 메소드 자동 호출
        /// </summary>
        private RunTimeDataManager()
        {
            Init();
        }

        #region Callback

        public Action<int> OnClayChanged;
        public Action<int> OnGravelChanged;

        #endregion
        
        #region var

        /// <summary>
        /// 변수 초기화
        /// </summary>
        void Init()
        {
            clay = 0;
            gravel = 0;
        }

        /// <summary>
        /// 외부 접근 변수와 내부 저장 변수를 분리(Stack Over flow 방지)
        /// </summary>
        private int _clay;
        private int _gravel;

        /// <summary>
        /// 외부로는 clay 수치 반환, 내부로는 업데이트
        /// </summary>
        public int clay
        {
            get { return _clay; }
            private set { _clay = value; }
        }

        /// <summary>
        /// 외부로는 gravel 수치 반환, 내부로는 업데이트
        /// </summary>
        public int gravel
        {
            get { return _gravel; }
            private set { _gravel = value; }
        }

        /// <summary>
        /// UI Event를 통해 Clay 자원을 획득 및 업데이트
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateClayResource(int amount)
        {
            clay += amount;
            OnClayChanged?.Invoke(_clay);
        }

        /// <summary>
        /// UI Event를 통해 Gravel 자원을 획득 및 업데이트
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateGravelResource(int amount)
        {
            gravel += amount;
            OnGravelChanged?.Invoke(_gravel);
        }
        #endregion
    }
}
