using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 3f; // 摄像机移动速度
    public float smoothTime = 0.3f; // 平滑移动的时间
    private Vector3 velocity = Vector3.zero;

    public float zoomSpeed = 1f; // 缩放速度
    public float minZoom = 5f; // 最小缩放值
    public float maxZoom = 20f; // 最大缩放值

    public float boundaryPercentage = 0.05f; // 边界百分比，这里设置为10%

    void Update()
    {
        float boundaryWidth = Screen.width * boundaryPercentage;
        float boundaryHeight = Screen.height * boundaryPercentage;

        Vector3 mousePosition = Input.mousePosition;
        bool isWithinBoundary = mousePosition.x < boundaryWidth || mousePosition.x > Screen.width - boundaryWidth ||
                                mousePosition.y < boundaryHeight || mousePosition.y > Screen.height - boundaryHeight;

        if (isWithinBoundary)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            targetPosition.z = transform.position.z; // 2D 游戏没有Z轴的数值

            // 使用SmoothDamp平滑移动到目标位置
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            // 重置速度向量，消除惯性
            velocity = Vector3.zero;
        }

        // 滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }
}