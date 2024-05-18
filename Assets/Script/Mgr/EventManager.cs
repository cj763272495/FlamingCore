using System;
using UnityEngine;

public class EventManager:MonoBehaviour {
    // ����һ���¼�������ұ�����ʱ����
    public static event Action<PlayerController> OnPlayerLoaded;

    // ����һ����������������������¼�
    public static void PlayerLoaded(PlayerController player) {
        OnPlayerLoaded?.Invoke(player);
    }
}
