using UnityEngine;

namespace WhaleAppTapGame.UI
{
    public class UIManager : MonoBehaviour
    {
        public UIHealthBarController PlayerHealthBarController;
        public UIWindow_GamveOver UIWindow_GameOver;
        public UIScoreController PlayerScoreController;

        public void Init()
        {
            PlayerScoreController.UpdateScore(0);
            UIWindow_GameOver.Init();
        }

        public void ShowUIWindow_GameOver(int score)
        {
            PlayerScoreController.gameObject.SetActive(false);
            PlayerHealthBarController.gameObject.SetActive(false);

            UIWindow_GameOver.Show(score);
        }
    }
}
