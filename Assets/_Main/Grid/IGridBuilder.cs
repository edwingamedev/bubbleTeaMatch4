namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IGridBuilder
    {
        void Build();
        Grid Grid { get; }
    }
}