
using UnityEngine;

public class LoginSys:MonoBehaviour {
    public static LoginSys Instance = null;

    public RegLoginWnd loginWnd;
    private ResSvc resSvc;

    public void InitSys() {
        resSvc = ResSvc.Instance;
        Instance = this;
    }

    public void EnterLogin() {
        resSvc.AsyncLoadScene(Constants.StartScene, OpenLoginWnd);
    }

    public void OpenLoginWnd() { 
        UIManager.Instance.regLogInWnd.Init();
    } 
}
