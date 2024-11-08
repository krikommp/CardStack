using UnityEngine;

namespace GameMain.Scripts.Math
{
    public class CardBoundingBox
    {
        public Vector2 m_leftTop;
        public Vector2 m_rightBottom;

        public CardBoundingBox()
        {
            m_leftTop = new Vector2(float.MaxValue, float.MinValue);
            m_rightBottom = new Vector2(float.MinValue, float.MaxValue);
        }
        
        public CardBoundingBox(Vector2 leftTop, Vector2 rightBottom)
        {
            m_leftTop = leftTop;
            m_rightBottom = rightBottom;
        }

        public bool IsOverlay(CardBoundingBox other)
        {
            if (m_leftTop.x < other.m_rightBottom.x
                && m_rightBottom.x > other.m_leftTop.x
                && m_leftTop.y > other.m_rightBottom.y
                && m_rightBottom.y < other.m_leftTop.y)
            {
                return true;
            }

            return false;
        }

        public bool IsIn(CardBoundingBox other)
        { 
            if (m_leftTop.x > other.m_leftTop.x
              && m_rightBottom.x < other.m_rightBottom.x
              && m_leftTop.y < other.m_leftTop.y
              && m_rightBottom.y > other.m_rightBottom.y)
            {
                return true;
            }

            return false;
        }

        #region Overrides of Object

        public override string ToString()
        {
            return $"{m_leftTop} -> {m_rightBottom}";
        }

        #endregion
    }
}