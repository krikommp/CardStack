using System;
using System.Collections.Generic;
using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    /// <summary>
    /// 卡堆
    /// </summary>
    public interface ICardPile
    {
        /// <summary>
        /// 卡堆变化回调函数
        /// </summary>
        Action<UICard[]> OnPileChange { get; set; }
        
        /// <summary>
        /// 向卡堆中添加卡牌
        /// </summary>
        void AddCard(UICard card, bool needNotify = true);
        
        /// <summary>
        /// 从卡堆中移除卡牌
        /// </summary>
        void RemoveCard(UICard card, bool needNotify = true);
    }
    
    public class CardPile : MonoBehaviour, ICardPile
    {
        protected List<UICard> m_cards;
        protected RectTransform m_rectTransform; 
        protected Canvas m_canvas;
        
        private event Action<UICard[]> m_onPileChange = cards => { };
        
        public Canvas Canvas
        {
            get { return m_canvas; }
        }
        
        public RectTransform RectTransform
        {
            get { return m_rectTransform; }
        }

        public UICard[] Cards
        {
            get { return m_cards.ToArray(); }
        }

        #region Implementation of ICardPile

        /// <summary>
        /// 卡堆变化回调函数
        /// </summary>
        public Action<UICard[]> OnPileChange
        {
            get { return m_onPileChange; }
            set { m_onPileChange = value; }
        }

        /// <summary>
        /// 向卡堆中添加卡牌
        /// </summary>
        public void AddCard(UICard card, bool needNotify = true)
        {
            if (card == null) {
                throw new ArgumentNullException("card");
            }
            
            var cam = Canvas.worldCamera;
            var pos = RectTransformUtility.WorldToScreenPoint(cam, card.RectTransform.position);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_rectTransform, 
                pos, 
                cam,
                out var position);
            
            card.MoveTo(new Vector3(position.x, position.y, 1.0f));
            
            card.RectTransform.SetParent(m_rectTransform, false);
            
            m_cards.Add(card);
            
            if (needNotify)
            {
                NotifyPileChange();
            }
        }

        /// <summary>
        /// 从卡堆中移除卡牌
        /// </summary>
        public void RemoveCard(UICard card, bool needNotify = true)
        {
            if (card == null) {
                throw new ArgumentNullException("card");
            }

            m_cards.Remove(card);

            if (needNotify)
            {
                NotifyPileChange();
            }
        }

        /// <summary>
        /// 将卡牌从一个卡堆移动到另一个卡堆
        /// </summary>
        /// <param name="otherPile">需要移动到的卡堆</param>
        /// <param name="index">从该位置开始往后所有的卡牌</param>
        public void MoveCard(CardPile otherPile, int index)
        {
            Debug.Assert(index < m_cards.Count, "[CardPile] 卡牌索引错误");

            var cardsToMove = new List<UICard>();
            for (int i = index; i < m_cards.Count; ++i)
            {
                cardsToMove.Add(m_cards[i]);
            }

            foreach (var cardToMove in cardsToMove)
            {
                RemoveCard(cardToMove);
            }

            foreach (var cardToMove in cardsToMove)
            {
                otherPile.AddCard(cardToMove);
            }
        }
        
        public UICard[] ClearCards()
        {
            var cards = m_cards.ToArray();
            m_cards.Clear();
            NotifyPileChange();
            return cards;
        }

        #endregion
        
        public void NotifyPileChange()
        {
            m_onPileChange?.Invoke(m_cards.ToArray());
        }
    }
}