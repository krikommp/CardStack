using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackSorter : MonoBehaviour
    { 
        private CardStack m_cardStack;
        
        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();
            
            m_cardStack.OnPileChange += SortCardStack;
        }
        
        private void SortCardStack(UICard[] cards)
        {
            var cardPileOffset = GlobalManager.AssetManager.CardParameterDevelopSettings.CardPileOffset;
            
            for (int i = 0; i < cards.Length; ++i)
            {
                cards[i].MoveTo(new Vector3(0, i * cardPileOffset.y, 1.0f));
            }
            
            var cardSizeDelta = GlobalManager.AssetManager.CardParameterDevelopSettings.CardSizeDelta;
            m_cardStack.CombineProgressPrefab.RectTransform.localPosition = new Vector3(0, cardSizeDelta.y * (-0.5f) + cards.Length * cardPileOffset.y, 0);
        }
    }
}