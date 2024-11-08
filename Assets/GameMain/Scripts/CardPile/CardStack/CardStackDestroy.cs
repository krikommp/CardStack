using System;
using System.Collections;
using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackDestroy : MonoBehaviour
    {
        private CardStack m_cardStack;
        
        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();
            m_cardStack.OnPileChange += OnPileChange;
        }

        private void OnPileChange(UICard[] cards)
        {
            if (cards.Length == 0)
            {
                m_cardStack.m_onCardStackDestroy?.Invoke();
                StartCoroutine(DelayDestroy());
            }
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForEndOfFrame();
            RemoveCardStackFromDragArea();
            GlobalManager.CardStackPool.ReturnToPool(m_cardStack);
        }

        private void RemoveCardStackFromDragArea()
        {
            m_cardStack.DragArea.RemoveCardStack(m_cardStack);
        }
    }
}