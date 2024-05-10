using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolClass
{
     public static void SetGameObjectPosXZ(GameObject player, Transform targetTrans) {
        player.transform.position = new Vector3(targetTrans.position.x, player.transform.position.y, targetTrans.position.z);
    }
}
