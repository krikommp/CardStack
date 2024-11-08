using System;
using Client.GameLogic.Card;
using Client.GameLogic.CardPile;
using Client.GameLogic.Manager;
using Client.GameLogic.PlayArea;
using UnityEngine;

namespace Client.GameLogic.Test
{
    public class TestGenerateCard : MonoBehaviour
    {
        public Canvas m_canvas;
        public PlayerHand m_playerHand;

        private UICard m_card;
        
        public void OnGenerateCard()
        {
            Vector2 screenPos = new Vector2(520.00f, 147.50f);

            GlobalManager.CardManager.GiveACard(310001, screenPos, EDragAreaTag.Default, true);
        }
    }
}