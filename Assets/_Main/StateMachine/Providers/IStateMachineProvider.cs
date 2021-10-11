using System;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IStateMachineProvider
    {
        StateMachine GetStateMachine(Action OnStartGame, Action OnGameOver);        
    }
}