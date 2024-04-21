using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{ 
    public void ClickReturn() {//返回主界面
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
    public void ClickRetry() {//再试一次
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }
    public void ClickBack() {//继续游戏
        GameRoot.Instance.ClickPauseBtn();
    }
}
