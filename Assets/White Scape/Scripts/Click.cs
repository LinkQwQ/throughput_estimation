using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Net.Mime;

public class HighlightTileOnClick : MonoBehaviour
{
    public Tilemap tilemap; // 引用Tilemap
    public Color highlightColor = Color.yellow; // 高亮颜色，默认为黄色
    private Color originalColor; // 用于存储原始颜色
    public UnityEngine.UI.Text Location;
    private void Start()
    {
        originalColor = tilemap.color; // 在开始时获取Tilemap的初始颜色
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPos); // 将鼠标位置转换为格子位置
           
            if (tilemap.HasTile(gridPosition)) // 检测是否点击在有Tile的位置
            {
                HighlightTile(gridPosition);
                Debug.Log("Tile clicked at position: " + gridPosition);
                string convert = gridPosition.ToString();
                Location.text = convert;
            }
        }
    }

    void HighlightTile(Vector3Int position)
    {
        tilemap.SetColor(position, highlightColor); // 设置高亮颜色
        StartCoroutine(ResetTileColor(position)); // 延时后重置颜色
    }

    IEnumerator ResetTileColor(Vector3Int position)
    {
        yield return new WaitForSeconds(1); // 等待1秒
        tilemap.SetColor(position, originalColor); // 重置为原始颜色
    }
}