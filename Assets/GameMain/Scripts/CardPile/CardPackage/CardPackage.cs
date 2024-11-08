using System;
using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using Client.GameLogic.PlayArea;
using Client.GameLogic.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.GameLogic.CardPile
{
    public class CardPackage : CardPile
    {
        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_canvas = m_rectTransform.GetRootCanvas();
            m_cards = new();
        }

        public void Discard(UICard card)
        {
            m_cards.Remove(card);
        }

        public void Test()
        {
            var pos = GetCardSpawnPosition();
            
            GlobalManager.CardManager.GiveACard(310001, pos, EDragAreaTag.Default, true);
        }

        public Vector2 GetCardSpawnPosition()
        {
            var screenPos = m_canvas.worldCamera.WorldToScreenPoint(gameObject.transform.position);

            var cardSettings = GlobalManager.AssetManager.CardParameterDevelopSettings;

            screenPos.y += cardSettings.CardSizeDelta.y + cardSettings.CardOverlayOffset.y;
            screenPos.x += Random.Range(-100, 100);

            return screenPos;
        }
    }
}