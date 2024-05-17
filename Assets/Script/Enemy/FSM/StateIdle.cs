
//出生状态
public class StateIdle: IState {
    public void Enter(EnemyEntity entity,params object[] args) {
        entity.curAniState = AniState.Idle;
    }

    public void Exit(EnemyEntity entity,params object[] args) {
    }

    public void Process(EnemyEntity entity,params object[] args) {
        if(entity.ani) {
            entity.ani.Play("Idel");
        }
    }
}
