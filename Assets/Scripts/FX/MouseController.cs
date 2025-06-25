using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    #region event
    [Header("EventPrefab")]
    [SerializeField] private GameObject clickE;
    #endregion

    
    #region drag
    [SerializeField] private GameObject box;
    private GameObject dragBox;
    private Vector3 startPos;
    private Vector3 nowPos;
    private Vector3 deltaPos;
    private float deltaX;
    private float deltaY;
    #endregion

    void Update()
    {
        Drag();
        MouseClick();
    }

    void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1));
            dragBox = Instantiate(box, new Vector3(0, 0, 0), Quaternion.identity);
        }
        if (Input.GetMouseButton(0))
        {
            nowPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1));
            deltaPos = startPos + (nowPos - startPos) / 2;
            deltaX = Mathf.Abs(startPos.x - nowPos.x);
            deltaY = Mathf.Abs(startPos.y - nowPos.y);
            dragBox.transform.position = deltaPos;
            dragBox.transform.localScale = new Vector3(deltaX, deltaY, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(dragBox);
        }
    }

    /// <summary>
    /// GameMap에서 마우스를 클릭하면 애니메이션 프리팹이 생성된다.
    /// 단, UI 위에서는 스폰되지 않는다.
    /// </summary>
    void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 마우스 위치를 world로 변환하여 가져와야 이상한 곳에 스폰 되지 않는다.
                Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawnPos.z = 0f;
                Instantiate(clickE, spawnPos, Quaternion.identity);
            }

        }
    }
}
