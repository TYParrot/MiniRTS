using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyEffect : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
