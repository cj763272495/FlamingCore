using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{ 
    public void ClickReturn() {//����������
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
    public void ClickRetry() {//����һ��
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }
    public void ClickBack() {//������Ϸ
        GameRoot.Instance.ClickPauseBtn();
    }
}
