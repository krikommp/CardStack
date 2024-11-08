using Client.GameLogic.Manager;
using GameMain.Scripts.Math;
using UnityEngine;

namespace Client.GameLogic.Util
{
    public static class CardTool
    {
        public static CardBoundingBox CalcCardStackBoundingBox(Vector2 anchoredPosition, int cardCount)
        {
            var cardSizeDelta = GlobalManager.AssetManager.CardParameterDevelopSettings.CardSizeDelta;
            var cardPileOffset = GlobalManager.AssetManager.CardParameterDevelopSettings.CardPileOffset;
            var cardOverlayOffset = GlobalManager.AssetManager.CardParameterDevelopSettings.CardOverlayOffset;
            
            var leftTop = new Vector2(anchoredPosition.x - (cardSizeDelta.x * 0.5f) - cardOverlayOffset.x, anchoredPosition.y + (cardSizeDelta.y * 0.5f) + cardOverlayOffset.y);
            var rightBottom = new Vector2(anchoredPosition.x + (cardSizeDelta.x * 0.5f) + cardOverlayOffset.x, anchoredPosition.y - (cardSizeDelta.y * 0.5f) - (Mathf.Abs(cardPileOffset.y)) * (cardCount - 1) - cardOverlayOffset.y);
            
            return new CardBoundingBox(leftTop, rightBottom);
        }
    }
}