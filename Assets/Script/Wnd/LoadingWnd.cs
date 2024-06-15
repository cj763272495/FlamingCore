using UnityEngine; 

public class LoadingWnd : MonoBehaviour
{ 
    public void StartLoading() {//显示Start加载界面且同时获取加载数据
       gameObject.SetActive(true); 
    }

    public void LoadingEnd() {
        gameObject.SetActive(false);
    }
     
}
