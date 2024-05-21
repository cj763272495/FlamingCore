using System.Collections.Generic; 
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

    public void ChangeStatus(Entity entity,AniState targetState,params object[] args) {
        if(entity.curAniState == targetState) {
            return;
        }
        if(fsm.ContainsKey(targetState)) {
            if(entity.curAniState == targetState) {
                return;
            }
            if(entity.curAniState != AniState.None) {
                fsm[entity.curAniState].Exit(entity,args);
            }
            fsm[targetState].Enter(entity,args);
            fsm[targetState].Process(entity,args);
        }
    }
}
