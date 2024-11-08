using System;
using Client.GameLogic.State;
using Client.GameLogic.Util;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    public class PlayerHand : CardPile
    {
        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_canvas = m_rectTransform.GetRootCanvas();
            m_cards = new();

            GlobalState.m_playerHand = this;
        }
        
        public bool HasCards
        {
            get { return m_cards.Count > 0; }
        }
    }
}