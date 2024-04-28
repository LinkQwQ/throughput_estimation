using System;
using UnityEngine;

using UnityEditor;
using UnityEngine.UI;


public class Exit : MonoBehaviour
{
    public Button exit;

    private void Start()
    {
        exit.onClick.AddListener(OnExitButtonClick); 
        
    }

    void OnExitButtonClick()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        // 在构建的游戏中
        Application.Quit();
        #endif
    }
    
}