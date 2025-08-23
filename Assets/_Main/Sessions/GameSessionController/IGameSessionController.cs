namespace EdwinGameDev.BubbleTeaMatch4
{
    public interface IGameSessionController
    {
        void StartGameMode<T>(T mode) where T : IGameMode;
        void EnableSoloCamera();
        void EnableVSCamera();
        GameSession GetOrCreateGameSession(int id, IInputProcessor inputProcessor);
    }
}