using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using Client.GameLogic.PlayArea;
using Client.GameLogic.Util;
using GameMain.Scripts.CardCombine;
using GameMain.Scripts.Math;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    public class CardStackInitParams
    {
        public DragArea m_dragArea;
        public UICard[] m_cards;
    }

    public class CardStack : CardPile, IGameObjectPoolable
    {
        private DragArea m_dragArea;
        private CardBoundingBox m_aabb;
        private int m_cardRoot;
        private List<CardFormula> m_cardFormulas;
        [SerializeField] private CardCombineProgress m_combineProgressPrefab;
        
        public DragArea DragArea
        {
            get { return m_dragArea; }
        }

        public CardBoundingBox AABB
        {
            get { return m_aabb; }
        }

        public int CardRoot
        {
            get { return m_cardRoot; }
        }

        public List<CardFormula> CardFormulas
        {
            get { return m_cardFormulas; }
        }
        
        public CardCombineProgress CombineProgressPrefab
        {
            get { return m_combineProgressPrefab; }
        }

        #region Event

        public Action m_onCardStackCreate = () => { };
        public Action m_onCardStackDestroy = () => { };
        public Action m_onCardStackMoveFinish = () => { };

        #endregion

        #region Implementation of IGameObjectPoolable

        public void PoolInit(object arg)
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_cards = new();
            m_aabb = new();

            var initParams = (CardStackInitParams)arg;

            if (initParams == null)
            {
                Debug.LogError($"[CardStack] PoolInit 失败，参数为空");
                return;
            }
            
            m_dragArea = initParams.m_dragArea;
            m_canvas = initParams.m_dragArea.Canvas;
            m_rectTransform.SetParent(initParams.m_dragArea.RectTransform, false);
            m_combineProgressPrefab.gameObject.SetActive(false);

            SetupCardStackInfo(initParams);
            m_onCardStackCreate?.Invoke();
            foreach (var card in initParams.m_cards)
            {
                AddCard(card);
            }
        }

        public void PoolRecycle()
        {
            m_rectTransform = null;
            m_cards = null;
            m_aabb = null;
        }

        private void SetupCardStackInfo(CardStackInitParams initParams)
        {
            if (initParams == null)
            {
                return;
            }

            if (initParams.m_cards == null || initParams.m_cards.Length == 0)
            {
                return;
            }

            m_cardRoot = initParams.m_cards[0].CardData.Id;
            m_cardFormulas = GlobalManager.AssetManager.TbFormula.DataList.Where(data => data.Formula[0] == m_cardRoot).ToList();
        }

        #endregion

        #region Motion

        public void MoveTo(Vector2 pos)
        {
            m_rectTransform.anchoredPosition = LimitCardStackPlacementRange(pos);
            var newBoundingBox = CardTool.CalcCardStackBoundingBox(m_rectTransform.anchoredPosition, m_cards.Count);
            UpdateBoundingBox(newBoundingBox.m_leftTop, newBoundingBox.m_rightBottom);
        }

        public void MoveWithOffset(Vector2 offset)
        {
            MoveTo(m_rectTransform.anchoredPosition + offset);
        }

        private Vector2 LimitCardStackPlacementRange(Vector2 newCardStackPos)
        {
            var newBoundingBox = CardTool.CalcCardStackBoundingBox(newCardStackPos, m_cards.Count);
            
            var offset = new Vector2();
            if (newBoundingBox.m_leftTop.x <= m_dragArea.AABB.m_leftTop.x)
            {
                float value = Mathf.Abs(m_dragArea.AABB.m_leftTop.x - newBoundingBox.m_leftTop.x);
                offset.x = value;
            }
            if (newBoundingBox.m_rightBottom.x >= m_dragArea.AABB.m_rightBottom.x)
            {
                float value = Mathf.Abs(newBoundingBox.m_rightBottom.x - m_dragArea.AABB.m_rightBottom.x);
                offset.x = -value;
            }

            if (newBoundingBox.m_leftTop.y >= m_dragArea.AABB.m_leftTop.y)
            {
                float value = Mathf.Abs(newBoundingBox.m_leftTop.y - m_dragArea.AABB.m_leftTop.y);
                offset.y = -value;
            }
            
            if (newBoundingBox.m_rightBottom.y <= m_dragArea.AABB.m_rightBottom.y)
            {
                float value = Mathf.Abs(m_dragArea.AABB.m_rightBottom.y - newBoundingBox.m_rightBottom.y);
                offset.y = value;
            }

            return newCardStackPos + offset;
        }
        
        #endregion
        

        #region BoundingBox

        public void UpdateBoundingBox(Vector2 leftTop, Vector2 rightBottom)
        {
            m_aabb.m_leftTop = leftTop;
            m_aabb.m_rightBottom = rightBottom;
        }

        #endregion
        
    }
}