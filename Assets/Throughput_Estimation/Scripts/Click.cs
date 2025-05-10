using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighlightTileOnClick : MonoBehaviour
{
    public Tilemap tilemap;
    private Color highlightColor = Color.red;
    private Color originalColor;
    public Tile APTile;
    public Tile APDualTile;
    public Tile HostTile;
    public UnityEngine.UI.Text Location;

    public Button Host;
    public Button AP;
    public Button Save;
    public Button Reset;

    public List<Tilemap> wallTilemaps; // ✅ 拖入 CorridorWall、PartitionWall

    private bool isHostMode = false;
    private bool isAPMode = false;
    private Vector3Int lastClickedPosition;
    private Vector3Int? previousAPPosition = null;
    private int APnum = 0;
    private int Hostnum = 0;

    private List<string[]> clickPositions = new List<string[]>();
    private Dictionary<string, Vector3Int> apPositions = new Dictionary<string, Vector3Int>();
    private Dictionary<string, Vector3Int> hostPositions = new Dictionary<string, Vector3Int>();

    private void Start()
    {
        originalColor = tilemap.color;
        AP.onClick.AddListener(APOnClick);
        Host.onClick.AddListener(HostOnClick);
        Save.onClick.AddListener(SaveOnClick);
        Reset.onClick.AddListener(ReloadCurrentScene);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPos);

            if (tilemap.HasTile(gridPosition))
            {
                lastClickedPosition = gridPosition;

                if (isHostMode)
                {
                    ChangeToHost(gridPosition);
                    Hostnum++;
                    float New_X = gridPosition.x / 2.0f;
                    float New_Y = gridPosition.y / 2.0f;
                    string id = "H" + Hostnum;
                    clickPositions.Add(new string[] { id, New_X.ToString(), New_Y.ToString(), "Host" });
                    hostPositions[id] = gridPosition;
                }
                else if (isAPMode)
                {
                    if (previousAPPosition.HasValue && previousAPPosition.Value == gridPosition)
                    {
                        tilemap.SetTile(gridPosition, APDualTile);
                    }
                    else
                    {
                        ChangeToAp(gridPosition);
                        previousAPPosition = gridPosition;
                    }
                    APnum++;
                    float New_X = gridPosition.x / 2.0f;
                    float New_Y = gridPosition.y / 2.0f;
                    string id = "AP" + APnum;
                    clickPositions.Add(new string[] { id, New_X.ToString(), New_Y.ToString(), "AP" });
                    apPositions[id] = gridPosition;
                }
                else
                {
                    HighlightTile(gridPosition);
                }

                Location.text = gridPosition.ToString();
            }
        }
    }

    void HighlightTile(Vector3Int position)
    {
        tilemap.SetColor(position, highlightColor);
    }

    void APOnClick() { isAPMode = true; isHostMode = false; }
    void HostOnClick() { isHostMode = true; isAPMode = false; }

    void ChangeToAp(Vector3Int position)
    {
        tilemap.SetTile(position, APTile);
        StartCoroutine(ResetTileColor(position));
    }

    void ChangeToHost(Vector3Int position)
    {
        tilemap.SetTile(position, HostTile);
        StartCoroutine(ResetTileColor(position));
    }

    IEnumerator ResetTileColor(Vector3Int position)
    {
        yield return new WaitForSeconds(1);
        tilemap.SetColor(position, originalColor);
    }

    void SaveOnClick()
    {
        if (clickPositions.Count > 0)
        {
            SaveFile.WriteCSV(clickPositions.ToArray());

            List<string[]> wallResults = new List<string[]>();
            wallResults.Add(new string[] { "host", "ap", "w1", "w2", "w3", "w4", "w5", "w6" });

            foreach (var h in hostPositions)
            {
                foreach (var a in apPositions)
                {
                    int[] walls = CountWallTypesBetween(h.Value, a.Value);
                    wallResults.Add(new string[] {
                        h.Key, a.Key,
                        walls[1].ToString(), walls[2].ToString(),
                        walls[3].ToString(), walls[4].ToString(), walls[5].ToString(), walls[6].ToString()
                    });
                }
            }

            SaveFile.WriteWallStatsCSV(wallResults.ToArray());
            clickPositions.Clear();
        }
    }

    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    int[] CountWallTypesBetween(Vector3Int start, Vector3Int end)
    {
        int[] counts = new int[7];
        int x0 = start.x, y0 = start.y;
        int x1 = end.x, y1 = end.y;
        int dx = Mathf.Abs(x1 - x0), dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            Vector3Int pos = new Vector3Int(x0, y0, 0);

            foreach (var wallMap in wallTilemaps)
            {
                TileBase tile = wallMap.GetTile(pos);
                if (tile != null)
                {
                    Debug.Log($"🧱 Detected tile at {pos} on {wallMap.name} with tile: {tile.name}");
                    string layerName = wallMap.transform.name;
                    int wallType = -1;

                    if (layerName == "CorridorWall") wallType = 1;
                    else if (layerName == "PartitionWall") wallType = 2;
                    // 可拓展更多判断

                    if (wallType >= 0 && wallType < 7)
                    {
                        counts[wallType]++;
                    }
                }
            }

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }

        return counts;
    }
}
