using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.UI;

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc;
    public FloatingJoystick joystick;
    private int coin = 0;

    public void EarnCoin(int num) {
        coin += num;
    }

    public int GetCoinNum() {
        return coin;
    }

    public void Init(int mapid, Action cb = null) {
        resSvc = ResSvc.Instance;
        resSvc.AsyncLoadScene("SampleScene", () => {
            //GameObject map = GameObject.FindGameObjectWithTag("MapRoot");

            //map.transform.localPosition = Vector3.zero;
            //map.transform.localScale = Vector3.one;

            //Camera.main.transform.position = mapCfg.mainCamPos;
            //Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            //LoadPlayer();
            //entitySelfPlayer.Idle();

            if (cb != null) {
                cb();
            }
        });
    }

    private void LoadPlayer() {
        GameObject player = resSvc.LoadPrefab("Prefab/Player");
        player.transform.position = Vector3.one;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.joystick = joystick;
        //playerController.Init();
    }
}
