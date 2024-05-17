using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class StateMgr : MonoBehaviour
{
    private Dictionary<AniState,IState> fsm = new Dictionary<AniState,IState>();

    public void Init() {
        fsm.Add(AniState.Born,new StateBorn());
        fsm.Add(AniState.Idle,new StateIdle());
        fsm.Add(AniState.Take,new StateTake());
        //fsm.Add(AniState.Attack,new StateAttack()); 
    }

    public void ChangeStatus(EnemyEntity enemy,AniState targetState,params object[] args) {
        if(enemy.curAniState == targetState) {
            return;
        }
        if(fsm.ContainsKey(targetState)) {
            if(enemy.curAniState == targetState) {
                return;
            }
            if(enemy.curAniState != AniState.None) {
                fsm[enemy.curAniState].Exit(enemy,args);
            }
            fsm[targetState].Enter(enemy,args);
            fsm[targetState].Process(enemy,args);
        }
    }
}
