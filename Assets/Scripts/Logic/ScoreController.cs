namespace WhaleAppTapGame.Logic
{
    public class ScoreController
    {
        public System.Action<int> OnScoreChanged;

        private int m_ScorePerEnemy;

        public int Score { get; private set; }


        public ScoreController(int scorePerEnemy) => m_ScorePerEnemy = scorePerEnemy;

        public void IncrementScore()
        {
            Score += m_ScorePerEnemy;

            OnScoreChanged?.Invoke(Score);
        }
    }
}
