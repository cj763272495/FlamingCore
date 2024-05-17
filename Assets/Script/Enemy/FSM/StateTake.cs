
//出生状态
public class StateTake: IState {
    public void Enter(EnemyEntity entity,params object[] args) {
        entity.curAniState = AniState.Take;
    }

    public void Exit(EnemyEntity entity,params object[] args) {
    }

    public void Process(EnemyEntity entity,params object[] args) {
        if(entity.ani) {
            if(!entity.ani.GetCurrentAnimatorStateInfo(0).IsName("Take 001")) {
                entity.ani.Play("Take 001");
            }
        }
    }
}
