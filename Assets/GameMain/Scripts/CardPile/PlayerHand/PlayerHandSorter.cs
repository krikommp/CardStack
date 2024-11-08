using System;
using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(PlayerHand))]
    public class PlayerHandSorter : MonoBehaviour
    {
        private PlayerHand m_playerHand;

        private void Awake()
        {
            m_playerHand = GetComponent<PlayerHand>();

            m_playerHand.OnPileChange += OnPlayerHandCardsChanged;
        }

        private void OnPlayerHandCardsChanged(UICard[] cards)
        {
            MoveAllCardsToMousePos(cards);
        }

        private void MoveAllCardsToMousePos(UICard[] cards)
        {
            // 将鼠标的屏幕位置转换为在 Canvas 里的位置
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_playerHand.RectTransform, 
                Input.mousePosition, 
                m_playerHand.Canvas.worldCamera,
                out var position);

            float movementSpeed = GlobalManager.AssetManager.CardParameterDevelopSettings.MovementSpeed;
            var cardPileOffset = GlobalManager.AssetManager.CardParameterDevelopSettings.CardPileOffset;
            
            for (int i = 0; i < cards.Length; ++i)
            {
                cards[i].MoveTo(new Vector3(position.x + (i * cardPileOffset.x), position.y + (i * cardPileOffset.y), 1.0f), movementSpeed);
            }
        }
    }
}