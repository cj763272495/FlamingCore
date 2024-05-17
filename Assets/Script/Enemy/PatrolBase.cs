using UnityEngine;

public class PatrolBase: MonoBehaviour {
    public Transform[] pathTrans;
    public float speed = 2;
    public int moveDir = 1; 
    public bool loopMove = true;

    private int _curIndex = 0;

    private void Update() {
        Move();
    }

    public void Move() { 
        if(loopMove) {
            LoopMove(transform);
        } else {
            BackMove(transform);
        }
    }

    private void LoopMove(Transform transform) {//正向循环移动
        if(transform.position != pathTrans[_curIndex].position) {
            transform.position = Vector3.MoveTowards(transform.position,
                pathTrans[_curIndex].position,speed * Time.deltaTime);
        } else {
            _curIndex++;
        }
        if(_curIndex > pathTrans.Length - 1) {
            _curIndex = 0;
        }
    }

    private void BackMove(Transform transform) { //到终点返回
        if(transform.position != pathTrans[_curIndex].position) {
            transform.position = Vector3.MoveTowards(transform.position, pathTrans[_curIndex].position,speed * Time.deltaTime);
        } else {
            _curIndex += moveDir;
        }
        if(_curIndex == pathTrans.Length - 1) {
            moveDir = -1;
        }
        if(_curIndex == 0) {
            moveDir = 1;
        }
    }
}
