using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    //todo��ʵ�ֵ��������Ļ��ʼ��Ϸ�ķ��� 

    public float breathRate = 2.0f;
    public Image BreathImage;

    void Start() {
        //����ʹ��Invoke��ʾlogo��
        GameStart();
    }

    private void GameStart() {//��ʾStart���ؽ�����ͬʱ��ȡ��������
        gameObject.SetActive(true);
        InvokeRepeating(nameof(Breathe),0f,breathRate);

        GameRoot.Instance.GameStart();  
        ResSvc.Instance.AsyncLoadScene("StartScene",() => {
            gameObject.SetActive(false);
            UIManager.Instance.regLogInWnd.Init();
        });
    }

    void Breathe() {
        float t = Mathf.PingPong(Time.time,breathRate) / breathRate;
        float alpha = Mathf.SmoothStep(0,1,t); // ƽ������
        BreathImage.color = new Color(1,1,1,alpha);
    }

}
