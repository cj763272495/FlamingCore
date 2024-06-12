using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class RegLoginWnd : MonoBehaviour,IPointerDownHandler
{
    public InputField username;
    private PlayersDataSystem pds;
    private bool isLogin ;
    private GameObject startCanvas;
    public GameObject loginGroup;
    private GameObject logo;

    public void Init() {
        gameObject.SetActive(true);
        pds = PlayersDataSystem.Instance;
        if(!pds.IsNewInviorment) {
            username.text = pds.playerID;
        } else {
            username.Select();
            username.ActivateInputField();
        }
        startCanvas = GameObject.Find("startCanvas").gameObject;
        logo = GameObject.Find("Logo").gameObject;
        isLogin = false;
        loginGroup.SetActive(true);
        startCanvas.SetActive(false);
        logo.SetActive(false);
    }

    public void ClickStart() {
        if(string.IsNullOrEmpty(username.text)) {
            UIManager.Instance.ShowUserMsg("请输入用户名");
            return;
        }
        if(pds.PlayerLogin(username.text)) { 
            isLogin = true;
            loginGroup.SetActive(false);
            startCanvas.SetActive(true);
            logo.SetActive(true);
        }
    }
     

    //实现点击窗口任意位置关闭窗口
    //private void Update() {
    //    if(Input.GetMouseButtonDown(0)) {
    //        if(!isLogin) {
    //            return;
    //        }
    //        gameObject.SetActive(false);
    //        GameRoot.Instance.EnterMainCity();
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData) {
        if(Input.GetMouseButtonDown(0)) {
            if(!isLogin) {
                return;
            }
            gameObject.SetActive(false);
            GameRoot.Instance.EnterMainCity();
        }
    }
}

