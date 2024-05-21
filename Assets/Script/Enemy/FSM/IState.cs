public interface IState {
    void Enter(Entity enemy, params object[] args);

    void Process(Entity enemy, params object[] args);

    void Exit(Entity enemy, params object[] args);

}

public enum AniState {
    None,
    Idle,
    Take,
    Attack,
    Born,
    //Die,
    //Hit,
}