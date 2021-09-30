namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IInputProcessor
    {
        bool MoveLeft();

        bool MoveRight();

        bool MoveDown();

        void TurnClockwise();

        void TurnCounterClockwise();
    }
}