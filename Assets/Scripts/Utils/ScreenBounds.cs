using UnityEngine;

namespace WhaleAppTapGame.Utils
{ 
    public static class ScreenBounds 
    {
        private static Vector2 m_ScreenBounds = Vector2.zero;

        public static Vector2 GetScreenBounds()
        {
            //Cache screen bounds at first call
             if (m_ScreenBounds == Vector2.zero)
                m_ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            return m_ScreenBounds;
        }
    }
}
