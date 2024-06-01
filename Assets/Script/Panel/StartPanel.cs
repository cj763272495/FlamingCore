using DG.Tweening; 
using UnityEngine;
using UnityEngine.UI; 

public class StartPanel : MonoBehaviour
{
    public float breathRate = 2.0f;
    public Image BreathImage;

    void Start() {
        //����ʹ��Invoke��ʾlogo��
        GameStart();
        DOTween.Init();
    }

    private void GameStart() {//��ʾStart���ؽ�����ͬʱ��ȡ��������
        gameObject.SetActive(true);
       ToolClass.BreathingImg(BreathImage);

        GameRoot.Instance.GameStart();  
        ResSvc.Instance.AsyncLoadScene("StartScene",() => {
            gameObject.SetActive(false);
            UIManager.Instance.regLogInWnd.Init();
        });
    }
     
}
