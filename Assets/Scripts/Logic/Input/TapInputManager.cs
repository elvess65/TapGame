namespace WhaleAppTapGame.Logic.Input
{
    public class TapInputManager : iInputManager
    {
        public event System.Action OnInputExecuted;

        public void UpdateComponent(float deltaTime)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
                OnInputExecuted?.Invoke();
        }
    }
}
