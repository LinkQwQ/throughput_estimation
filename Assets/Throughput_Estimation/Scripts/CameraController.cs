using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 3f; // 摄像机移动速度
    public float zoomSpeed = 1f; // 缩放速度
    public float minZoom = 5f; // 最小缩放值
    public float maxZoom = 20f; // 最大缩放值

    void Update()
    {
        // 方向键移动
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
    }
}