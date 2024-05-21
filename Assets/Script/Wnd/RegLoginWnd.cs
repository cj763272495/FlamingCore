using UnityEngine; 
using UnityEngine.UI;

public class RegLoginWnd : MonoBehaviour
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
            username.text = pds._playerID;
        }
        startCanvas = GameObject.Find("startCanvas").gameObject;
        logo = GameObject.Find("Logo").gameObject;
        isLogin = false;
        loginGroup.SetActive(true);
        startCanvas.SetActive(false);
        logo.SetActive(false);
    }

    public void ClickStart() {
        if(pds.PlayerLogin(username.text)) { 
            isLogin = true;
            loginGroup.SetActive(false);
            startCanvas.SetActive(true);
            logo.SetActive(true);
        }
    }

    //ʵ�ֵ����������λ�ùرմ���
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

