using UnityEngine; 

public class LoadingWnd : MonoBehaviour
{ 
    public void StartLoading() {//��ʾStart���ؽ�����ͬʱ��ȡ��������
       gameObject.SetActive(true); 
    }

    public void LoadingEnd() {
        gameObject.SetActive(false);
    }
     
}
