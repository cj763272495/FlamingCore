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
        float alpha = Mathf.PingPong(Time.time, 1); // ����һ����0��1֮��仯��͸����ֵ
        Color color = logo.color;
        color.a = alpha; // ����͸����
        logo.color = color; // Ӧ���µ���ɫֵ
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
