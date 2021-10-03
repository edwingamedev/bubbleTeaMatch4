namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IInputProcessor
    {
        bool StartGame();
        bool Left();

        bool Right();

        bool Down();

        bool TurnClockwise();

        bool TurnCounterClockwise();
    }
}