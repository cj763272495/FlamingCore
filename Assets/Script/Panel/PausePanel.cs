using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public BattleWnd battleWnd;

    public void ClickReturn() {
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }
    public void ClickRetry() {
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }
    public void ClickBack() {
        GameRoot.Instance.ClickPauseBtn();
    }


}
