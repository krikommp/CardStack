using System;
using Client.GameLogic.CardPile;
using UnityEngine;

namespace Client.GameLogic.PlayArea
{
    [RequireComponent(typeof(DragArea))]
    public class DragCardStackPhysicsManager : MonoBehaviour
    {
        private DragArea m_dragArea;

        private void Awake()
        {
            m_dragArea = GetComponent<DragArea>();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < m_dragArea.CardStacks.Count; ++i)
            {
                var cardStack = m_dragArea.CardStacks[i];
                var cardStackAABB = cardStack.AABB;

                for (int j = 0; j < m_dragArea.CardStacks.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var targetCardStack = m_dragArea.CardStacks[j];
                    var targetCardStackAABB = targetCardStack.AABB;

                    if (cardStackAABB.IsOverlay(targetCardStackAABB))
                    {
                        CardStackPhysics.GetComponent(cardStack)?.OnCardStackTrigger(targetCardStack);
                    }
                }
            }
        }
    }
}