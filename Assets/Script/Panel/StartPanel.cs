using DG.Tweening; 
using UnityEngine;
using UnityEngine.UI; 

public class StartPanel : MonoBehaviour
{
    public float breathRate = 2.0f;
    public Image BreathImage;

    void Start() {
        //可以使用Invoke显示logo等
        GameStart();
        DOTween.Init();
    }

    private void GameStart() {//显示Start加载界面且同时获取加载数据
        gameObject.SetActive(true);
       ToolClass.BreathingImg(BreathImage);

        GameRoot.Instance.GameStart();  
        ResSvc.Instance.AsyncLoadScene("StartScene",() => {
            gameObject.SetActive(false);
            UIManager.Instance.regLogInWnd.Init();
        });
    }
     
}
