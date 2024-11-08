using Client.GameLogic.Card;
using Client.GameLogic.Event;
using Client.GameLogic.State;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackPicker : MonoBehaviour
    {
        private CardStack m_cardStack;
        
        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();
            m_cardStack.m_onCardStackCreate += OnCardStackCreate;
            m_cardStack.m_onCardStackDestroy += OnCardStackDestroy;
        }

        private void OnCardStackDestroy()
        {
            GlobalEvents.m_onCardPicked -= OnCardPicked;
        }

        private void OnCardStackCreate()
        {
            GlobalEvents.m_onCardPicked += OnCardPicked;
        }

        private void OnCardPicked(UICard card)
        {
            if (GlobalState.m_playerHand == null)
            {
                return;
            }

            int pickedCardIndex = -1;
            for (int i = 0; i < m_cardStack.Cards.Length; ++i)
            {
                if (m_cardStack.Cards[i] == card)
                {
                    pickedCardIndex = i;
                }
            }

            if (pickedCardIndex == -1)
            {
                return;
            }
            
            if (GlobalState.m_playerHand.HasCards)
            {
                // 1. 判断是否是同一类型的卡牌
                // 2. 如果是同一类型，判断是否可以堆叠
                // 3. 如果不是统一类型，判断是否可以组合，并触发组合逻辑
                GlobalState.m_playerHand.MoveCard(m_cardStack, 0);
                return;
            }

            m_cardStack.MoveCard(GlobalState.m_playerHand, pickedCardIndex);
        }
    }
}