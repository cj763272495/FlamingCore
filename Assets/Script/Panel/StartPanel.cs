using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    //todo：实现点击触摸屏幕开始游戏的方法 

    public float breathRate = 2.0f;
    public Image BreathImage;

    void Start() {
        //可以使用Invoke显示logo等
        GameStart();
    }

    private void GameStart() {//显示Start加载界面且同时获取加载数据
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
        float alpha = Mathf.SmoothStep(0,1,t); // 平滑过渡
        BreathImage.color = new Color(1,1,1,alpha);
    }

}
