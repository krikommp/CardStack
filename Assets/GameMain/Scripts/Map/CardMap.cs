using System;
using System.Collections.Generic;
using Client.GameLogic.PlayArea;
using GameMain.Scripts.Math;
using UnityEngine;

namespace Client.GameLogic.Map
{
    public class CardMap : MonoBehaviour
    {
        [SerializeField] private List<DragArea> m_dragAreas;
        private RectTransform m_rectTransform;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        #region Motion

        public void MoveToWithOffset(Vector2 offset, CardBoundingBox boundingBox)
        {
            MoveTo(m_rectTransform.anchoredPosition + offset, boundingBox);
        }
        
        public void MoveTo(Vector2 position, CardBoundingBox boundingBox)
        {
            var sizeDelta = m_rectTransform.sizeDelta;

            float halfWidth = sizeDelta.x * 0.5f;
            float halfHeight = sizeDelta.y * 0.5f;

            if (position.x > halfWidth + boundingBox.m_leftTop.x)
            {
                position.x = halfWidth + boundingBox.m_leftTop.x;
            }

            if (position.x < -halfWidth + boundingBox.m_rightBottom.x)
            {
                position.x = -halfWidth + boundingBox.m_rightBottom.x;
            }

            if (position.y > halfHeight - boundingBox.m_leftTop.y)
            {
                position.y = halfHeight - boundingBox.m_leftTop.y;
            }

            if (position.y < -halfHeight - boundingBox.m_rightBottom.y)
            {
                position.y = -halfHeight - boundingBox.m_rightBottom.y;
            }

            m_rectTransform.anchoredPosition = position;
        }

        #endregion
    }
}