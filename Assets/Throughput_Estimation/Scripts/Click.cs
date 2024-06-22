using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class HighlightTileOnClick : MonoBehaviour
{
    public Tilemap tilemap; // 引用Tilemap
    private Color highlightColor = Color.red; // 高亮颜色，默认为黄色
    private Color originalColor; // 用于存储原始颜色
    public Tile APTile;
    public Tile APDualTile;// 新的Tile用于替换
    public Tile HostTile; // Host模式下使用的Tile
    public UnityEngine.UI.Text Location;
    //Button List
    //public Button Clear;
    public Button Host;
    public Button AP;
    public Button Save;

    public Button Reset;
    ////Button List
    private bool isHostMode = false;
    private bool isAPMode = false;// 是否处于Host模式
    private Vector3Int lastClickedPosition; // 存储最后点击的位置
    private List<string[]> clickPositions = new List<string[]>(); // 用于存储所有点击的位置
    private Vector3Int? previousAPPosition = null;
    
    private int APnum = 0;
    private int Hostnum = 0;


    private void Start()
    {
        originalColor = tilemap.color; // 在开始时获取Tilemap的初始颜色
        
        Button host = Host.GetComponent<Button>();
        Button ap = AP.GetComponent<Button>();
        //Button clear = Clear.GetComponent<Button>();
        
        ap.onClick.AddListener(APOnClick);
        host.onClick.AddListener(HostOnClick);
        //clear.onClick.AddListener(ClearOnClick);
        Save.onClick.AddListener(SaveOnClick);
        Reset.onClick.AddListener(ReloadCurrentScene);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            if (EventSystem.current.IsPointerOverGameObject()) // 检查点击是否在UI元素上
            {
                Debug.Log("Clicked on UI, ignoring gameplay interaction.");
                return; // 如果在UI元素上点击，忽略后续的游戏内操作
            }
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPos); // 将鼠标位置转换为格子位置
            
           
            if (tilemap.HasTile(gridPosition)) // 检测是否点击在有Tile的位置
            {
                lastClickedPosition = gridPosition; // 更新最后点击的位置
                
                if (isHostMode)
                {
                    ChangeToHost(gridPosition);
                    Hostnum += 1;
                    float New_X = gridPosition.x / 2.0f;
                    float New_Y = gridPosition.y / 2.0f;
                    clickPositions.Add(new string[] { "H"+Hostnum, New_X.ToString(), New_Y.ToString(), "Host" }); // 添加坐标和一个描述
                    //clickPositions.Add(new string[] { "H"+Hostnum, gridPosition.x.ToString(), gridPosition.y.ToString(), "Host" }); // 添加坐标和一个描述
                }
                if (isAPMode)
                {
                    if (previousAPPosition.HasValue && previousAPPosition.Value == gridPosition)
                    {
                        // 同时更改颜色以突出显示重复点击
                        //tilemap.SetColor(gridPosition, highlightColor);
                        tilemap.SetTile(gridPosition, APDualTile);
                        Debug.Log("Repeated click on the same tile in AP mode.");
                    }
                    else
                    {
                        ChangeToAp(gridPosition);
                        previousAPPosition = gridPosition; // 更新存储的位置
                    }
                    APnum += 1;
                    float New_X = gridPosition.x / 2.0f;
                    float New_Y = gridPosition.y / 2.0f;
                    clickPositions.Add(new string[] { "AP"+APnum, New_X.ToString(), New_Y.ToString(), "AP" }); // 添加坐标和一个描述
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
        
    }

    // 进入AP选择模式
    void APOnClick()
    {
        isHostMode = false; // 确保关闭Host模式
        isAPMode = true; // 开启AP模式

    }
    void ChangeToAp(Vector3Int position)
    {
        tilemap.SetTile(position, APTile); // 替换为APTile
        StartCoroutine(ResetTileColor(position)); // 延时后重置颜色
    }


    // 进入host选择模式
    void HostOnClick()
    {
        isHostMode = true; // 开启Host模式
        isAPMode = false; 

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

    }

    IEnumerator ResetTileColor(Vector3Int position)
    {
        yield return new WaitForSeconds(1); // 等待1秒
        tilemap.SetColor(position, originalColor); // 重置为原始颜色
    }

    void SaveOnClick()
    {
        if (clickPositions.Count > 0)
        {
            SaveFile.WriteCSV(clickPositions.ToArray());
            clickPositions.Clear(); // 清空列表以防重复写入
        }
    }

    void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 重新加载当前场景
        SceneManager.LoadScene(currentSceneName);
    }
   

}
