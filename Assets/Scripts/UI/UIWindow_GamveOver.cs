using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WhaleAppTapGame.UI
{
    public class UIWindow_GamveOver : MonoBehaviour
    {
        public Button Button_Replay;
        public UIScoreController PlayerScoreController;


        public void Init()
        {
            Button_Replay.onClick.AddListener(ButtonReplay_PressHandler);
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public void Show(int score)
        {
            PlayerScoreController.UpdateScore(score);
            gameObject.SetActive(true);
        }


        void ButtonReplay_PressHandler() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
