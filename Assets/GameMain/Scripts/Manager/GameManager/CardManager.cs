using System.Collections;
using System.Collections.Generic;
using Client.GameLogic.Card;
using Client.GameLogic.CardPile;
using Client.GameLogic.PlayArea;
using UnityEngine;

namespace Client.GameLogic.Manager.GameManager
{
    public class CardManager : IManagerInitializable
    {
        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            yield return null;
            
            Debug.Log("[CardManager] 卡牌管理器初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.Low;

        #endregion

        private PlayerHand m_playerHand;
        private Dictionary<EDragAreaTag, DragArea> m_dragAreas;
        
        public DragArea GetDragArea(EDragAreaTag tag)
        {
            return m_dragAreas[tag];
        }

        public CardStack GiveACard(int cardId, Vector2 pos, EDragAreaTag dragAreaTag = EDragAreaTag.Default, bool isMouseInput = false)
        {
            return GiveCards(cardId, 1, pos, dragAreaTag, isMouseInput);
        }

        public CardStack GiveCards(int cardId, int num, Vector2 pos, EDragAreaTag dragAreaTag = EDragAreaTag.Default, bool isMouseInput = false)
        {
            if (!m_dragAreas.ContainsKey(dragAreaTag))
            {
                return null;
            }

            var cards = new UICard[num];

            for (int i = 0; i < num; ++i)
            {
                var card = UICard.Create(cardId);
                cards[i] = card;
            }
            
            var dragArea = m_dragAreas[dragAreaTag];
            dragArea.UpdateCardStackPhysics();
            
            var initParams = new CardStackInitParams()
            {
                m_dragArea = dragArea,
                m_cards = cards
            };
            
            var newCardStack = CardStackSpawn.SpawnCardStack(initParams, pos, isMouseInput);
            
            dragArea.AddCardStack(newCardStack);

            return newCardStack;
        }

        public CardStack GiveCards(int cardId, int num, CardStack cardStack)
        {
            for (int i = 0; i < num; ++i)
            {
                var card = UICard.Create(cardId);
                cardStack.AddCard(card);
            }

            return cardStack;
        }

        public CardStack GiveACard(int cardId, CardStack cardStack)
        {
            return GiveCards(cardId, 1, cardStack);
        }


        public void FindGameSceneObjects()
        {
            m_playerHand = GameObject.FindObjectOfType<PlayerHand>();
            
            m_dragAreas = new Dictionary<EDragAreaTag, DragArea>();
            var dragAreas = GameObject.FindObjectsOfType<DragArea>();
            foreach (var dragArea in dragAreas)
            {
                m_dragAreas.Add(dragArea.Tag, dragArea);
            }
        }
        
        public void ClearGameSceneObjects()
        {
            m_playerHand = null;
            
            m_dragAreas.Clear();
            m_dragAreas = null;
        }
    }
}