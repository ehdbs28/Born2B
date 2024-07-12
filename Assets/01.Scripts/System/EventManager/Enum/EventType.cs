public enum EventType
{
    OnPlayerDead,
    OnPlayerDamaged,
    OnPlayerTurnOver,       // 플레이어의 턴이 넘어갔을 때
    OnPlayerAttacked,
    
    OnTurnEnded,            // 전체 턴이 넘어갔을 때
    OnTurnChanged, //턴이 바뀌었을때 TurnType넘어옴 0번 : 이전 턴, 1번 : 바뀔 턴 

    //플로우 관련 이벤트들
    OnGameStart,
    OnSelectPlayerUnit,
    OnStageJoin,
    OnBattleStart,
    OnBattleFinish,

}