using UnityEngine;

namespace WhaleAppTapGame.Logic.Input
{
    public class TapInputManager : iInputManager
    {
        public event System.Action<GameObject> OnInputExecuted;

        public void UpdateComponent(float deltaTime)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                    OnInputExecuted?.Invoke(hit.collider.gameObject);
            }
        }
    }
}
