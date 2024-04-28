using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class MouseEnter : MonoBehaviour
{
    // Start is called before the first frame update
    bool Loc;
    public bool WindowShow = false;
    public Tilemap tilemap;
    void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        Loc = true;
    }
    
    private void OnMouseExit()
    {
        Loc = false;
    }

    private void OnGUI()
    {
        if (Loc)
        {
            GUIStyle style1 = new GUIStyle();
            style1.fontSize = 25;
            style1.normal.textColor = Color.black;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPos);
            Vector2 gridPosition2D = new Vector2(gridPosition.x/2.0f, gridPosition.y/2.0f);
  
            GUI.Label(new Rect(Input.mousePosition.x,Screen.height - Input.mousePosition.y,400,50),gridPosition2D.ToString("F1"),style1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
