namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IInputController
    {
        void CheckInputs();
        void SetBubbles(BubbleSet bubbleSet);
        bool ValidateAndMoveDown();
        bool StartGame();
    }
}