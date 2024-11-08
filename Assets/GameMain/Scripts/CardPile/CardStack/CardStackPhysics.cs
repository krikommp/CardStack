using System;
using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using Client.GameLogic.PlayArea;
using Client.GameLogic.Util;
using GameMain.Scripts.Math;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    public enum ECardStackPhysicsState
    {
        DotMove,
        Move,
    }

    [RequireComponent(typeof(CardStack))]
    public class CardStackPhysics : MonoBehaviour
    {
        private CardStack m_cardStack;
        private ECardStackPhysicsState m_state = ECardStackPhysicsState.DotMove;
        
        public ECardStackPhysicsState CardStackPhysicsState => m_state;

        public static CardStackPhysics GetComponent(CardStack cardStack)
        {
            return cardStack == null ? null : cardStack.GetComponent<CardStackPhysics>();
        }

        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();
        }

        public void OnCardStackTrigger(CardStack other)
        {
            if (m_state == ECardStackPhysicsState.Move)
            {
                var dir =  transform.position - other.transform.position;
                dir.Normalize();
                var perpendicular = new Vector2(dir.x, dir.y);
                m_cardStack.MoveWithOffset(perpendicular * 250.0f * Time.fixedDeltaTime);
            } 
        }

        public void UpdatePhysicsState(ECardStackPhysicsState state)
        {
            m_state = state;
        }
    }
}