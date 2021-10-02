namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IInputController
    {
        void CheckInputs();
        void SetBubbles(Bubble mainBubble, Bubble subBubble);
        bool ValidateAndMoveDown();
    }
}