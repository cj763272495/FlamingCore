
//出生状态
public class StateTake: IState {
    public void Enter(Entity entity,params object[] args) {
        entity.curAniState = AniState.Take;
    }

    public void Exit(Entity entity,params object[] args) {
    }

    public void Process(Entity entity,params object[] args) {
        if(entity.ani) {
            if(!entity.ani.GetCurrentAnimatorStateInfo(0).IsName("Take 001")) {
                entity.ani.Play("Take 001");
            }
        }
    }
}
