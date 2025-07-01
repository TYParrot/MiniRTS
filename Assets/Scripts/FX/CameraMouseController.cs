using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Core.Unit;

public class CameraMouseController : MonoBehaviour
{
    #region Camera
    [Header("Camera")]
    public float moveSpeed = 5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private float edgeSensitivity = 0.8f;   //중앙에서 얼마나 벗어나야 움직이는지
    private Camera mainCamera;
    #endregion

    #region Select
    [Header("Select")]
    [SerializeField]
    private LayerMask layerUnit;
    [SerializeField]
    private RTSUnitController rtsUnitController;
    [SerializeField]
    private SpawnManager spawn;
    #endregion

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



    private void Awake()
    {
        mainCamera = Camera.main;
    }


    void Update()
    {
        UpdateCamera();
        Drag();
        ClickUnit();
    }

    /// <summary>
    /// 마우스 우측 버튼을 클릭해야 카메라 이동이 활성화 된다.
    /// UI Btn과의 상호작용에 카메라가 임의로 이동하는 것을 방지하기 위함.
    /// </summary>
    void UpdateCamera()
    {
        if (!Input.GetMouseButton(1))
        {
            return;
        }

        // 마우스 위치 추적 부터 진행한다.
        Vector2 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 중심 피봇 기준으로 생각해야 한다.
        float horizontalRatio = ((mousePos.x / screenWidth) - 0.5f) * 2f;
        float verticalRatio = ((mousePos.y / screenHeight) - 0.5f) * 2f;

        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(horizontalRatio) > 1 - edgeSensitivity)
        {
            direction.x = horizontalRatio;
        }
        if (Mathf.Abs(verticalRatio) > 1 - edgeSensitivity)
        {
            direction.y = verticalRatio;
        }

        if (direction != Vector2.zero)
        {
            // 대각선 이동 시에도 동일 속도 업데이트
            direction = direction.normalized;
            Vector3 pos = mainCamera.transform.position;
            pos.x += direction.x * moveSpeed * Time.deltaTime;
            pos.y += direction.y * moveSpeed * Time.deltaTime;

            //맵 밖으로 나가지 않도록 제한
            pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
            pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

            mainCamera.transform.position = pos;
        }
    }

    /// <summary>
    /// 영역 내에 유닛이 있으면 해당 유닛들을 모두 Select 처리
    /// </summary>
    void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z * -1));
            dragBox = Instantiate(box, new Vector3(0, 0, 0), Quaternion.identity);
        }
        if (Input.GetMouseButton(0))
        {
            nowPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z * -1));
            deltaPos = startPos + (nowPos - startPos) / 2;
            deltaX = Mathf.Abs(startPos.x - nowPos.x);
            deltaY = Mathf.Abs(startPos.y - nowPos.y);
            dragBox.transform.position = deltaPos;
            dragBox.transform.localScale = new Vector3(deltaX, deltaY, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            float minX = Mathf.Min(startPos.x, nowPos.x);
            float maxX = Mathf.Max(startPos.x, nowPos.x);
            float minY = Mathf.Min(startPos.y, nowPos.y);
            float maxY = Mathf.Max(startPos.y, nowPos.y);

            
            var units = spawn.units;

            if (units != null)
            {
                // 스폰되어 있는 모든 UnitController를 순회하여 조사
                foreach (var unit in units)
                {
                    Vector3 unitPos = unit.transform.position;

                    // 드래그 영역 내부
                    if (unitPos.x >= minX && unitPos.x <= maxX && unitPos.y >= minY && unitPos.y <= maxY)
                    {
                        rtsUnitController.SelectUnit(unit);
                    }
                }
            }
            Destroy(dragBox);
        }
    }

    void ClickUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 2D Ray (Point 검사)
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, layerUnit);

            if (hit.collider != null)
            {
                var unit = hit.transform.GetComponent<UnitController>();
                if (unit == null) return;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rtsUnitController.ShiftClickSelectUnit(unit);
                }
                else
                {
                    rtsUnitController.ClickSelectUnit(unit);
                }
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 마우스 클릭 효과
                Vector3 spawnPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                spawnPos.z = 0f;
                Instantiate(clickE, spawnPos, Quaternion.identity);

                // 선택된 유닛들을 이동시킴
                rtsUnitController.MoveTo(new Vector2(spawnPos.x, spawnPos.y));

                rtsUnitController.DeselectAll();
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    rtsUnitController.DeselectAll();
                }
            }
        }
    }
}