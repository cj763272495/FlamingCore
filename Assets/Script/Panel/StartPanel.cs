using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    //todo��ʵ�ֵ��������Ļ��ʼ��Ϸ�ķ��� 
    
    void Start()
    {
        Invoke(nameof(GameStart), 1);
    }

    private void GameStart() { 
        GameRoot.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
