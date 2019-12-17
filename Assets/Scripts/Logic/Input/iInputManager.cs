namespace WhaleAppTapGame.Logic.Input
{
    public interface iInputManager
    {
        event System.Action OnInputExecuted;

        void UpdateComponent(float deltaTime);
    }
}
