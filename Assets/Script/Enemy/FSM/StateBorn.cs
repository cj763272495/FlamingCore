
//出生状态
public class StateBorn:IState {
    public void Enter(Entity entity,params object[] args) {
        entity.curAniState = AniState.Born;
    }

    public void Exit(Entity entity,params object[] args) {

    }

    public void Process(Entity entity,params object[] args) {
        if(entity.ani) {
            entity.ani.Play("Birth"); 
        }
    }

}
