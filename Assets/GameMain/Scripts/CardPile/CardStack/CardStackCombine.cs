using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using Client.GameLogic.Manager.GameManager;
using UnityEngine;
using TimeScheduledHandle = System.Int32;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackCombine : MonoBehaviour
    {
        private CardStack m_cardStack;
        private TimeScheduledHandle m_combineHandle = TimeManager.kInvalidTimeScheduledHandle;
        private bool m_cardStackDestroy = false;
        
        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();
        
            m_cardStack.OnPileChange += CheckCardCombine;
            m_cardStack.m_onCardStackCreate += OnCardStackCreate;
            m_cardStack.m_onCardStackDestroy += OnCardStackDestroy;
        }

        private void OnCardStackCreate()
        {
            m_cardStackDestroy = false;
            m_combineCards = new();
            m_usedFormula = null;
            m_cachedCardStack = null;
        }

        private void OnCardStackDestroy()
        {
            m_cardStackDestroy = true;
            if (m_combineHandle != TimeManager.kInvalidTimeScheduledHandle)
            {
                GlobalManager.TimeManager.CancelScheduledTask(m_combineHandle);
                CardCombineEnd();
            }
        }

        private class CardUnion
        {
            public int m_cardType;
            public List<UICard> m_cards;
        }
        
        private List<UICard> m_combineCards = new List<UICard>();
        private CardFormula m_usedFormula;

        private void CheckCardCombine(UICard[] cards)
        {
            var cardUnions = new List<CardUnion>();

            foreach (var card in cards)
            {
                if (cardUnions.Count == 0)
                {
                    var cardUnion = new CardUnion
                    {
                        m_cardType = card.CardData.Id,
                        m_cards = new List<UICard>() { card }
                    };
                    cardUnions.Add(cardUnion);
                }
                else
                {
                    if (cardUnions.Last().m_cardType == card.CardData.Id)
                    {
                        cardUnions.Last().m_cards.Add(card);
                    }
                    else
                    {
                        var cardUnion = new CardUnion
                        {
                            m_cardType = card.CardData.Id,
                            m_cards = new List<UICard>() { card }
                        };
                        cardUnions.Add(cardUnion);
                    }
                }
            }

            bool canCombine = true;
            foreach (var cardFormula in m_cardStack.CardFormulas)
            {
                canCombine = true;
                int validIdxInFormula = 0;
                foreach (int cardId in cardFormula.Formula)
                {
                    if (cardId == -1)
                    {
                        continue;
                    }

                    validIdxInFormula++;
                }

                if (cardUnions.Count != validIdxInFormula)
                {
                    canCombine = false;
                    continue;
                }

                
                for (int i = 0; i < validIdxInFormula; ++i)
                {
                    int cardType = cardFormula.Formula[i];

                    if (cardType != cardUnions[i].m_cardType)
                    {
                        canCombine = false;
                        break;
                    }
                }
                
                if (canCombine == false)
                {
                    continue;
                }
               
                for (int i = 0; i < validIdxInFormula; ++i)
                {
                    int weight = cardFormula.Weights[i];

                    if (i == 0)
                    {
                        if (cardUnions[i].m_cards.Count < weight)
                        {
                            canCombine = false;
                        }
                    }
                    else if (i == validIdxInFormula - 1)
                    {
                        if (cardUnions[i].m_cards.Count < weight)
                        {
                            canCombine = false;
                        }
                    }
                    else
                    {
                        if (cardUnions[i].m_cards.Count != weight)
                        {
                            canCombine = false;
                        }
                    }
                }

                if (canCombine == false)
                {
                    continue;
                }

                var combineCards = new List<UICard>();
                for (int i = 0; i < validIdxInFormula; ++i)
                {
                    int weight = cardFormula.Weights[i];
                    int cardLength = cardUnions[i].m_cards.Count;

                    for (int j = cardLength - weight; j < cardLength; ++j)
                    {
                        combineCards.Add(cardUnions[i].m_cards[j]);
                    }
                }

                if (combineCards.Count != 0)
                {
                    if (m_combineHandle != TimeManager.kInvalidTimeScheduledHandle)
                    {
                        GlobalManager.TimeManager.CancelScheduledTask(m_combineHandle);
                        CardCombineEnd();
                    }

                    m_combineCards = combineCards;
                    m_usedFormula = cardFormula;
                    canCombine = true;
                    
                    StartCardCombine();
                    break;
                }
            }

            if (canCombine == false && m_combineHandle != TimeManager.kInvalidTimeScheduledHandle)
            {
                GlobalManager.TimeManager.CancelScheduledTask(m_combineHandle);
                CardCombineEnd();
            }
        }

        private void StartCardCombine()
        {
            m_combineHandle = GlobalManager.TimeManager.ScheduleTaskAddMouth(m_usedFormula.SpeedTime, OnCardCombineFinish, false,
                out var endTime);
            m_cardStack.CombineProgressPrefab.StartCombine(endTime);
        }

        private CardStack m_cachedCardStack;
        private void OnCardCombineFinish()
        {
            for (int i = 0; i < m_combineCards.Count; ++i)
            {
                // TODO... 减少合成次数
                if (m_combineCards[i].CardData.DefaultCombinationCount != -1)
                {
                    m_cardStack.RemoveCard(m_combineCards[i], false);
                    GlobalManager.CardPool.ReturnToPool(m_combineCards[i]);
                }
            }
            
            if (m_cachedCardStack != null && m_cachedCardStack.gameObject.activeSelf != false)
            {
                m_cachedCardStack =
                    GlobalManager.CardManager.GiveCards(m_usedFormula.Result, m_usedFormula.Num, m_cachedCardStack);
            }
            else
            {
                var newCardPos = m_cardStack.RectTransform.anchoredPosition;

                if (m_cardStack.Cards.Length != 0)
                {
                    newCardPos.x += GlobalManager.AssetManager.CardParameterDevelopSettings.CardSizeDelta.x;
                    if (newCardPos.x > m_cardStack.DragArea.AABB.m_rightBottom.x)
                    {
                        newCardPos.x -= GlobalManager.AssetManager.CardParameterDevelopSettings.CardSizeDelta.x * 2;
                    }
                }
                
                m_cachedCardStack = GlobalManager.CardManager.GiveCards(m_usedFormula.Result, m_usedFormula.Num, newCardPos);
            }

            if (m_cardStackDestroy == false)
            {
                CardCombineEnd();
            }
            m_cardStack.NotifyPileChange();
        }

        private void CardCombineEnd()
        {
            m_combineCards.Clear();
            m_combineHandle = TimeManager.kInvalidTimeScheduledHandle;
            m_cardStack.CombineProgressPrefab.StopCombine();
        }
    }
}