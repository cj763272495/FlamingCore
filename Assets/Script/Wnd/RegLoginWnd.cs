 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegLoginWnd : MonoBehaviour
{
    public InputField username;
    private PlayersDataSystem pds;
    private bool isLogin ;
    private GameObject startCanvas;
    public GameObject loginGroup;

    public void Init() {
        gameObject.SetActive(true);
        pds = PlayersDataSystem.Instance;
        if(!pds.IsNewInviorment) {
            username.text = pds._playerID;
        }
        startCanvas = GameObject.Find("startCanvas").gameObject;
        isLogin = false;
        loginGroup.SetActive(true);
        startCanvas.SetActive(false);
    }

    public void ClickStart() {
        if(pds.PlayerLogin(username.text)) { 
            isLogin = true;
            loginGroup.SetActive(false);
            startCanvas.SetActive(true);
        }
    }

    //实现点击窗口任意位置关闭窗口
    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            if(!isLogin) {
                return;
            }
            gameObject.SetActive(false);
            GameRoot.Instance.EnterMainCity();
        }
    }
 
}

