using System;
using Client.GameLogic.State;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardPackage))]
    public class CardPackagePicker : MonoBehaviour, IPointerClickHandler
    {
        private CardPackage m_cardPackage;

        private void Awake()
        {
            m_cardPackage = GetComponent<CardPackage>();
        }

        #region Implementation of IPointerClickHandler

        /// <summary>
        /// Use this callback to detect clicks.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogWarning("Click CardPackagePicker");
            if (GlobalState.m_playerHand == null)
            {
                return;
            }
            
            GlobalState.m_playerHand.MoveCard(m_cardPackage, 0);
        }

        #endregion
    }
}