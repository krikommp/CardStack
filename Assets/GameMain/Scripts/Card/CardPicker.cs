using System;
using Client.GameLogic.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.GameLogic.Card
{
    [RequireComponent(typeof(UICard))]
    public class CardPicker : MonoBehaviour, IPointerClickHandler
    {
        private UICard m_card;

        private void Awake()
        {
            m_card = GetComponent<UICard>();
        }

        #region Implementation of IPointerClickHandler

        /// <summary>
        /// Use this callback to detect clicks.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            GlobalEvents.m_onCardPicked?.Invoke(m_card);
        }

        #endregion
    }
}