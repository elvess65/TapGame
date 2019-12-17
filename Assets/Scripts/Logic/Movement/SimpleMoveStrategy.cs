using UnityEngine;

namespace WhaleAppTapGame.Logic.Movement
{
    public class SimpleMoveStrategy : iMoveStrategy
    {
        public event System.Action OnOutOfBottomBound;

        protected float m_Speed;
        protected Vector3 m_MoveDir;
        protected Vector2 m_ScreenBounds;
        protected Vector2 m_SpriteBounds;
        protected Transform m_ControlledTransform;


        public SimpleMoveStrategy(Transform controlledTransform, float speed, Vector2 screenBounds, Vector2 spriteBounds)
        {
            m_Speed = speed;
            m_MoveDir = -Vector3.up;
            
            m_ScreenBounds = screenBounds;
            m_SpriteBounds = spriteBounds;

            m_ControlledTransform = controlledTransform;
        }

        public virtual void Move()
        {
            if (m_ControlledTransform.position.y <= -m_ScreenBounds.y - m_SpriteBounds.y)
            {
                OnOutOfBottomBound?.Invoke();
                OnOutOfBottomBound = null;
            }

            m_ControlledTransform.Translate(m_MoveDir.normalized * m_Speed * Time.deltaTime);
        }
    }
}
