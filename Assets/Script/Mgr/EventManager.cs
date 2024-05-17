using System;
using UnityEngine;

public class EventManager:MonoBehaviour {
    // 定义一个事件，当玩家被加载时触发
    public static event Action<PlayerController> OnPlayerLoaded;

    // 定义一个方法，用来触发上面的事件
    public static void PlayerLoaded(PlayerController player) {
        OnPlayerLoaded?.Invoke(player);
    }
}
