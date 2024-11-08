using System;
using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(PlayerHand))]
    public class PlayerHandPostProcess : MonoBehaviour
    {
        private PlayerHand m_playerHand;
        
        private void Awake()
        {
            m_playerHand = GetComponent<PlayerHand>();

            m_playerHand.OnPileChange += OnPilePostProcess;
        }

        private void OnPilePostProcess(UICard[] cards)
        {
            foreach (var card in cards)
            {
                card.DisableRaycast();
                card.m_maintainingState = ECardMaintainingState.PlayerHand;
            }
        }
    }
}