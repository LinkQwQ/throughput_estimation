using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public AnimationCurve panelIn;
    public AnimationCurve panelOut;
    public float speed;
    public GameObject panel;
    public Button button;
    public Button close;
    private bool isPanelVisible = false;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        close.onClick.AddListener(OnCloseButtonClick);
        panel.transform.localScale = Vector3.zero;  // 初始化面板为不可见
    }

    void OnButtonClick()
    {
        if (!isPanelVisible)
        {
            StartCoroutine(ShowPanel());
        }
    }
    void OnCloseButtonClick()
    {
        if (isPanelVisible)
        {
            StartCoroutine(HidePanel());
        }
    }

    IEnumerator ShowPanel()
    {
        float timer = 0;
        while (timer < 1)
        {
            float scale = panelIn.Evaluate(timer);
            panel.transform.localScale = new Vector3(scale, scale, scale);
            timer += Time.deltaTime * speed;
            yield return null;
        }
        panel.transform.localScale = new Vector3(1, 1, 1); // 确保最终值为1
        isPanelVisible = true;
    }

    IEnumerator HidePanel()
    {
        float timer = 0;
        while (timer < 1)
        {
            float scale = panelOut.Evaluate(timer);
            panel.transform.localScale = new Vector3(scale, scale, scale);
            timer += Time.deltaTime * speed;
            yield return null;
        }
        panel.transform.localScale = Vector3.zero; // 确保面板完全关闭
        isPanelVisible = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isPanelVisible)
        {
            StartCoroutine(HidePanel());
        }
    }
}