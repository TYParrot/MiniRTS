using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Core.Data
{
    public class RunTimeDataManager
    {
        public static RunTimeDataManager Instance;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Init();
        }

        /// <summary>
        /// 변수 초기화
        /// </summary>
        void Init()
        {
            clay = 0;
            gravel = 0;
        }

        /// <summary>
        /// 외부로는 clay 수치 반환, 내부로는 업데이트
        /// </summary>
        public int clay
        {
            get { return clay; }
            private set { clay = value; }
        }

        /// <summary>
        /// 외부로는 gravel 수치 반환, 내부로는 업데이트
        /// </summary>
        public int gravel
        {
            get { return gravel; }
            private set { gravel = value; }
        }

        /// <summary>
        /// UI Event를 통해 Clay 자원을 획득 및 업데이트
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateClayResource(int amount)
        {
            clay += amount;
        }

        /// <summary>
        /// UI Event를 통해 Gravel 자원을 획득 및 업데이트
        /// </summary>
        /// <param name="amount"></param>
        public void UpdateGravelResource(int amount)
        {
            gravel += amount;
        }
    }
}
