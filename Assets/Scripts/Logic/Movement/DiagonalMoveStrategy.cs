using UnityEngine;

namespace WhaleAppTapGame.Logic.Movement
{
    public class DiagonalMoveStrategy : SimpleMoveStrategy
    {
        public DiagonalMoveStrategy(Transform controlledTransform, float speed, Vector2 screenBounds, Vector2 spriteBounds)
            : base(controlledTransform, speed, screenBounds, spriteBounds)
        {
            m_MoveDir.x = Random.Range(0, 100) > 50 ? 1 : -1;
        }

        public override void Move()
        {
            float targetPosX = m_ControlledTransform.position.x + m_MoveDir.x * m_Speed * Time.deltaTime;

            if (targetPosX <= -m_ScreenBounds.x + m_SpriteBounds.x ||
                targetPosX >= m_ScreenBounds.x - m_SpriteBounds.x)
                m_MoveDir.x *= -1;

            base.Move();
        }
    }
}
