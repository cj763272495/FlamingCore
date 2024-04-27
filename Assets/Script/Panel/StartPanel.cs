using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    //todo：实现点击触摸屏幕开始游戏的方法 
    
    void Start()
    {
        Invoke(nameof(GameStart), 1);
    }

    private void GameStart() { 
        GameRoot.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
