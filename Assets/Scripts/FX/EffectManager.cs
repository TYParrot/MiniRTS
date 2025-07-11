using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Effect
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance{ get; private set; }
        public GameObject destroyEffect;
        public GameObject effectParent;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //유닛이 죽으면 해당 위치에 Spawn
        public void PlayDestryEffect(GameObject target)
        {
            var clone = GameObject.Instantiate(destroyEffect, target.transform.position, Quaternion.identity);
            clone.transform.SetParent(effectParent.transform, worldPositionStays: true);
        }

    }   
}

