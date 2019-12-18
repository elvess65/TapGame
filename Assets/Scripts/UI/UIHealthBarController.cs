using UnityEngine;
using UnityEngine.UI;

namespace WhaleAppTapGame.UI
{
    public class UIHealthBarController : MonoBehaviour
    {
        public Image Image_FG;

        private int m_MaxHP;

        public void Init(int maxHP) => m_MaxHP = maxHP;

        public void UpdateHealthBar(int currentHP) => Image_FG.fillAmount = (float)currentHP / m_MaxHP;
    }
}
