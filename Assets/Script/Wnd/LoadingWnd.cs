using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : MonoBehaviour { 
    public Image imgFG;
    public Image imgPoint;
    public Text txtPrg;

    public Image logo;

    private float fgWidth;

    private void Start() {
        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
        txtPrg.text =  "0%";
        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector3(-545f, 0, 0);
    }

    private void Update() {
        float alpha = Mathf.PingPong(Time.time, 1); // 创建一个在0到1之间变化的透明度值
        Color color = logo.color;
        color.a = alpha; // 更新透明度
        logo.color = color; // 应用新的颜色值
    }

    public void SetProgress(float prg) {
        txtPrg.text =  (int)(prg * 100) + "%";
        imgFG.fillAmount = prg;

        float posX = prg * fgWidth - 545;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);

    }

    public void SetWndState(bool state = true) {
        gameObject.SetActive(state);
    }
}
