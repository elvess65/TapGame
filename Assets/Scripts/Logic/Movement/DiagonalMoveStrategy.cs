using UnityEngine;

namespace WhaleAppTapGame.Logic.Movement
{
    public class DiagonalMoveStrategy : SimpleMoveStrategy
    {
        private bool m_IsMovingDiagonal = false;

        public DiagonalMoveStrategy(Transform controlledTransform, float speed, Vector2 screenBounds, Vector2 spriteBounds)
            : base(controlledTransform, speed, screenBounds, spriteBounds)
        {
            m_Speed *= 1.5f;

            m_IsMovingDiagonal = Random.Range(0, 100) > 50;
            m_IsMovingDiagonal = true;

            if (m_IsMovingDiagonal)
                m_MoveDir.x = Random.Range(0, 100) > 50 ? 1 : -1;
        }

        public override void Move()
        {
            float targetPosX = m_ControlledTransform.position.x + m_MoveDir.x * m_Speed * Time.deltaTime;

            if (m_IsMovingDiagonal && (targetPosX <= -m_ScreenBounds.x + m_SpriteBounds.x ||
                                       targetPosX >= m_ScreenBounds.x - m_SpriteBounds.x))
                m_MoveDir.x *= -1;

            base.Move();
        }
    }
}
