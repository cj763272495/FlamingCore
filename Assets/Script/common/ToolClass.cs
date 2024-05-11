using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolClass
{
     public static void SetGameObjectPosXZ(GameObject player, Vector3 pos) {
        player.transform.position = new Vector3(pos.x, player.transform.position.y,pos.z);
    }
}
