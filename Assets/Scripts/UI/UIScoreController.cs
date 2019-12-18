using UnityEngine;
using UnityEngine.UI;

namespace WhaleAppTapGame.UI
{
    public class UIScoreController : MonoBehaviour
    {
        public Text Text_Score;

        public void UpdateScore(int score) => Text_Score.text = $"Score: {score}";
    }
}
