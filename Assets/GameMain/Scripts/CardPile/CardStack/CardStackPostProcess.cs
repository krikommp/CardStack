using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackPostProcess: MonoBehaviour
    {
        private CardStack m_cardStack;
        
        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();

            m_cardStack.OnPileChange += OnPilePostProcess;
        }

        private void OnPilePostProcess(UICard[] cards)
        {
            foreach (var card in cards)
            {
                card.EnableRaycast();
                card.m_maintainingState = ECardMaintainingState.CardStack;
            }
        }
    }
}