using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HomWnd;
    public GameObject PauseWnd;
    public static UIManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
     
    public void OpenPasueWnd(bool open = true) {
        PauseWnd.SetActive(open);
    }

    public void GameOver() {
        PauseWnd.SetActive(true);
    }

}
