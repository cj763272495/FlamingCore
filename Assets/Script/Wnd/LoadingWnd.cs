using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : MonoBehaviour { 
    public Image logo; 

    private void Start() {
        ToolClass.BreathingImg(logo);
    }
      
    public void SetWndState(bool state = true) {
        gameObject.SetActive(state);
    }
}
