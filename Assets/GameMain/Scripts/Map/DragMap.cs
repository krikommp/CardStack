using System.Collections;
using Client.GameLogic.CardPile;
using GameMain.Scripts.Math;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.GameLogic.Map
{
    public class DragMap : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private PlayerHand m_playerHand;
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private CardMap m_cardMap;
        private RectTransform m_rectTransform;
        private CardBoundingBox m_boundingBox;
        
        private Vector2 m_dragLastPos;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            StartCoroutine(RefreshBoundingBox());
        }

        private IEnumerator RefreshBoundingBox()
        {
            yield return new WaitForEndOfFrame();
            
            m_boundingBox = new CardBoundingBox();

            var sizeDelta = m_rectTransform.sizeDelta;
            float halfWidth = sizeDelta.x * 0.5f;
            float halfHeight = sizeDelta.y * 0.5f;

            m_boundingBox.m_leftTop = new Vector2(-halfWidth, halfHeight);
            m_boundingBox.m_rightBottom = new Vector2(halfWidth, -halfHeight);
        }

        #region Implementation of IBeginDragHandler

        /// <summary>
        /// Called by a BaseInputModule before a drag is started.
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_dragLastPos = eventData.position;
        }

        #endregion

        #region Implementation of IDragHandler

        /// <summary>
        /// When dragging is occurring this will be called every time the cursor is moved.
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            var offset = eventData.position - m_dragLastPos;
            m_dragLastPos = eventData.position;
            
            m_cardMap.MoveToWithOffset(offset, m_boundingBox);
        }

        #endregion
    }
}

