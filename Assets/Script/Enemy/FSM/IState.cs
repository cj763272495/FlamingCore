public interface IState {
    void Enter(EnemyEntity enemy, params object[] args);

    void Process(EnemyEntity enemy, params object[] args);

    void Exit(EnemyEntity enemy, params object[] args);

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