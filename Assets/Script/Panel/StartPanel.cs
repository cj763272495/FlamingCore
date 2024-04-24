using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(GameStart), 1);
    }

    private void GameStart() { 
        GameRoot.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
