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
            LoopMove();
        } else {
            BackMove();
        }
    }

    private void LoopMove() {//����ѭ���ƶ�
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

    private void BackMove() { //���յ㷵��
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
