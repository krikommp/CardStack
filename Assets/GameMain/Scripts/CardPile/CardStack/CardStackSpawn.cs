using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using Client.GameLogic.PlayArea;
using GameMain.Scripts.Math;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardStack))]
    public class CardStackSpawn : MonoBehaviour
    {
        private CardStack m_cardStack;
        
        public static CardStack SpawnCardStack(CardStackInitParams initParams, Vector2 targetPos, bool isMouseInput = true)
        {
            var cardStack = GlobalManager.CardStackPool.Get(initParams);

            if (isMouseInput)
            {
                Debug.LogError($"mouse pos:    {targetPos}");
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    initParams.m_dragArea.RectTransform, 
                    targetPos, 
                    initParams.m_dragArea.Canvas.worldCamera,
                    out var position);

                cardStack.MoveTo(position);
            }
            else
            {
                cardStack.MoveTo(targetPos);
            }

            return cardStack;
        }

        private void Awake()
        {
            m_cardStack = GetComponent<CardStack>();
        }
        
        public static CardStackSpawn GetComponent(CardStack cardStack)
        {
            return cardStack == null ? null : cardStack.GetComponent<CardStackSpawn>();
        }
    }
}