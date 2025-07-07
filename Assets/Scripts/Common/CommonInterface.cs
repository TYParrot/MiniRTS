using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using UnityEngine;

namespace Core.Common
{
    /// <summary>
    /// 아군, 적군이 공통적으로 갖고 있는 인터페이스
    /// </summary>
    public interface CommonInterface
    {
        void TakeDamage(int amount);

    }
}

