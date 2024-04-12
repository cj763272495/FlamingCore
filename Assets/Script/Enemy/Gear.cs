using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Enemy
{

    public Transform[] trans;
    public int cur_index=0;
    private float speed=2;
    public int moveDir=1;
    private float GearRotateSpeed = 500;
    public bool loopMove = true;

    private void Start() {
        transform.position = new Vector3(trans[0].position.x,
            transform.position.y, trans[0].position.z);
    }
    private void Update() {
        transform.Rotate(Vector3.up * GearRotateSpeed * Time.deltaTime);
        if (loopMove) {
            LoopMove();
        } else {
            BackMove();
        }
    }

    private void LoopMove() {//正向循环移动
        if (transform.position != trans[cur_index].position) { 
            transform.position = Vector3.MoveTowards(transform.position,
                trans[cur_index].position, speed * Time.deltaTime);
        } else {
            cur_index ++;
        }
        if (cur_index > trans.Length - 1) {
            cur_index = 0;
        } 
    }

    private void BackMove() { //到终点返回
        if (transform.position != trans[cur_index].position) { 
            transform.position = Vector3.MoveTowards(transform.position,
                trans[cur_index].position, speed * Time.deltaTime);
        } else {
            cur_index += moveDir;
        }
        if (cur_index == trans.Length - 1) {
            moveDir = -1;
        }
        if (cur_index == 0) {
            moveDir = 1;
        }
    }
}
