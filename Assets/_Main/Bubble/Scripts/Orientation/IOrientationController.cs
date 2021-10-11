namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IOrientationController<T>
    {
        void SetLeftOrientation(T main, T sub);
        void SetRightOrientation(T main, T sub);
        void SetTopOrientation(T main, T sub);
        void SetBottomOrientation(T main, T sub);
    }
}