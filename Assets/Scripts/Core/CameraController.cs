using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private float edgeSensitivity = 0.8f;   //중앙에서 얼마나 벗어나야 움직이는지

    // Update is called once per frame
    void Update()
    {
        
        UpdateCamera();
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
            Vector3 pos = transform.position;
            pos.x += direction.x * moveSpeed * Time.deltaTime;
            pos.y += direction.y * moveSpeed * Time.deltaTime;

            //맵 밖으로 나가지 않도록 제한
            pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
            pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

            transform.position = pos;
        }
    }
}
