using System;
using System.Collections;
using System.Collections.Generic;
using Client.GameLogic.Card;
using Client.GameLogic.CardPile;
using Client.GameLogic.Manager;
using Client.GameLogic.State;
using Client.GameLogic.Util;
using GameMain.Scripts.Math;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.GameLogic.PlayArea
{
    public enum EDragAreaTag
    {
        Default,
    }

    public class DragArea : MonoBehaviour, IPointerClickHandler
    {
        private Canvas m_canvas;
        
        private RectTransform m_rectTransform;
        private List<CardStack> m_cardStacks;
        private CardBoundingBox m_boundingBox;
        private EDragAreaTag m_tag;
        
        public Canvas Canvas
        {
            get { return m_canvas; }
        }

        public RectTransform RectTransform
        {
            get { return m_rectTransform; }
        }

        public CardBoundingBox AABB
        {
            get { return m_boundingBox; }
        }
        
        public List<CardStack> CardStacks
        {
            get { return m_cardStacks; }
        }

        public EDragAreaTag Tag
        {
            get { return m_tag; }
        }

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_cardStacks = new List<CardStack>();
            m_boundingBox = new CardBoundingBox();
            m_canvas = m_rectTransform.GetRootCanvas();
            
            StartCoroutine(RefreshBoundingBox());
        }

        // private void FindCardStackOverlay(CardStack pendingCardStack)
        // {
        //     foreach (var cardStack in m_cardStacks)
        //     {
        //         if (pendingCardStack.AABB.IsOverlay(cardStack.AABB))
        //         {
        //             Debug.LogWarning("Overlay");
        //         }
        //     }
        // }

        public void RemoveCardStack(CardStack cardStack)
        {
            m_cardStacks.Remove(cardStack);
        }

        private IEnumerator RefreshBoundingBox()
        {
            yield return new WaitForEndOfFrame();
            
            m_boundingBox = new CardBoundingBox();

            var sizeDelta = m_rectTransform.sizeDelta;
            float halfWidth = sizeDelta.x * 0.5f;
            float halfHeight = sizeDelta.y * 0.5f;

            m_boundingBox.m_leftTop = new Vector2(-halfWidth, halfHeight);
            m_boundingBox.m_rightBottom = new Vector2(halfWidth, -halfHeight);
        }

        #region Implementation of IPointerClickHandler

        /// <summary>
        /// Use this callback to detect clicks.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!GlobalState.m_playerHand.HasCards)
            {
                return;
            }

            UpdateCardStackPhysics();
            
            
            var initParams = new CardStackInitParams
            {
                m_dragArea = this,
                m_cards = GlobalState.m_playerHand.ClearCards()
            };
            
            var newCardStack = CardStackSpawn.SpawnCardStack(initParams, Input.mousePosition, true);

            // FindCardStackOverlay(cardStack);
            AddCardStack(newCardStack);
        }
        #endregion

        public void UpdateCardStackPhysics()
        {
            foreach (var cardStack in m_cardStacks)
            {
                var cardStackPhysics = CardStackPhysics.GetComponent(cardStack);
                cardStackPhysics.UpdatePhysicsState(ECardStackPhysicsState.Move);
            }
        }

        public void AddCardStack(CardStack cardStack)
        {
            m_cardStacks.Add(cardStack);
        }
    }
}