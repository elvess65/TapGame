using UnityEngine;

namespace WhaleAppTapGame.Logic.Input
{
    public interface iInputManager
    {
        event System.Action<GameObject> OnInputExecuted;

        void UpdateComponent(float deltaTime);
    }
}
