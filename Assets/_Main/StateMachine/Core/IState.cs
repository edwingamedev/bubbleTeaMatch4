namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IState
    {
        void OnEnter();
        void Tick();
        void OnExit();
    }
}