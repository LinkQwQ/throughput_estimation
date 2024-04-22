using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;

public class HighlightTileOnClick : MonoBehaviour
{
    public Tilemap tilemap; // 引用Tilemap
    public Color highlightColor = Color.yellow; // 高亮颜色，默认为黄色
    private Color originalColor; // 用于存储原始颜色
    public Tile APTile; // 新的Tile用于替换
    public Tile HostTile; // Host模式下使用的Tile
    public UnityEngine.UI.Text Location;
    public Button Clear;
    public Button Host;
    public Button AP;
    private bool isHostMode = false; // 是否处于Host模式
    private Vector3Int lastClickedPosition; // 存储最后点击的位置

    private void Start()
    {
        originalColor = tilemap.color; // 在开始时获取Tilemap的初始颜色
        
        Button host = Host.GetComponent<Button>();
        Button ap = AP.GetComponent<Button>();
        Button clear = Clear.GetComponent<Button>();
        
        ap.onClick.AddListener(APOnClick);
        host.onClick.AddListener(HostOnClick);
        clear.onClick.AddListener(ClearOnClick);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPos); // 将鼠标位置转换为格子位置
           
            if (tilemap.HasTile(gridPosition)) // 检测是否点击在有Tile的位置
            {
                lastClickedPosition = gridPosition; // 更新最后点击的位置
                if (isHostMode)
                {
                    ChangeToHost(gridPosition);
                }
                else
                {
                    HighlightTile(gridPosition);
                }
                Debug.Log("Tile clicked at position: " + gridPosition);
                Location.text = gridPosition.ToString();
            }
        }
    }

    void HighlightTile(Vector3Int position)
    {
        tilemap.SetColor(position, highlightColor); // 设置高亮颜色
        StartCoroutine(ResetTileColor(position)); // 延时后重置颜色
    }

    // 进入AP选择模式
    void APOnClick()
    {
        isHostMode = false; // 关闭Host模式
        Debug.Log("AP mode activated");
    }

    // 进入host选择模式
    void HostOnClick()
    {
        isHostMode = true; // 开启Host模式
        Debug.Log("Host mode activated");
    }

    void ChangeToHost(Vector3Int position)
    {
        tilemap.SetTile(position, HostTile); // 替换为HostTile
        StartCoroutine(ResetTileColor(position)); // 延时后重置颜色
    }
    
    // 清除
    void ClearOnClick()
    {
        isHostMode = false; // 关闭Host模式
        Debug.Log("Clear mode activated");
    }

    IEnumerator ResetTileColor(Vector3Int position)
    {
        yield return new WaitForSeconds(1); // 等待1秒
        tilemap.SetColor(position, originalColor); // 重置为原始颜色
    }
}
